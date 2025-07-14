namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

internal static class CallSiteLocator
{
    public static async IAsyncEnumerable<SymbolCallerInfo> LocateVariableCallers(Project project, VariableDeclaratorSyntax variableDeclarator, string classPath)
    {
        string clasName = classPath.Split('.').Last();

        IEnumerable<ISymbol> classDeclarations = await SymbolFinder.FindDeclarationsAsync(project, clasName, ignoreCase: false);

        foreach (var classDeclaration in classDeclarations)
        {
            if (classDeclaration is ITypeSymbol typeDeclaration && typeDeclaration.ContainingNamespace.ToString() is string containingNamespace)
            {
                string symbolClassPath = containingNamespace + "." + typeDeclaration.Name;
                if (classPath == symbolClassPath)
                {
                    ImmutableArray<ISymbol> members = typeDeclaration.GetMembers();
                    IFieldSymbol symbol = (IFieldSymbol)members.First(m => m.Name == variableDeclarator.Identifier.Text);
                    IEnumerable<SymbolCallerInfo> callers = await SymbolFinder.FindCallersAsync(symbol, project.Solution);

                    Console.WriteLine($"Found {callers.Count()} use(s) for field {variableDeclarator.Identifier.Text}");

                    foreach (var caller in callers)
                    {
                        yield return caller;
                    }
                }
            }
        }
    }

    public static void InitReduceCallSite(Compilation compilation)
    {
        ComponentConnectorConnect = GetComponentConnectorConnectSymbol(compilation);
        VisualTypeSymbol = GetVisualTypeSymbol(compilation);
        TaskRunSymbol = GetTaskRunSymbol(compilation);
    }

    private static IMethodSymbol? ComponentConnectorConnect;
    private static ITypeSymbol? VisualTypeSymbol;
    private static IMethodSymbol? TaskRunSymbol;

    public static async Task<CallSiteInfo> ReduceCallSite(Solution solution, Compilation compilation, List<CallerInfo> uncheckedCallers)
    {
        CallerInfo uncheckedCallerInfo = uncheckedCallers.First();
        uncheckedCallers.RemoveAt(0);
        SymbolCallerInfo uncheckedCaller = uncheckedCallerInfo.SymbolCaller;
        int indentation = uncheckedCallerInfo.Indentation;

        string tab = string.Empty;
        while (indentation-- > 0)
        {
            tab += "  ";
        }

        Console.WriteLine($"{tab}Checking {uncheckedCaller.CallingSymbol}");

        if (IsTerminalCallSite(compilation, uncheckedCaller))
            return new CallSiteInfo() { ResolvedCallType = ResolvedCallType.Terminal };

        List<SyntaxNode> callerNodes = await GetCallSyntaxNodesAsync(uncheckedCaller, solution);
        foreach (var callerNode in callerNodes)
        {
            SyntaxNode? rootCall = GetRootCall(callerNode);

            if (rootCall is PropertyDeclarationSyntax propertyDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol propertySymbol)
            {
                var symbol = compilation.GetSemanticModel(propertyDeclaration.SyntaxTree).GetDeclaredSymbol(propertyDeclaration);
                if (SymbolEqualityComparer.Default.Equals(symbol, propertySymbol))
                    continue;
            }
            else if (rootCall is MethodDeclarationSyntax methodDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol methodSymbol)
            {
                var symbol = compilation.GetSemanticModel(methodDeclaration.SyntaxTree).GetDeclaredSymbol(methodDeclaration);
                if (SymbolEqualityComparer.Default.Equals(symbol, methodSymbol))
                    continue;
            }
            else if (rootCall is ConstructorDeclarationSyntax constructorDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol constructorSymbol)
            {
                var symbol = compilation.GetSemanticModel(constructorDeclaration.SyntaxTree).GetDeclaredSymbol(constructorDeclaration);
                if (SymbolEqualityComparer.Default.Equals(symbol, constructorSymbol))
                    continue;
            }
            else if (rootCall is IndexerDeclarationSyntax indexerDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol indexSymbol)
            {
                var symbol = compilation.GetSemanticModel(indexerDeclaration.SyntaxTree).GetDeclaredSymbol(indexerDeclaration);
                if (SymbolEqualityComparer.Default.Equals(symbol, indexSymbol))
                    continue;
            }
            else if (rootCall is ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression)
            {
                var typeInfo = compilation.GetSemanticModel(parenthesizedLambdaExpression.SyntaxTree).GetTypeInfo(parenthesizedLambdaExpression);
                if (typeInfo.ConvertedType is ITypeSymbol &&
                    parenthesizedLambdaExpression.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault() is InvocationExpressionSyntax invocationExpression &&
                    compilation.GetSemanticModel(invocationExpression.SyntaxTree).GetSymbolInfo(invocationExpression).Symbol is ISymbol invokedSymbol &&
                    TaskRunSymbol is not null &&
                    SymbolEqualityComparer.Default.Equals(invokedSymbol, TaskRunSymbol))
                {
                    var lineSpan = parenthesizedLambdaExpression.GetLocation().GetLineSpan();
                    return new CallSiteInfo()
                    {
                        ResolvedCallType = ResolvedCallType.Invalid,
                        Caller = uncheckedCaller,
                        LineNumber = lineSpan.StartLinePosition.Line + 1,
                    };
                }
            }

            return new CallSiteInfo()
            {
                ResolvedCallType = ResolvedCallType.Unknown,
                Caller = uncheckedCaller,
                LineNumber = rootCall is not null
                    ? rootCall.GetLocation().GetLineSpan().StartLinePosition.Line
                    : -1,
            };
        }

        return new CallSiteInfo() { ResolvedCallType = ResolvedCallType.Continue, Caller = uncheckedCaller, Indentation = uncheckedCallerInfo.Indentation };
    }

