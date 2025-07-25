﻿namespace WpfAnalyzers;

using System.Collections.Immutable;
using Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Analyzer for rule WPFA1001: missing InitializeComponent.
/// </summary>
[DiagnosticAnalyzer(LanguageNames.CSharp)]
public class WPFA1001MissingInitializeComponents : DiagnosticAnalyzer
{
    /// <summary>
    /// Diagnostic ID for this rule.
    /// </summary>
    public const string DiagnosticId = "WPFA1001";

    private static readonly LocalizableString Title = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1001AnalyzerTitle), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString MessageFormat = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1001AnalyzerMessageFormat), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
    private static readonly LocalizableString Description = new LocalizableResourceString(nameof(AnalyzerResources.WPFA1001AnalyzerDescription), AnalyzerResources.ResourceManager, typeof(AnalyzerResources));
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
    public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => [Rule];

    /// <summary>
    /// Initializes the rule analyzer.
    /// </summary>
    /// <param name="context">The analysis context.</param>
    public override void Initialize(AnalysisContext context)
    {
        context = Contract.AssertNotNull(context);

        context.ConfigureGeneratedCodeAnalysis(GeneratedCodeAnalysisFlags.None);
        context.EnableConcurrentExecution();

        context.RegisterSyntaxNodeAction(AnalyzeNode, SyntaxKind.ConstructorDeclaration);
    }

    private void AnalyzeNode(SyntaxNodeAnalysisContext context)
    {
        AnalyzerTools.AssertSyntaxRequirements<ConstructorDeclarationSyntax>(
            context,
            LanguageVersion.CSharp7,
            AnalyzeVerifiedNode,
            new SimpleAnalysisAssertion(context => IsWpfTypeConstructor(context, (ConstructorDeclarationSyntax)context.Node)));
    }

    private static bool IsWpfTypeConstructor(SyntaxNodeAnalysisContext context, ConstructorDeclarationSyntax constructorDeclaration)
    {
        IMethodSymbol ConstructorSymbol = Contract.AssertNotNull(context.SemanticModel.GetDeclaredSymbol(constructorDeclaration));
        INamedTypeSymbol ContainingType = Contract.AssertNotNull(ConstructorSymbol.ContainingType);

        return WpfTools.IsWpfType(context, ContainingType);
    }

    private void AnalyzeVerifiedNode(SyntaxNodeAnalysisContext context, ConstructorDeclarationSyntax constructorDeclaration, IAnalysisAssertion[] analysisAssertions)
    {
        // Make sure InitializeComponent is called, otherwise issue the diagnostic. ExpressionBody case.
        if (constructorDeclaration.ExpressionBody is ArrowExpressionClauseSyntax ExpressionBody)
        {
            if (ExpressionBody.Expression is InvocationExpressionSyntax InvocationExpression &&
                InvocationExpression.Expression is IdentifierNameSyntax IdentifierName &&
                IdentifierName.Identifier.Text == "InitializeComponent" &&
                InvocationExpression.ArgumentList.Arguments.Count == 0)
            {
                // InitializeComponent is called, no diagnostic.
                return;
            }
        }

        // Make sure InitializeComponent is called, otherwise issue the diagnostic. Body case.
        if (constructorDeclaration.Body is BlockSyntax Body)
        {
            foreach (var Statement in Body.Statements)
                if (Statement is ExpressionStatementSyntax ExpressionStatement &&
                    ExpressionStatement.Expression is InvocationExpressionSyntax InvocationExpression &&
                    InvocationExpression.Expression is IdentifierNameSyntax IdentifierName &&
                    IdentifierName.Identifier.Text == "InitializeComponent" &&
                    InvocationExpression.ArgumentList.Arguments.Count == 0)
                {
                    // InitializeComponent is called, no diagnostic.
                    return;
                }
        }

        string Text = constructorDeclaration.Identifier.Text;

        context.ReportDiagnostic(Diagnostic.Create(Rule, constructorDeclaration.Identifier.GetLocation(), Text));
    }
}
