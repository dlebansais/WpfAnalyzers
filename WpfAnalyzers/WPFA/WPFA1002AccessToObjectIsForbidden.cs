#pragma warning disable RS1022

namespace WpfAnalyzers;

using System.Collections.Immutable;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.FindSymbols;
using PersistentAnalysis;

/// <summary>
/// Analyzer for rule WPFA1002: access to object is forbidden.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class WPFA1002AccessToObjectIsForbidden : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic ID for this rule.
    /// </summary>
    public const string DiagnosticId = "WPFA1002";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1002AnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1002AnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1002AnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private const string Category = "Usage";

    private static readonly DiagnosticDescriptor Rule = new(DiagnosticId,
                                                            Title,
                                                            MessageFormat,
                                                            Category,
                                                            DiagnosticSeverity.Warning,
                                                            isEnabledByDefault: true,
                                                            description: Description,
                                                            AnalyzerTools.GetHelpLink(DiagnosticId));

    /// <summary>
    /// Gets the list of supported diagnostic.
    /// </summary>
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics { get { return [Rule]; } }

    /// <summary>
    /// Initializes the rule analyzer.
    /// </summary>
    /// <param name="context">The analysis context.</param>
    public override void Initialize(AnalysisContext context)
    {
        context = Contract.AssertNotNull(context);

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.SimpleMemberAccessExpression);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        AnalyzerTools.AssertSyntaxRequirements<MemberAccessExpressionSyntax>(
            context,
            LanguageVersion.CSharp7,
            AnalyzeVerifiedNode);
    }

    private void AnalyzeVerifiedNode(SyntaxNodeAnalysisContext context, MemberAccessExpressionSyntax memberAccessExpression, IAnalysisAssertion[] analysisAssertions)
    {
        if (memberAccessExpression.Expression is not IdentifierNameSyntax ControlName || memberAccessExpression.Name is not IdentifierNameSyntax PropertyName)
            return;

        SyntaxNode? RootNode = WpfTools.GetBlockRoot(memberAccessExpression);

        if (!IsControlPropertyAccess(context, ControlName, PropertyName))
            return;

        ThreadType CallingThread = ThreadType.Unknown;

        if (RootNode is ConstructorDeclarationSyntax ConstructorDeclaration)
            CallingThread = GetConstructorCallingThread(context, ConstructorDeclaration);

        if (RootNode is MethodDeclarationSyntax MethodDeclaration)
            CallingThread = GetMethodCallingThread(context, MethodDeclaration);

        if (RootNode is LambdaExpressionSyntax LambdaExpression)
            CallingThread = GetLambdaCallingThread(context, LambdaExpression);

        if (RootNode is ArrowExpressionClauseSyntax ArrowExpressionClause)
            CallingThread = GetArrowExpressionCallingThread(context, ArrowExpressionClause);

        if (CallingThread != ThreadType.Other)
            return;

        string Text = memberAccessExpression.ToString();

        context.ReportDiagnostic(Diagnostic.Create(Rule, context.Node.GetLocation(), Text));
    }

    private static bool IsControlPropertyAccess(SyntaxNodeAnalysisContext context, IdentifierNameSyntax controlName, IdentifierNameSyntax propertyName)
    {
        SymbolInfo ControlSymbolInfo = context.SemanticModel.GetSymbolInfo(controlName);
        if (ControlSymbolInfo.Symbol is not ISymbol ControlSymbol)
            return false;

        if (ControlSymbol.ContainingType is not INamedTypeSymbol ContainingType)
            return false;

        if (!WpfTools.IsVisual(context, ContainingType))
            return false;

        SymbolInfo PropertySymbolInfo = context.SemanticModel.GetSymbolInfo(propertyName);
        if (PropertySymbolInfo.Symbol is not IPropertySymbol)
            return false;

        return true;
    }

    private static ThreadType GetConstructorCallingThread(SyntaxNodeAnalysisContext context, ConstructorDeclarationSyntax constructorDeclaration)
    {
        return ThreadType.Unknown;
    }

    private static ThreadType GetMethodCallingThread(SyntaxNodeAnalysisContext context, MethodDeclarationSyntax methodDeclaration)
    {
        return ThreadType.Unknown;
    }

    private static ThreadType GetLambdaCallingThread(SyntaxNodeAnalysisContext context, LambdaExpressionSyntax lambdaExpression)
    {
        return ThreadType.Unknown;
    }

    private static ThreadType GetArrowExpressionCallingThread(SyntaxNodeAnalysisContext context, ArrowExpressionClauseSyntax arrowExpressionClause)
    {
        return ThreadType.Unknown;
    }
}