    private static bool IsTerminalCallSite(Compilation compilation, SymbolCallerInfo caller)
    {
        if (IsComponentConnectorConnect(compilation, caller))
            return true;

        if (IsVisualConstructor(compilation, caller))
            return true;

        return false;
    }

    private static bool IsComponentConnectorConnect(Compilation compilation, SymbolCallerInfo caller)
    {
        if (caller.CallingSymbol is IMethodSymbol uncheckedMethodCaller && ComponentConnectorConnect is not null && IsImplementationOf(uncheckedMethodCaller, ComponentConnectorConnect))
        {
            // Skip IComponentConnector.Connect, found in XAML-generated code, always called by the GUI thread.
            return true;
        }

        return false;
    }

    private static IMethodSymbol GetComponentConnectorConnectSymbol(Compilation compilation)
    {
        Type componentConnectorType = typeof(IComponentConnector);
        INamedTypeSymbol componentConnectorTypeSymbol = compilation.GetTypeByMetadataName(componentConnectorType.FullName!)!;
        IMethodSymbol componentConnectorConnect = componentConnectorTypeSymbol.GetMembers(nameof(IComponentConnector.Connect)).OfType<IMethodSymbol>().First();

        return componentConnectorConnect;
    }

    private static ITypeSymbol GetVisualTypeSymbol(Compilation compilation)
    {
        Type visualType = typeof(Visual);
        INamedTypeSymbol visualTypeSymbol = compilation.GetTypeByMetadataName(visualType.FullName!)!;

        return visualTypeSymbol;
    }

    private static IMethodSymbol GetTaskRunSymbol(Compilation compilation)
    {
        Type taskType = typeof(Task);
        INamedTypeSymbol taskTypeSymbol = compilation.GetTypeByMetadataName(taskType.FullName!)!;
        IMethodSymbol taskRun = taskTypeSymbol.GetMembers(nameof(Task.Run)).OfType<IMethodSymbol>().First();

        return taskRun;
    }

    private static bool IsImplementationOf(IMethodSymbol candidate, IMethodSymbol interfaceMethod)
    {
        INamedTypeSymbol implementingType = candidate.ContainingType;
        IMethodSymbol? implemented = implementingType.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;

        return SymbolEqualityComparer.Default.Equals(implemented, candidate);
    }

    private static bool IsVisualConstructor(Compilation compilation, SymbolCallerInfo caller)
    {
        if (caller.CallingSymbol is IMethodSymbol methodSymbol &&
            methodSymbol.MethodKind == MethodKind.Constructor &&
            VisualTypeSymbol is not null &&
            InheritFrom(methodSymbol.ContainingType, VisualTypeSymbol))
        {
            return true;
        }

        return false;
    }

    private static bool InheritFrom(ITypeSymbol candidate, ITypeSymbol ancestorType)
    {
        ITypeSymbol? current = candidate;

        while (current is not null)
        {
            if (SymbolEqualityComparer.Default.Equals(current, ancestorType))
                return true;

            current = current.BaseType;
        }

        return false;
    }

    private static async Task<List<SyntaxNode>> GetCallSyntaxNodesAsync(SymbolCallerInfo callerInfo, Solution solution, CancellationToken cancellationToken = default)
    {
        var result = new List<SyntaxNode>();

        foreach (var location in callerInfo.Locations)
        {
            if (!location.IsInSource)
                continue;

            var document = solution.GetDocument(location.SourceTree);
            if (document == null)
                continue;

            var root = await location.SourceTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            var node = root.FindNode(location.SourceSpan);
            result.Add(node);
        }

        return result;
    }

    private static SyntaxNode? GetRootCall(SyntaxNode caller)
    {
        SyntaxNode? node = caller;

        while (node is not null)
        {
            switch (node)
            {
                case ArrowExpressionClauseSyntax arrowExpression:
                    return arrowExpression.Parent;
                case AccessorDeclarationSyntax accessorDeclaration:
                    return accessorDeclaration.Parent?.Parent;
                case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression:
                    return parenthesizedLambdaExpression;
                case MethodDeclarationSyntax:
                case ConstructorDeclarationSyntax:
                    return node;
                default:
                    node = node.Parent;
                    break;
            }
        }

        return null;
    }
}
