namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

public class UiThreadChecker
{
    public event EventHandler<NoCallerEventArgs>? NoCallerEvent;
    public event EventHandler<BadCallerEventArgs>? BadCallerEvent;
    public event EventHandler<UnknownCallerEventArgs>? UnknownCallerEvent;

    public async Task CheckSolutionAsync(string pathToSolutionFile, string configuration, string targetFramework, string? projectName = null, Dictionary<string, string>? properties = null)
    {
        properties ??= new Dictionary<string, string>
        {
            { "DesignTimeBuild", "true" },
            { "BuildingInsideVisualStudio", "true" },
            { "AlwaysCompileMarkupFilesInSeparateDomain", "false" },
            { "ProvideCommandLineArgs", "true" },
        };

        properties.Add("Configuration", configuration);
        properties.Add("TargetFramework", targetFramework);

        MSBuildWorkspace msWorkspace = MSBuildWorkspace.Create(properties);
        Solution solution = await msWorkspace.OpenSolutionAsync(pathToSolutionFile).ConfigureAwait(false);

        foreach (Project project in solution.Projects)
            if (projectName is null || project.Name == projectName)
                await CheckProjectAsync(solution, project).ConfigureAwait(false);
    }

    public async Task CheckProjectAsync(Solution solution, Project project)
    {
        Compilation compilation = await project.GetCompilationAsync().ConfigureAwait(false) ?? throw new InvalidOperationException();

        Dictionary<VariableDeclaratorSyntax, string> restrictedObjects = await MemberLocator.LocateWpfThreadRestrictedObjects(project).ConfigureAwait(false);
        List<CallerInfo> uncheckedCallers = [];
        foreach (KeyValuePair<VariableDeclaratorSyntax, string> entry in restrictedObjects)
        {
            VariableDeclaratorSyntax variableDeclarator = entry.Key;
            string classPath = entry.Value;

            await foreach (var caller in CallSiteLocator.LocateVariableCallers(project, variableDeclarator, classPath))
                uncheckedCallers.Add(new CallerInfo(variableDeclarator.Identifier.Text, caller, 0));
        }

        CallSiteLocator.InitReduceCallSite(compilation);

        while (uncheckedCallers.Count > 0)
        {
            CallSiteInfo callSiteInfo = await CallSiteLocator.ReduceCallSite(solution, compilation, uncheckedCallers).ConfigureAwait(false);

            switch (callSiteInfo.ResolvedCallType)
            {
                case ResolvedCallType.Terminal:
                    break;
                case ResolvedCallType.Continue:
                    var newCallers = (await SymbolFinder.FindCallersAsync(callSiteInfo.Caller.CallingSymbol, solution).ConfigureAwait(false)).ToList();
                    if (newCallers.Count == 0)
                    {
                        NoCallerEvent?.Invoke(this, new NoCallerEventArgs(callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol.Name));
                    }
                    else
                    {
                        foreach (var newCaller in newCallers)
                        {
                            // If the caller is an override, it calls itself, a case we should ignore.
                            if (!SymbolEqualityComparer.Default.Equals(newCaller.CallingSymbol, callSiteInfo.Caller.CallingSymbol))
                                uncheckedCallers.Insert(0, new CallerInfo(callSiteInfo.VariableName, newCaller, callSiteInfo.Indentation + 1));
                        }
                    }

                    break;
                case ResolvedCallType.InvalidCaller:
                    BadCallerEvent?.Invoke(this, new BadCallerEventArgs(isAwaiter: false, callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol.Name, callSiteInfo.LineNumber));
                    break;
                case ResolvedCallType.InvalidAwaiter:
                    BadCallerEvent?.Invoke(this, new BadCallerEventArgs(isAwaiter: true, callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol.Name, callSiteInfo.LineNumber));
                    break;
                case ResolvedCallType.Unknown:
                    UnknownCallerEvent?.Invoke(this, new UnknownCallerEventArgs(callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol.Name, callSiteInfo.LineNumber));
                    break;
            }
        }
    }
}
