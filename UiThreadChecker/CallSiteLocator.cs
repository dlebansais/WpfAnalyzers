namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;

internal static class CallSiteLocator
{
    public static async Task<List<SymbolCallerInfo>> LocateVariableCallers(Project project, SyntaxToken variableDeclarator, string classPath)
    {
        string className = classPath.Split('.').Last();
        List<SymbolCallerInfo> result = new();
        IEnumerable<ISymbol> classDeclarations = await SymbolFinder.FindDeclarationsAsync(project, className, ignoreCase: false);

        foreach (ISymbol classDeclaration in classDeclarations)
        {
            ITypeSymbol typeDeclaration = (ITypeSymbol)classDeclaration;
            string? containingNamespace = typeDeclaration.ContainingNamespace.ToString();
            string symbolClassPath = containingNamespace + "." + typeDeclaration.Name;
            if (classPath == symbolClassPath)
            {
                ImmutableArray<ISymbol> members = typeDeclaration.GetMembers(variableDeclarator.Text);
                ISymbol symbol = members.First();
                IEnumerable<SymbolCallerInfo> callers = await SymbolFinder.FindCallersAsync(symbol, project.Solution);

                Console.WriteLine($"Found {callers.Count()} use(s) for field {variableDeclarator.Text}");

                result.AddRange(callers);
            }
        }

        return result;
    }

    public static void InitReduceCallSite(Compilation compilation)
    {
        ComponentConnectorConnect = GetComponentConnectorConnectSymbol(compilation);
        VisualTypeSymbol = GetVisualTypeSymbol(compilation);
        TaskRunSymbols = GetTaskRunSymbols(compilation);
        (DispatcherInvokeSymbols, DispatcherBeginInvokeSymbols, DispatcherInvokeAsyncSymbols) = GetDispatcherSymbols(compilation);
        ConfiguredTaskAwaitableSymbol = GetConfiguredTaskAwaitableSymbol(compilation);
        ConfiguredTaskAwaitableGenericSymbol = GetConfiguredTaskAwaitableGenericSymbol(compilation);
    }

    private static IMethodSymbol? ComponentConnectorConnect;
    private static ITypeSymbol? VisualTypeSymbol;
    private static ImmutableArray<IMethodSymbol> TaskRunSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherInvokeSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherBeginInvokeSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherInvokeAsyncSymbols = [];
    private static ITypeSymbol? ConfiguredTaskAwaitableSymbol;
    private static ITypeSymbol? ConfiguredTaskAwaitableGenericSymbol;

    public static async Task<CallSiteInfo> ReduceCallSite(Solution solution, Compilation compilation, List<CallerInfo> uncheckedCallers)
    {
        CallerInfo uncheckedCallerInfo = uncheckedCallers.First();
        uncheckedCallers.RemoveAt(0);
        string variableName = uncheckedCallerInfo.VariableName;
        SymbolCallerInfo uncheckedCaller = uncheckedCallerInfo.SymbolCaller;
        int indentation = uncheckedCallerInfo.Indentation;

        string tab = string.Empty;
        while (indentation-- > 0)
        {
            tab += "  ";
        }

        Console.WriteLine($"{tab}Checking {uncheckedCaller.CallingSymbol}");

        SyntaxNode? rootWithNoCaller = null;
        bool hasNoCaller = false;
        List<SyntaxNode> callerNodes = await GetCallSyntaxNodesAsync(uncheckedCaller, solution);
        foreach (var callerNode in callerNodes)
        {
            (SyntaxNode? rootCall, List<SyntaxNode> visitedNodes) = GetRootCall(callerNode);
            bool isHandled = false;
            SyntaxNode? previousVisitedNode = null;

            foreach (var visitedNode in visitedNodes)
            {
                if ((visitedNode is BlockSyntax || previousVisitedNode is BlockSyntax || previousVisitedNode is ElseClauseSyntax) &&
                    IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, visitedNode, previousVisitedNode, out CallSiteInfo? result))
                {
                    return result;
                }

                previousVisitedNode = visitedNode;
            }

            if (rootCall is PropertyDeclarationSyntax propertyDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol propertySymbol)
            {
                var symbol = compilation.GetSemanticModel(propertyDeclaration.SyntaxTree).GetDeclaredSymbol(propertyDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, propertySymbol));
                isHandled = true;
            }
            
