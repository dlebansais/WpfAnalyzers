namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Common;
using System.Diagnostics;
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
        SolutionCompilation solutionCompilation = new(solution);
        await solutionCompilation.Init();

        foreach (Project project in solution.Projects)
            if (projectName is null || project.Name == projectName)
                await CheckProjectAsync(solutionCompilation, project).ConfigureAwait(false);
    }

    private static bool IsSameCaller(CallerInfo c1, CallerInfo c2)
    {
        if (c1.VariableName != c2.VariableName)
            return false;

        if (!SymbolEqualityComparer.Default.Equals(c1.SymbolCaller.CalledSymbol, c2.SymbolCaller.CalledSymbol))
            return false;

        List<Location> locations1 = c1.SymbolCaller.Locations.ToList();
        List<Location> locations2 = c1.SymbolCaller.Locations.ToList();

        if (locations1.Count != locations2.Count)
            return false;

        for (int i = 0; i < locations1.Count; i++)
        {
            Location location1 = locations1[i];
            Location location2 = locations2[i];

            if (location1 != location2)
                return false;
        }

        return true;
    }

    private async Task CheckProjectAsync(SolutionCompilation solutionCompilation, Project project)
    {
        if (!solutionCompilation.IsWpfProject(project))
        {
            Console.WriteLine($"Skipping {project.Name}: not a WPF project.");
            return;
        }

        List<ThreadRestrictedMember> threadRestrictedMembers = await MemberLocator.LocateWpfThreadRestrictedObjects(solutionCompilation, project).ConfigureAwait(false);
        List<CallerInfo> uncheckedCallers = [];

        foreach (ThreadRestrictedMember member in threadRestrictedMembers)
        {
            foreach (SymbolCallerInfo caller in await CallSiteLocator.LocateVariableCallers(project, solutionCompilation, member))
            {
                var locations = caller.Locations.ToList();

                for (int i = 0; i < locations.Count; i++)
                {
                    uncheckedCallers.Add(new CallerInfo(member.Name, caller, i, 0));
                }
            }
        }

        CallSiteLocator.InitReduceCallSite(solutionCompilation, project);
        List<CallerInfo> CheckedCallers = new(uncheckedCallers);

        while (uncheckedCallers.Count > 0)
        {
            CallSiteInfo callSiteInfo = await CallSiteLocator.ReduceCallSite(solutionCompilation, uncheckedCallers).ConfigureAwait(false);

            switch (callSiteInfo.ResolvedCallType)
            {
                case ResolvedCallType.Terminal:
                    break;
                case ResolvedCallType.Continue:
                    if (callSiteInfo.CustomActionMember is not null)
                    {
                        var newCallers = (await SymbolFinder.FindCallersAsync(callSiteInfo.CustomActionMember.Symbol, solutionCompilation.Solution).ConfigureAwait(false)).ToList();
                        if (newCallers.Count == 0)
                        {
                            NoCallerEvent?.Invoke(this, new NoCallerEventArgs(callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol));
                        }
                        else
                        {
                            foreach (var newCaller in newCallers)
                            {
                                var locations = newCaller.Locations.ToList();

                                for (int i = 0; i < locations.Count; i++)
                                {
                                    var location = locations[i];
                                    SyntaxTree? sourceTree = location.SourceTree;
                                    Debug.Assert(sourceTree is not null);

                                    SyntaxNode rootNode = sourceTree.GetRoot();
                                    SyntaxNode? nodeAtLocation = rootNode.FindNode(location.SourceSpan)!;
                                    Debug.Assert(nodeAtLocation is not null);

                                    if (IsCustomActionCall(nodeAtLocation))
                                    {
                                        CallerInfo newCall = new(callSiteInfo.VariableName, newCaller, i, callSiteInfo.Indentation + 1);

                                        if (!CheckedCallers.Any(c => IsSameCaller(c, newCall)))
                                        {
                                            CheckedCallers.Add(newCall);
                                            uncheckedCallers.Insert(0, newCall);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        ISymbol callingSymbol = callSiteInfo.Caller.CallingSymbol;
                        var callersCollection = await SymbolFinder.FindCallersAsync(callingSymbol, solutionCompilation.Solution).ConfigureAwait(false);
                        var newCallers = callersCollection.ToList();

                        if (newCallers.Count == 0)
                        {
                            if (!HasGuiThreadAttribute(callingSymbol))
                            {
                                NoCallerEvent?.Invoke(this, new NoCallerEventArgs(callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol));
                            }
                        }
                        else
                        {
                            foreach (SymbolCallerInfo newCaller in newCallers)
                            {
                                // If the caller is an override, it calls itself, a case we should ignore.
                                if (!SymbolEqualityComparer.Default.Equals(newCaller.CallingSymbol, callSiteInfo.Caller.CallingSymbol))
                                {
                                    var locations = newCaller.Locations.ToList();

                                    for (int i = 0; i < locations.Count; i++)
                                    {
                                        CallerInfo newCall = new(callSiteInfo.VariableName, newCaller, i, callSiteInfo.Indentation + 1);

                                        if (!CheckedCallers.Any(c => IsSameCaller(c, newCall)))
                                        {
                                            CheckedCallers.Add(newCall);
                                            uncheckedCallers.Insert(0, newCall);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    break;
                case ResolvedCallType.InvalidCaller:
                    BadCallerEvent?.Invoke(this, new BadCallerEventArgs(isAwaiter: false, callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol, callSiteInfo.LineNumber));
                    break;
                case ResolvedCallType.InvalidAwaiter:
                    BadCallerEvent?.Invoke(this, new BadCallerEventArgs(isAwaiter: true, callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol, callSiteInfo.LineNumber));
                    break;
                case ResolvedCallType.Unknown:
                    UnknownCallerEvent?.Invoke(this, new UnknownCallerEventArgs(callSiteInfo.VariableName, callSiteInfo.Caller.CallingSymbol, callSiteInfo.LineNumber));
                    break;
            }
        }
    }

    private static bool IsCustomActionCall(SyntaxNode node)
    {
        SyntaxNode? parent = node.Parent;
        Debug.Assert(parent is not null);

        switch (parent)
        {
            case MemberAccessExpressionSyntax memberAccessExpression:
                if (node == memberAccessExpression.Name)
                {
                    return IsCustomActionCall(memberAccessExpression);
                }
                throw new InvalidOperationException($"Unexpected syntax node type for thread restriction check: {parent.GetType().Name}");

            default:
                throw new InvalidOperationException($"Unexpected syntax node type for thread restriction check: {parent.GetType().Name}");
        }
    }

    private static bool IsCustomActionCall(MemberAccessExpressionSyntax memberAccessExpression)
    {
        SyntaxNode? parent = memberAccessExpression.Parent;
        Debug.Assert(parent is not null);

        switch (parent)
        {
            case InvocationExpressionSyntax invocationExpressionSyntax:
                if (memberAccessExpression == invocationExpressionSyntax.Expression)
                {
                    return true;
                }
                throw new InvalidOperationException($"Unexpected syntax node type for thread restriction check: {parent.GetType().Name}");
            
            case AssignmentExpressionSyntax:
                return false;

            default:
                throw new InvalidOperationException($"Unexpected syntax node type for thread restriction check: {parent.GetType().Name}");
        }
    }

    private static bool HasGuiThreadAttribute(ISymbol callingSymbol)
    {
        foreach (var location in callingSymbol.Locations)
        {
            SyntaxTree? sourceTree = location.SourceTree;
            Debug.Assert(sourceTree is not null);

            SyntaxNode rootNode = sourceTree.GetRoot();
            SyntaxNode? nodeAtLocation = rootNode.FindNode(location.SourceSpan)!;
            Debug.Assert(nodeAtLocation is not null);

            if (HasGuiThreadAttribute(nodeAtLocation))
                return true;
        }

        return false;
    }

    private static bool HasGuiThreadAttribute(SyntaxNode node)
    {
        if (node is MemberDeclarationSyntax memberDeclaration)
        {
            foreach (var attributeList in memberDeclaration.AttributeLists)
            {
                foreach (var attribute in attributeList.Attributes)
                {
                    if (IsGuiThreadAttribute(attribute))
                        return true;
                }
            }
        }

        return false;
    }

    private static bool IsGuiThreadAttribute(AttributeSyntax attribute)
    {
        return attribute.Name is IdentifierNameSyntax identifierName &&
               WithAttributeSuffix(identifierName.Identifier.Text) == nameof(CategoryAttribute) &&
               attribute.ArgumentList is not null &&
               attribute.ArgumentList.Arguments.Count == 1 &&
               attribute.ArgumentList.Arguments.First() is AttributeArgumentSyntax attributeArgument &&
               attributeArgument.Expression is LiteralExpressionSyntax literalExpression &&
               literalExpression.Token.Text == "\"GuiThread\"";
    }

    private static string WithAttributeSuffix(string text)
    {
        return $"{text}Attribute";
    }
}
