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
    public static async Task<List<SymbolCallerInfo>> LocateVariableCallers(Project project, SolutionCompilation solutionCompilation, ThreadRestrictedMember member)
    {
        string className = member.MemberPath.Split('.').Last();
        List<SymbolCallerInfo> result = new();
        IEnumerable<ISymbol> classDeclarations = await SymbolFinder.FindDeclarationsAsync(project, className, ignoreCase: false);

        foreach (ISymbol classDeclaration in classDeclarations)
        {
            // could be a property of the same name
            if (classDeclaration is ITypeSymbol typeDeclaration)
            {
                string? containingNamespace = typeDeclaration.ContainingNamespace.ToString();
                string symbolClassPath = containingNamespace + "." + typeDeclaration.Name;
                if (member.MemberPath == symbolClassPath)
                {
                    ImmutableArray<ISymbol> members = typeDeclaration.GetMembers(member.Name);
                    ISymbol symbol = members.First();
                    IEnumerable<SymbolCallerInfo> callers = await SymbolFinder.FindCallersAsync(symbol, project.Solution);

                    Console.WriteLine($"Found {callers.Count()} use(s) for field {member.Name}");

                    if (member is WpfVisualMember)
                        result.AddRange(callers);
                    else if (member is ObservableCollectionMember)
                    {
                        foreach (SymbolCallerInfo caller in callers)
                        {
                            bool isThreadRestricted = caller.Locations.All(location => IsThreadRestrictedCall(location));
                            
                            if (isThreadRestricted)
                                result.Add(caller);
                        }
                    }
                }
            }
        }

        return result;
    }

    private static bool IsThreadRestrictedCall(Location location)
    {
        SyntaxTree? sourceTree = location.SourceTree;
        Debug.Assert(sourceTree is not null);

        SyntaxNode rootNode = sourceTree.GetRoot();
        SyntaxNode? nodeAtLocation = rootNode.FindNode(location.SourceSpan)!;
        Debug.Assert(nodeAtLocation is not null);

        if (nodeAtLocation.Parent is SyntaxNode parent)
        {
            ExpressionSyntax callerExpression;
            ExpressionSyntax calledExpression;

            switch (parent)
            {
                case ConditionalAccessExpressionSyntax conditionalAccessExpression:
                    callerExpression = conditionalAccessExpression.Expression;
                    calledExpression = conditionalAccessExpression.WhenNotNull;

                    if (callerExpression == nodeAtLocation)
                    {
                        if (calledExpression is InvocationExpressionSyntax invocationExpression)
                            return IsThreadRestrictedCall(invocationExpression);
                    }

                    return false;

                case MemberAccessExpressionSyntax memberAccessExpression:
                    if (memberAccessExpression.Name is IdentifierNameSyntax identifierName)
                        return IsThreadRestrictedCall(identifierName);
                    return false;

                case AssignmentExpressionSyntax:
                case BinaryExpressionSyntax:
                case IsPatternExpressionSyntax:
                case ArgumentListSyntax:
                    return false;

                default:
                    throw new InvalidOperationException($"Unexpected syntax node type for thread restriction check: {parent.GetType().Name}");
            }
        }

        return true;
    }

    private static bool IsThreadRestrictedCall(InvocationExpressionSyntax invocationExpression)
    {
        ExpressionSyntax expression = invocationExpression.Expression;

        switch (expression)
        {
            case MemberBindingExpressionSyntax memberBindingExpression:
                return IsThreadRestrictedCall(memberBindingExpression);

            case MemberAccessExpressionSyntax memberAccessExpression:
                return IsThreadRestrictedCall(memberAccessExpression);

            default:
                throw new InvalidOperationException($"Unexpected expression type for thread restriction check: {expression.GetType().Name}");
        }
    }

    private static bool IsThreadRestrictedCall(MemberAccessExpressionSyntax memberAccessExpression)
    {
        ExpressionSyntax expression = memberAccessExpression.Expression;

        switch (expression)
        {
            case InvocationExpressionSyntax invocationExpression:
                return IsThreadRestrictedCall(invocationExpression);

            default:
                throw new InvalidOperationException($"Unexpected expression type for thread restriction check: {expression.GetType().Name}");
        }
    }

    private static bool IsThreadRestrictedCall(MemberBindingExpressionSyntax memberBindingExpression)
    {
        SimpleNameSyntax name = memberBindingExpression.Name;

        if (name is IdentifierNameSyntax identifierName)
            return IsThreadRestrictedCall(identifierName);
        else
            throw new InvalidOperationException($"Unexpected name type for thread restriction check: {name.GetType().Name}");
    }

    private static bool IsThreadRestrictedCall(IdentifierNameSyntax identifierName)
    {
        List<string> threadRestrictedCalls =
        [
            "Clear",
            "Add",
        ];
        List<string> unrestrictedCalls =
        [
            "Count",
        ];
        List<string> knownExtensionsCalls =
        [
            "First",
            "FirstOrDefault",
            "Select",
            "OrderBy",
        ];

        string text = identifierName.Identifier.Text;
        if (threadRestrictedCalls.Contains(text))
            return true;

        if (unrestrictedCalls.Contains(text))
            return false;

        if (knownExtensionsCalls.Contains(text))
            return false;

        throw new InvalidOperationException($"Unexpected name for thread restriction check: '{text}'");
    }

    public static void InitReduceCallSite(SolutionCompilation solutionCompilation, Project project)
    {
        ComponentConnectorConnect = GetComponentConnectorConnectSymbol(solutionCompilation, project);
        StyleConnectorConnect = GetStyleConnectorConnectSymbol(solutionCompilation, project);
        VisualTypeSymbol = GetVisualTypeSymbol(solutionCompilation, project);
        TaskRunSymbols = GetTaskRunSymbols(solutionCompilation, project);
        (DispatcherInvokeSymbols, DispatcherBeginInvokeSymbols, DispatcherInvokeAsyncSymbols) = GetDispatcherSymbols(solutionCompilation, project);
        ConfiguredTaskAwaitableSymbol = GetConfiguredTaskAwaitableSymbol(solutionCompilation, project);
        ConfiguredTaskAwaitableGenericSymbol = GetConfiguredTaskAwaitableGenericSymbol(solutionCompilation, project);
        DispatcherTimerAddSymbols = GetDispatcherTimerAddSymbols(solutionCompilation, project);
    }

    private static IMethodSymbol? ComponentConnectorConnect;
    private static IMethodSymbol? StyleConnectorConnect;
    public static INamedTypeSymbol? VisualTypeSymbol;
    private static ImmutableArray<IMethodSymbol> TaskRunSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherInvokeSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherBeginInvokeSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherInvokeAsyncSymbols = [];
    private static ImmutableArray<IMethodSymbol> DispatcherTimerAddSymbols = [];
    private static ITypeSymbol? ConfiguredTaskAwaitableSymbol;
    private static ITypeSymbol? ConfiguredTaskAwaitableGenericSymbol;

    public static async Task<CallSiteInfo> ReduceCallSite(SolutionCompilation solutionCompilation, List<CallerInfo> uncheckedCallers)
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
        SemanticModel semanticModel;
        bool hasNoCaller = false;
        List<SyntaxNode> callerNodes = await GetCallSyntaxNodesAsync(uncheckedCaller);
        foreach (var callerNode in callerNodes)
        {
            (SyntaxNode? rootCall, List<SyntaxNode> visitedNodes) = GetRootCall(callerNode);
            bool isHandled = false;
            SyntaxNode? previousVisitedNode = null;

            foreach (var visitedNode in visitedNodes)
            {
                if ((visitedNode is BlockSyntax || previousVisitedNode is BlockSyntax || previousVisitedNode is ElseClauseSyntax) &&
                    IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, visitedNode, previousVisitedNode, out CallSiteInfo? result))
                {
                    return result;
                }

                previousVisitedNode = visitedNode;
            }

            if (rootCall is FieldDeclarationSyntax fieldDeclaration && uncheckedCaller.CallingSymbol is IFieldSymbol fieldSymbol)
            {
                semanticModel = solutionCompilation.GetSemanticModel(fieldDeclaration.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(fieldDeclaration);
                
                // Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, fieldSymbol));
                isHandled = true;
            }

            if (rootCall is PropertyDeclarationSyntax propertyDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol propertySymbol)
            {
                semanticModel = solutionCompilation.GetSemanticModel(propertyDeclaration.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(propertyDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, propertySymbol));
                isHandled = true;
            }
            
            if (rootCall is MethodDeclarationSyntax methodDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol methodSymbol)
            {
                semanticModel = solutionCompilation.GetSemanticModel(methodDeclaration.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(methodDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, methodSymbol));
                isHandled = true;
            }

            if (rootCall is ConstructorDeclarationSyntax constructorDeclaration && uncheckedCaller.CallingSymbol is IMethodSymbol constructorSymbol)
            {
                semanticModel = solutionCompilation.GetSemanticModel(constructorDeclaration.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(constructorDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, constructorSymbol));
                isHandled = true;
            }

            if (rootCall is IndexerDeclarationSyntax indexerDeclaration && uncheckedCaller.CallingSymbol is IPropertySymbol indexSymbol)
            {
                semanticModel = solutionCompilation.GetSemanticModel(indexerDeclaration.SyntaxTree);
                var symbol = semanticModel.GetDeclaredSymbol(indexerDeclaration);
                Debug.Assert(SymbolEqualityComparer.Default.Equals(symbol, indexSymbol));
                isHandled = true;
            }

            if (rootCall is LambdaExpressionSyntax lambdaExpression)
            {
                semanticModel = solutionCompilation.GetSemanticModel(lambdaExpression.SyntaxTree);
                TypeInfo typeInfo = semanticModel.GetTypeInfo(lambdaExpression);

                ExpressionSyntax? ancestorExpression = null;
                ancestorExpression ??= lambdaExpression.Ancestors().OfType<InvocationExpressionSyntax>().FirstOrDefault();
                ancestorExpression ??= lambdaExpression.Ancestors().OfType<AssignmentExpressionSyntax>().FirstOrDefault();

                if (ancestorExpression is ExpressionSyntax expression)
                {
                    semanticModel = solutionCompilation.GetSemanticModel(expression.SyntaxTree);
                    SymbolInfo InvokedSymbolInfo = semanticModel.GetSymbolInfo(expression);
                    if (InvokedSymbolInfo.Symbol is IMethodSymbol invokedSymbol)
                    {
                        CallSiteInfo? result;

                        if (IsInvalidCallSite(TaskRunSymbols, invokedSymbol, lambdaExpression, uncheckedCaller, variableName, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherInvokeSymbols, invokedSymbol, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherBeginInvokeSymbols, invokedSymbol, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherInvokeAsyncSymbols, invokedSymbol, out result))
                            return result;
                        if (IsTerminalCallSiteInfo(DispatcherTimerAddSymbols, invokedSymbol, out result))
                            return result;
                    }
                    else if (ancestorExpression is AssignmentExpressionSyntax assignmentExpression)
                    {
                        if (IsCustomActionAssignment(solutionCompilation, assignmentExpression.Left, out CustomActionMember? member))
                        {
                            return new CallSiteInfo()
                            {
                                ResolvedCallType = ResolvedCallType.Continue,
                                Caller = uncheckedCaller,
                                CustomActionMember = member,
                                Indentation = uncheckedCallerInfo.Indentation,
                                VariableName = variableName,
                            };
                        }
                    }
                }

                rootWithNoCaller = rootCall;
                hasNoCaller = true;
                break;
            }

            Debug.Assert(isHandled);
        }

        if (IsTerminalCallSite(uncheckedCaller))
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

    private static bool IsTerminalCallSite(SymbolCallerInfo caller)
    {
        if (IsComponentConnectorConnect(caller))
            return true;

        if (IsStyleConnectorConnect(caller))
            return true;

        if (IsVisualConstructor(caller))
            return true;

        return false;
    }

    private static bool IsComponentConnectorConnect(SymbolCallerInfo caller)
    {
        if (caller.CallingSymbol is IMethodSymbol uncheckedMethodCaller && ComponentConnectorConnect is not null && IsImplementationOf(uncheckedMethodCaller, ComponentConnectorConnect))
        {
            // Skip IComponentConnector.Connect, found in XAML-generated code, always called by the GUI thread.
            return true;
        }

        return false;
    }

    private static IMethodSymbol GetComponentConnectorConnectSymbol(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? componentConnectorTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(IComponentConnector));
        Debug.Assert(componentConnectorTypeSymbol is not null);

        IMethodSymbol componentConnectorConnect = componentConnectorTypeSymbol.GetMembers(nameof(IComponentConnector.Connect)).OfType<IMethodSymbol>().First();

        return componentConnectorConnect;
    }

    private static bool IsStyleConnectorConnect(SymbolCallerInfo caller)
    {
        if (caller.CallingSymbol is IMethodSymbol uncheckedMethodCaller && StyleConnectorConnect is not null && IsImplementationOf(uncheckedMethodCaller, StyleConnectorConnect))
        {
            // Skip IStyleConnector.Connect, found in XAML-generated code, always called by the GUI thread.
            return true;
        }

        return false;
    }

    private static IMethodSymbol GetStyleConnectorConnectSymbol(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? componentConnectorTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(IStyleConnector));
        Debug.Assert(componentConnectorTypeSymbol is not null);

        IMethodSymbol componentConnectorConnect = componentConnectorTypeSymbol.GetMembers(nameof(IStyleConnector.Connect)).OfType<IMethodSymbol>().First();

        return componentConnectorConnect;
    }

    private static INamedTypeSymbol GetVisualTypeSymbol(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? visualTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(Visual));
        Debug.Assert(visualTypeSymbol is not null);

        return visualTypeSymbol;
    }

    private static ImmutableArray<IMethodSymbol> GetTaskRunSymbols(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? taskTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(Task));
        Debug.Assert(taskTypeSymbol is not null);

        ImmutableArray<IMethodSymbol> taskRun = [.. taskTypeSymbol.GetMembers(nameof(Task.Run)).OfType<IMethodSymbol>()];

        return taskRun;
    }

    private static (ImmutableArray<IMethodSymbol>, ImmutableArray<IMethodSymbol>, ImmutableArray<IMethodSymbol>) GetDispatcherSymbols(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? dispatcherTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(Dispatcher));
        Debug.Assert(dispatcherTypeSymbol is not null);

        ImmutableArray<IMethodSymbol> dispatcherInvoke = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.Invoke)).OfType<IMethodSymbol>()];
        ImmutableArray<IMethodSymbol> dispatcherBeginInvoke = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.BeginInvoke)).OfType<IMethodSymbol>()];
        ImmutableArray<IMethodSymbol> dispatcherInvokeAsync = [..dispatcherTypeSymbol.GetMembers(nameof(Dispatcher.InvokeAsync)).OfType<IMethodSymbol>()];

        return (dispatcherInvoke, dispatcherBeginInvoke, dispatcherInvokeAsync);
    }

    private static ITypeSymbol GetConfiguredTaskAwaitableSymbol(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? configuredTaskAwaitableTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(ConfiguredTaskAwaitable));
        Debug.Assert(configuredTaskAwaitableTypeSymbol is not null);

        return configuredTaskAwaitableTypeSymbol;
    }

    private static ITypeSymbol GetConfiguredTaskAwaitableGenericSymbol(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? configuredTaskAwaitableTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(ConfiguredTaskAwaitable<>));
        Debug.Assert(configuredTaskAwaitableTypeSymbol != null);

        return configuredTaskAwaitableTypeSymbol;
    }

    private static ImmutableArray<IMethodSymbol> GetDispatcherTimerAddSymbols(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? taskTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(DispatcherTimer));
        Debug.Assert(taskTypeSymbol is not null);

        ImmutableArray<IMethodSymbol> taskRun = [.. taskTypeSymbol.GetMembers($"add_{nameof(DispatcherTimer.Tick)}").OfType<IMethodSymbol>()];

        return taskRun;
    }

    private static bool IsImplementationOf(IMethodSymbol candidate, IMethodSymbol interfaceMethod)
    {
        INamedTypeSymbol implementingType = candidate.ContainingType;
        IMethodSymbol? implemented = implementingType.FindImplementationForInterfaceMember(interfaceMethod) as IMethodSymbol;

        return SymbolEqualityComparer.Default.Equals(implemented, candidate);
    }

    private static bool IsVisualConstructor(SymbolCallerInfo caller)
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

    public static bool InheritFrom(ITypeSymbol candidate, ITypeSymbol ancestorType)
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

    private static async Task<List<SyntaxNode>> GetCallSyntaxNodesAsync(SymbolCallerInfo callerInfo, CancellationToken cancellationToken = default)
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
            Debug.Assert(node is not null, $"\n\n{string.Join('\n', visitedNodes.ConvertAll(n => n.GetType().Name))}");

            visitedNodes.Add(node);

            switch (node)
            {
                case ArrowExpressionClauseSyntax arrowExpression:
                    if (arrowExpression.Parent is AccessorDeclarationSyntax accessor)
                    {
                        node = node.Parent;
                        break;
                    }
                    return (arrowExpression.Parent, visitedNodes);
                case AccessorDeclarationSyntax accessorDeclaration:
                    Debug.Assert(accessorDeclaration.Parent is not null);
                    return (accessorDeclaration.Parent.Parent, visitedNodes );
                case ParenthesizedLambdaExpressionSyntax parenthesizedLambdaExpression:
                    return (parenthesizedLambdaExpression, visitedNodes );
                case SimpleLambdaExpressionSyntax simpleLambdaExpression:
                    return (simpleLambdaExpression, visitedNodes);
                case MethodDeclarationSyntax:
                case ConstructorDeclarationSyntax:
                case FieldDeclarationSyntax:
                case IndexerDeclarationSyntax:
                case PropertyDeclarationSyntax:
                    return (node, visitedNodes);
                default:
                    node = node.Parent;
                    break;
            }
        }
    }

    private static bool IsInvalidCallSite(ImmutableArray<IMethodSymbol> methodSymbols,
                                          IMethodSymbol invokedSymbol,
                                          LambdaExpressionSyntax lambdaExpression,
                                          SymbolCallerInfo uncheckedCaller,
                                          string variableName,
                                          [NotNullWhen(true)] out CallSiteInfo? result)
    {
        if (methodSymbols.Any(methodSymbol => IsMethodSymbolEqual(methodSymbol, invokedSymbol)))
        {
            var lineSpan = lambdaExpression.GetLocation().GetLineSpan();
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

    private static bool IsCustomActionAssignment(SolutionCompilation solutionCompilation, ExpressionSyntax expression, [NotNullWhen(true)] out CustomActionMember? member)
    {
        switch (expression)
        {
            case MemberAccessExpressionSyntax memberAccessExpression:
                SimpleNameSyntax name = memberAccessExpression.Name;
                SemanticModel semanticModel = solutionCompilation.GetSemanticModel(name.SyntaxTree);
                SymbolInfo symbolInfo = semanticModel.GetSymbolInfo(name);
                Debug.Assert(symbolInfo.Symbol is not null);

                member = new CustomActionMember(name.Identifier, symbolInfo.Symbol);
                return true;

            default:
                throw new InvalidOperationException($"Unexpected action assignment type: {expression.GetType().Name}");
        }
    }

    private static bool IsConfigureAwaitFalse(SolutionCompilation solutionCompilation, string variableName, SymbolCallerInfo uncheckedCaller, SyntaxNode node, SyntaxNode? previousVisitedNode, [NotNullWhen(true)] out CallSiteInfo? result)
    {
        switch (node)
        {
            case BlockSyntax block:
                foreach (var statement in block.Statements)
                {
                    if (statement == previousVisitedNode)
                        break;

                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, statement, null, out result))
                        return true;
                }

                result = null;
                return false;

            case AwaitExpressionSyntax awaitExpression:
                if (IsConfigureAwaitFalseAwaitExpression(solutionCompilation, variableName, uncheckedCaller, awaitExpression, out result))
                    return true;
                break;

            case IfStatementSyntax ifStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, ifStatement.Condition, null, out result))
                    return true;
                break;

            case ElseClauseSyntax:
                break;

            case WhileStatementSyntax whileStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, whileStatement.Condition, null, out result))
                    return true;
                break;

            case DoStatementSyntax doStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, doStatement.Condition, null, out result))
                    return true;
                break;

            case ForStatementSyntax forStatement:
                if (forStatement.Declaration is VariableDeclarationSyntax forStatementVariableDeclaration && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forStatementVariableDeclaration, null, out result))
                    return true;
                foreach (var initializer in forStatement.Initializers)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, initializer, null, out result))
                        return true;
                if (forStatement.Condition is ExpressionSyntax forCondition && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forCondition, null, out result))
                    return true;
                break;
            case VariableDeclarationSyntax variableDeclaration:
                foreach (var variableDeclarator in variableDeclaration.Variables)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, variableDeclarator, null, out result))
                        return true;
                break;
            case ExpressionStatementSyntax expressionStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, expressionStatement.Expression, null, out result))
                    return true;
                break;
            case MemberAccessExpressionSyntax memberAccessExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, memberAccessExpression.Expression, null, out result))
                    return true;
                break;
            case ReturnStatementSyntax returnStatementSyntax:
                if (returnStatementSyntax.Expression is ExpressionSyntax expression && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, expression, null, out result))
                    return true;
                break;
            case AssignmentExpressionSyntax assignmentExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, assignmentExpression.Left, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, assignmentExpression.Right, null, out result))
                    return true;
                break;
            case SwitchStatementSyntax switchStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, switchStatement.Expression, null, out result))
                    return true;
                foreach (var section in switchStatement.Sections)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, section, null, out result))
                        return true;
                break;
            case SwitchSectionSyntax switchSectionSyntax:
                foreach (var statement in switchSectionSyntax.Statements)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, statement, null, out result))
                        return true;
                break;
            case EqualsValueClauseSyntax equalsValueClause:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, equalsValueClause.Value, null, out result))
                    return true;
                break;
            case InvocationExpressionSyntax invocationExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, invocationExpression.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, invocationExpression.ArgumentList, null, out result))
                    return true;
                break;
            case ArgumentListSyntax argumentList:
                foreach (var argument in argumentList.Arguments)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, argument, null, out result))
                        return true;
                break;
            case ArgumentSyntax argument:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, argument.Expression, null, out result))
                    return true;
                break;
            case BinaryExpressionSyntax binaryExpressionSyntax:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, binaryExpressionSyntax.Left, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, binaryExpressionSyntax.Right, null, out result))
                    return true;
                break;
            case ParenthesizedExpressionSyntax parenthesizedExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, parenthesizedExpression.Expression, null, out result))
                    return true;
                break;
            case CastExpressionSyntax castExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, castExpression.Expression, null, out result))
                    return true;
                break;
            case VariableDeclaratorSyntax variableDeclarator:
                if (variableDeclarator.Initializer is EqualsValueClauseSyntax initializerEqualsValueClause && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, initializerEqualsValueClause, null, out result))
                    return true;
                break;
            case ObjectCreationExpressionSyntax objectCreationExpression:
                if (objectCreationExpression.ArgumentList is ArgumentListSyntax objectCreationExpressionArgumentList && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, objectCreationExpressionArgumentList, null, out result))
                    return true;
                if (objectCreationExpression.Initializer is InitializerExpressionSyntax objectCreationExpressionInitializer && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, objectCreationExpressionInitializer, null, out result))
                    return true;
                break;
            case ImplicitObjectCreationExpressionSyntax implicitObjectCreationExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, implicitObjectCreationExpression.ArgumentList, null, out result))
                    return true;
                if (implicitObjectCreationExpression.Initializer is InitializerExpressionSyntax implicitObjectCreationExpressionInitializer && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, implicitObjectCreationExpressionInitializer, null, out result))
                    return true;
                break;
            case ArrayCreationExpressionSyntax arrayCreationExpression:
                if (arrayCreationExpression.Initializer is InitializerExpressionSyntax arrayInitializer && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, arrayInitializer, null, out result))
                    return true;
                break;
            case InitializerExpressionSyntax initializerExpression:
                foreach (var initExpression in initializerExpression.Expressions)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, initExpression, null, out result))
                        return true;
                break;
            case LocalDeclarationStatementSyntax localDeclarationStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, localDeclarationStatement.Declaration, null, out result))
                    return true;
                break;
            case TryStatementSyntax tryStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, tryStatement.Block, null, out result))
                    return true;
                foreach (var catchClause in tryStatement.Catches)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, catchClause, null, out result))
                        return true;
                if (tryStatement.Finally is not null && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, tryStatement.Finally, null, out result))
                    return true;
                break;
            case CatchClauseSyntax catchClause:
                if (catchClause.Declaration is CatchDeclarationSyntax catchClauseDeclaration && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, catchClauseDeclaration, null, out result))
                    return true;
                if (catchClause.Filter is CatchFilterClauseSyntax catchFilterClause && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, catchFilterClause, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, catchClause.Block, null, out result))
                    return true;
                break;
            case CatchDeclarationSyntax:
                break;
            case CatchFilterClauseSyntax filterClause:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, filterClause.FilterExpression, null, out result))
                    return true;
                break;
            case FinallyClauseSyntax finallyClause:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, finallyClause.Block, null, out result))
                    return true;
                break;
            case ThrowStatementSyntax: /* A throw interrupts the execution flow */
            case ThrowExpressionSyntax:
                break;
            case ConditionalAccessExpressionSyntax conditionalAccessExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, conditionalAccessExpression.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, conditionalAccessExpression.WhenNotNull, null, out result))
                    return true;
                break;
            case PrefixUnaryExpressionSyntax prefixUnaryExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, prefixUnaryExpression.Operand, null, out result))
                    return true;
                break;
            case PostfixUnaryExpressionSyntax postfixUnaryExpressionSyntax:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, postfixUnaryExpressionSyntax.Operand, null, out result))
                    return true;
                break;
            case ForEachStatementSyntax forEachStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forEachStatement.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forEachStatement.Statement, null, out result))
                    return true;
                break;
            case ForEachVariableStatementSyntax forEachVariableStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forEachVariableStatement.Variable, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forEachVariableStatement.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, forEachVariableStatement.Statement, null, out result))
                    return true;
                break;
            case IsPatternExpressionSyntax patternExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, patternExpression.Expression, null, out result))
                    return true;
                break;
            case SimpleLambdaExpressionSyntax simpleLambdaExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, simpleLambdaExpression.Body, null, out result))
                    return true;
                break;
            case LockStatementSyntax lockStatement:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, lockStatement.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, lockStatement.Statement, null, out result))
                    return true;
                break;
            case ConditionalExpressionSyntax conditionalExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, conditionalExpression.Condition, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, conditionalExpression.WhenTrue, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, conditionalExpression.WhenFalse, null, out result))
                    return true;
                break;
            case ElementAccessExpressionSyntax elementAccessExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, elementAccessExpression.Expression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, elementAccessExpression.ArgumentList, null, out result))
                    return true;
                break;
            case BracketedArgumentListSyntax bracketedArgumentList:
                foreach (var argument in bracketedArgumentList.Arguments)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, argument, null, out result))
                        return true;
                break;
            case InterpolatedStringExpressionSyntax interpolatedStringExpression:
                foreach (var argument in interpolatedStringExpression.Contents)
                    if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, argument, null, out result))
                        return true;
                break;
            case InterpolationSyntax interpolation:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, interpolation.Expression, null, out result))
                    return true;
                break;
            case InterpolatedStringTextSyntax:
                break;
            case ElementBindingExpressionSyntax elementBindingExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, elementBindingExpression.ArgumentList, null, out result))
                    return true;
                break;
            case DeclarationExpressionSyntax declarationExpression:
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, declarationExpression.Designation, null, out result))
                    return true;
                break;
            case VariableDesignationSyntax:
                break;
            case UsingStatementSyntax usingStatement:
                if (usingStatement.Expression is ExpressionSyntax usingStatementExpression && IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, usingStatementExpression, null, out result))
                    return true;
                if (IsConfigureAwaitFalse(solutionCompilation, variableName, uncheckedCaller, usingStatement.Statement, null, out result))
                    return true;
                break;
            case PredefinedTypeSyntax:
            case PatternSyntax:
            case MemberBindingExpressionSyntax:
            case IdentifierNameSyntax:
            case ThisExpressionSyntax:
            case BaseExpressionSyntax:
            case LiteralExpressionSyntax:
            case TypeOfExpressionSyntax:
            case GenericNameSyntax:
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

    private static bool IsConfigureAwaitFalseAwaitExpression(SolutionCompilation solutionCompilation, string variableName, SymbolCallerInfo uncheckedCaller, AwaitExpressionSyntax awaitExpression, [NotNullWhen(true)] out CallSiteInfo? result)
    {
        SemanticModel semanticModel = solutionCompilation.GetSemanticModel(awaitExpression.SyntaxTree);
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