            if (rootCall is MethodDeclarationSyntax methodDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol methodSymbol)
            {
                var symbol = compilation.GetSemanticModel(methodDeclaration.SyntaxTree).GetDeclaredSymbol(methodDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, methodSymbol));
                isHandled = true;
            }

            if (rootCall is ConstructorDeclarationSyntax constructorDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol constructorSymbol)
            {
                var symbol = compilation.GetSemanticModel(constructorDeclaration.SyntaxTree).GetDeclaredSymbol(constructorDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, constructorSymbol));
                isHandled = true;
            }

            if (rootCall is IndexerDeclarationSyntax indexerDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol indexSymbol)
            {
                var symbol = compilation.GetSemanticModel(indexerDeclaration.SyntaxTree).GetDeclaredSymbol(indexerDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, indexSymbol));
                isHandled = true;
            }

            if (rootCall is ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression)
            {
                TypeInfo typeInfo = compilation.GetSemanticModel(parenthesizedLambdaExpression.SyntaxTree).GetTypeInfo(parenthesizedLambdaExpression);
                if (parenthesizedLambdaExpression.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault() is InvocationExpressionSyntax invocationExpression)
                {
                    SymbolInfo InvokedSymbolInfo = compilation.GetSemanticModel(invocationExpression.SyntaxTree).GetSymbolInfo(invocationExpression);
                    if (InvokedSymbolInfo.Symbol is IMethodSymbol invokedSymbol)
                    {
                        CallSiteInfo? result;

                        if (IsInvalidCallSite(TaskRunSymbols, invokedSymbol, parenthesizedLambdaExpression, uncheckedCaller, variableName, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherInvokeSymbols, invokedSymbol, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherBeginInvokeSymbols, invokedSymbol, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherInvokeAsyncSymbols, invokedSymbol, out result))
                            return result;
                    }
                }

                rootWithNoCaller = rootCall;
                hasNoCaller = true;
                break;
            }

            Debug.Assert(isHandled);
        }

        if (IsTerminalCallSite(compilation, uncheckedCaller))
            return new CallSiteInfo() { ResolvedCallType = ResolvedCallType.Terminal };

        if (hasNoCaller)
        {
            Debug.Assert(rootWithNoCaller is not null);

            return new CallSiteInfo()
            {
                ResolvedCallType = ResolvedCallType.Unknown,
                Caller = uncheckedCaller,
                LineNumber = rootWithNoCaller.GetLocation().GetLineSpan().StartLinePosition.Line,
                VariableName = variableName,
            };
        }

        return new CallSiteInfo()
        {
            ResolvedCallType = ResolvedCallType.Continue,
            Caller = uncheckedCaller,
            Indentation = uncheckedCallerInfo.Indentation,
            VariableName = variableName,
        };
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

    private static ImmutableArray<IMethodSymbol> GetTaskRunSymbols(Compilation compilation)
    {
        Type taskType = typeof(Task);
        INamedTypeSymbol taskTypeSymbol = compilation.GetTypeByMetadataName(taskType.FullName!)!;
        ImmutableArray<IMethodSymbol> taskRun = [.. taskTypeSymbol.GetMembers(nameof(Task.Run)).OfType<IMethodSymbol>()];

        return taskRun;
    }

    private static (ImmutableArray<IMethodSymbol>, ImmutableArray<IMethodSymbol>, ImmutableArray<IMethodSymbol>) GetDispatcherSymbols(Compilation compilation)
    {
        Type dispatcherType = typeof(Dispatcher);
        INamedTypeSymbol dispatcherTypeSymbol = compilation.GetTypeByMetadataName(dispatcherType.FullName!)!;
        ImmutableArray<IMethodSymbol> dispatcherInvoke = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.Invoke)).OfType<IMethodSymbol>()];
        ImmutableArray<IMethodSymbol> dispatcherBeginInvoke = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.BeginInvoke)).OfType<IMethodSymbol>()];
        ImmutableArray<IMethodSymbol> dispatcherInvokeAsync = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.InvokeAsync)).OfType<IMethodSymbol>()];

        return (dispatcherInvoke, dispatcherBeginInvoke, dispatcherInvokeAsync);
    }

    private static ITypeSymbol GetConfiguredTaskAwaitableSymbol(Compilation compilation)
    {
        Type configuredTaskAwaitableType = typeof(ConfiguredTaskAwaitable);
        INamedTypeSymbol configuredTaskAwaitableTypeSymbol = compilation.GetTypeByMetadataName(configuredTaskAwaitableType.FullName!)!;

        return configuredTaskAwaitableTypeSymbol;
    }

    private static ITypeSymbol GetConfiguredTaskAwaitableGenericSymbol(Compilation compilation)
    {
        Type configuredTaskAwaitableType = typeof(ConfiguredTaskAwaitable<>);
        INamedTypeSymbol configuredTaskAwaitableTypeSymbol = compilation.GetTypeByMetadataName(configuredTaskAwaitableType.FullName!)!;

        return configuredTaskAwaitableTypeSymbol;
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

    internal static bool InheritFrom(ITypeSymbol candidate, ITypeSymbol ancestorType)
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
        List<SyntaxNode> result = new();

        foreach (var location in callerInfo.Locations)
        {
            /*
            if (!location.IsInSource)
                continue;

            var document = solution.GetDocument(location.SourceTree);
            if (document == null)
                continue;
            */

            Debug.Assert(location.SourceTree is not null);

            SyntaxNode root = await location.SourceTree.GetRootAsync(cancellationToken).ConfigureAwait(false);
            SyntaxNode node = root.FindNode(location.SourceSpan);
            result.Add(node);
        }

        return result;
    }

    private static (SyntaxNode?, List<SyntaxNode>) GetRootCall(SyntaxNode caller)
    {
        SyntaxNode? node = caller;
        List<SyntaxNode> visitedNodes = new();

        while (true)
        {
            Debug.Assert(node is not null);

            visitedNodes.Add(node);

            switch (node)
            {
                case ArrowExpressionClauseSyntax arrowExpression:
                    return (arrowExpression.Parent, visitedNodes);
                case AccessorDeclarationSyntax accessorDeclaration:
                    Debug.Assert(accessorDeclaration.Parent is not null);
                    return (accessorDeclaration.Parent.Parent, visitedNodes );
                case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression:
                    return (parenthesizedLambdaExpression, visitedNodes );
                case MethodDeclarationSyntax:
                case ConstructorDeclarationSyntax:
                    return (node, visitedNodes);
                default:
                    node = node.Parent;
                    break;
            }
        }
    }

    private static bool IsInvalidCallSite(ImmutableArray<IMethodSymbol> methodSymbols,
                                          IMethodSymbol invokedSymbol,
                                          ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression,
                                          SymbolCallerInfo uncheckedCaller,
                                          string variableName,
                                          [NotNullWhen(true)] out CallSiteInfo? result)
    {
        if (methodSymbols.Any(methodSymbol => IsMethodSymbolEqual(methodSymbol, invokedSymbol)))
        {
            var lineSpan = parenthesizedLambdaExpression.GetLocation().GetLineSpan();
            result = new CallSiteInfo()
            {
                ResolvedCallType = ResolvedCallType.InvalidCaller,
                Caller = uncheckedCaller,
                LineNumber = lineSpan.StartLinePosition.Line + 1,
                VariableName = variableName,
            };

            return true;
        }

        result = null;
        return false;
    }

    private static bool IsTerminalCallSiteInfo(ImmutableArray<IMethodSymbol> methodSymbols,
                                               IMethodSymbol invokedSymbol,
                                               [NotNullWhen(true)] out CallSiteInfo? result)
    {
        if (methodSymbols.Any(methodSymbol => IsMethodSymbolEqual(methodSymbol, invokedSymbol)))
        {
            result = new CallSiteInfo() { ResolvedCallType = ResolvedCallType.Terminal };
            return true;
        }

        result = null;
        return false;
    }

    private static bool IsMethodSymbolEqual(IMethodSymbol methodSymbol, IMethodSymbol otherMethodSymbol)
    {
        if (!methodSymbol.IsGenericMethod &&
            !otherMethodSymbol.IsGenericMethod &&
            SymbolEqualityComparer.Default.Equals(otherMethodSymbol, methodSymbol))
        {
            return true;
        }

        if (methodSymbol.IsGenericMethod &&
            otherMethodSymbol.IsGenericMethod &&
            SymbolEqualityComparer.Default.Equals(otherMethodSymbol.ConstructedFrom, methodSymbol.ConstructedFrom))
        {
            return true;
        }

        return false;
    }

    private static bool IsConfigureAwaitFalse(Compilation compilation, string variableName, SymbolCallerInfo uncheckedCaller, SyntaxNode node, SyntaxNode? previousVisitedNode, [NotNullWhen(true)] out CallSiteInfo? result)
    {
        switch (node)
        {
            case BlockSyntax block:
                foreach (var statement in block.Statements)
                {
                    if (statement == previousVisitedNode)
                        break;

                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, statement, null, out result))
                        return true;
                }

                result = null;
                return false;

            case AwaitExpressionSyntax awaitExpression:
                if (IsConfigureAwaitFalseAwaitExpression(compilation, variableName, uncheckedCaller, awaitExpression, out result))
                    return true;
                break;

            case IfStatementSyntax ifStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, ifStatement.Condition, null, out result))
                    return true;
                break;

            case ElseClauseSyntax:
                break;

            case WhileStatementSyntax whileStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, whileStatement.Condition, null, out result))
                    return true;
                break;

            case DoStatementSyntax doStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, doStatement.Condition, null, out result))
                    return true;
                break;

            case ForStatementSyntax forStatement:
                if (forStatement.Declaration is VariableDeclarationSyntax forStatementVariableDeclaration && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, forStatementVariableDeclaration, null, out result))
                    return true;
                foreach (var initializer in forStatement.Initializers)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, initializer, null, out result))
                        return true;
                if (forStatement.Condition is ExpressionSyntax forCondition && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, forCondition, null, out result))
                    return true;
                break;
            case VariableDeclarationSyntax variableDeclaration:
                foreach (var variableDeclarator in variableDeclaration.Variables)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, variableDeclarator, null, out result))
                        return true;
                break;
            case ExpressionStatementSyntax expressionStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, expressionStatement.Expression, null, out result))
                    return true;
                break;
            case MemberAccessExpressionSyntax memberAccessExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, memberAccessExpression.Expression, null, out result))
                    return true;
                break;
            case ReturnStatementSyntax returnStatementSyntax:
                if (returnStatementSyntax.Expression is ExpressionSyntax expression && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, expression, null, out result))
                    return true;
                break;
            case AssignmentExpressionSyntax assignmentExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, assignmentExpression.Left, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, assignmentExpression.Right, null, out result))
                    return true;
                break;
            case SwitchStatementSyntax switchStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, switchStatement.Expression, null, out result))
                    return true;
                foreach (var section in switchStatement.Sections)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, section, null, out result))
                        return true;
                break;
            case SwitchSectionSyntax switchSectionSyntax:
                foreach (var statement in switchSectionSyntax.Statements)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, statement, null, out result))
                        return true;
                break;
            case EqualsValueClauseSyntax equalsValueClause:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, equalsValueClause.Value, null, out result))
                    return true;
                break;
            case InvocationExpressionSyntax invocationExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, invocationExpression.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, invocationExpression.ArgumentList, null, out result))
                    return true;
                break;
            case ArgumentListSyntax argumentList:
                foreach (var argument in argumentList.Arguments)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, argument, null, out result))
                        return true;
                break;
            case ArgumentSyntax argument:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, argument.Expression, null, out result))
                    return true;
                break;
            case BinaryExpressionSyntax binaryExpressionSyntax:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, binaryExpressionSyntax.Left, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, binaryExpressionSyntax.Right, null, out result))
                    return true;
                break;
            case ParenthesizedExpressionSyntax parenthesizedExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, parenthesizedExpression.Expression, null, out result))
                    return true;
                break;
            case CastExpressionSyntax castExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, castExpression.Expression, null, out result))
                    return true;
                break;
            case VariableDeclaratorSyntax variableDeclarator:
                if (variableDeclarator.Initializer is EqualsValueClauseSyntax initializerEqualsValueClause && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, initializerEqualsValueClause, null, out result))
                    return true;
                break;
            case ObjectCreationExpressionSyntax objectCreationExpression:
                if (objectCreationExpression.ArgumentList is ArgumentListSyntax objectCreationExpressionArgumentList && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, objectCreationExpressionArgumentList, null, out result))
                    return true;
                if (objectCreationExpression.Initializer is InitializerExpressionSyntax objectCreationExpressionInitializer && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, objectCreationExpressionInitializer, null, out result))
                    return true;
                break;
            case InitializerExpressionSyntax initializerExpression:
                foreach (var initExpression in initializerExpression.Expressions)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, initExpression, null, out result))
                        return true;
                break;
            case LocalDeclarationStatementSyntax localDeclarationStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, localDeclarationStatement.Declaration, null, out result))
                    return true;
                break;
            case TryStatementSyntax tryStatement:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, tryStatement.Block, null, out result))
                    return true;
                foreach (var catchClause in tryStatement.Catches)
                    if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, catchClause, null, out result))
                        return true;
                if (tryStatement.Finally is FinallyClauseSyntax finallyClause && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, finallyClause, null, out result))
                    return true;
                break;
            case CatchClauseSyntax catchClause:
                if (catchClause.Declaration is CatchDeclarationSyntax catchClauseDeclaration && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, catchClauseDeclaration, null, out result))
                    return true;
                if (catchClause.Filter is CatchFilterClauseSyntax catchFilterClause && IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, catchFilterClause, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, catchClause.Block, null, out result))
                    return true;
                break;
            case CatchDeclarationSyntax:
                break;
            case CatchFilterClauseSyntax filterClause:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, filterClause.FilterExpression, null, out result))
                    return true;
                break;
            case ThrowStatementSyntax: /* A throw interrupts the execution flow */
                break;
            case ConditionalAccessExpressionSyntax conditionalAccessExpression:
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, conditionalAccessExpression.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(compilation, variableName, uncheckedCaller, conditionalAccessExpression.WhenNotNull, null, out result))
                    return true;
                break;
            case MemberBindingExpressionSyntax:
            case IdentifierNameSyntax:
            case ThisExpressionSyntax:
            case BaseExpressionSyntax:
            case LiteralExpressionSyntax:
                break;
            case PropertyDeclarationSyntax:
            case MethodDeclarationSyntax:
            case ConstructorDeclarationSyntax:
            case IndexerDeclarationSyntax:
            case ParenthesizedLambdaExpressionSyntax:
            case ArrowExpressionClauseSyntax:
            case AccessorDeclarationSyntax:
                break;
            default:
                throw new InvalidOperationException($"Unexpected syntax node type: {node.GetType().Name}");
        }

        result = null;
        return false;
    }

    private static bool IsConfigureAwaitFalseAwaitExpression(Compilation compilation, string variableName, SymbolCallerInfo uncheckedCaller, AwaitExpressionSyntax awaitExpression, [NotNullWhen(true)] out CallSiteInfo? result)
    {
        SemanticModel semanticModel = compilation.GetSemanticModel(awaitExpression.SyntaxTree);
        var typeInfo = semanticModel.GetTypeInfo(awaitExpression.Expression);
        if (typeInfo.Type is INamedTypeSymbol namedTypeSymbol)
        {
            bool isConfiguredAwaitable = SymbolEqualityComparer.Default.Equals(namedTypeSymbol, ConfiguredTaskAwaitableSymbol);
            bool isConfiguredAwaitableGeneric = SymbolEqualityComparer.Default.Equals(namedTypeSymbol.ConstructedFrom, ConfiguredTaskAwaitableGenericSymbol);

            if (isConfiguredAwaitable || isConfiguredAwaitableGeneric)
            {
                if (awaitExpression.Expression is InvocationExpressionSyntax invocationExpression &&
                    invocationExpression.ArgumentList.Arguments.Count > 0 &&
                    invocationExpression.ArgumentList.Arguments[0].Expression is LiteralExpressionSyntax literalExpression &&
                    literalExpression.Token.Value is bool value &&
                    !value)
                {
                    var lineSpan = literalExpression.GetLocation().GetLineSpan();
                    result = new CallSiteInfo()
                    {
                        ResolvedCallType = ResolvedCallType.InvalidAwaiter,
                        Caller = uncheckedCaller,
                        LineNumber = lineSpan.StartLinePosition.Line + 1,
                        VariableName = variableName,
                    };
                    return true;
                }
            }
        }

        result = null;
        return false;
    }
}
