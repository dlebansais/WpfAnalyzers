namespace WpfAnalyzers;

using System;
using Contracts;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Helper providing methods for WPF analyzers.
/// </summary>
internal static class WpfTools
{
    /// <summary>
    /// Checks whether the provided type is a WPF type.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="typeSymbol">The type symbol.</param>
    public static bool IsWpfType(SyntaxNodeAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        // The type must descend from Visual and be a direct child of a type from either PresentationCore or PresentationFramework.
        return WpfTools.IsVisual(context, typeSymbol) &&
               (WpfTools.IsPresentationCoreFirstAncestor(context, typeSymbol) || WpfTools.IsPresentationFrameworkFirstAncestor(context, typeSymbol));
    }

    /// <summary>
    /// Checks whether the provided type inherits from System.Windows.Media.Visual.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="typeSymbol">The type symbol.</param>
    public static bool IsVisual(SyntaxNodeAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        var VisualTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Windows.Media.Visual");
        var BaseType = typeSymbol.BaseType;

        while (BaseType is not null)
        {
            if (SymbolEqualityComparer.Default.Equals(BaseType, VisualTypeSymbol))
                return true;

            BaseType = BaseType.BaseType;
        }

        return false;
    }

    /// <summary>
    /// Checks whether the provided type inherits directly from a type in PresentationCore.
    /// <paramref name="typeSymbol"/> must be a descendant of System.Windows.Media.Visual.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="typeSymbol">The type symbol.</param>
    public static bool IsPresentationCoreFirstAncestor(SyntaxNodeAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        var VisualTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Windows.Media.Visual");
        var BaseType = typeSymbol.BaseType;

        // IsVisual was successful, therefore VisualTypeSymbol can't be null.
        var CoreAssemblySymbol = Contract.AssertNotNull(VisualTypeSymbol).ContainingAssembly;

        // IsVisual was successful, therefore typeSymbol has a base type.
        var BaseTypeAssembly = Contract.AssertNotNull(BaseType).ContainingAssembly;

        return SymbolEqualityComparer.Default.Equals(BaseTypeAssembly, CoreAssemblySymbol);
    }

    /// <summary>
    /// Checks whether the provided type inherits directly from a type in PresentationFramework.
    /// <paramref name="typeSymbol"/> must be a descendant of System.Windows.Media.Visual.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="typeSymbol">The type symbol.</param>
    public static bool IsPresentationFrameworkFirstAncestor(SyntaxNodeAnalysisContext context, INamedTypeSymbol typeSymbol)
    {
        var WindowTypeSymbol = context.Compilation.GetTypeByMetadataName("System.Windows.Window");
        var FrameworkAssemblySymbol = WindowTypeSymbol?.ContainingAssembly;

        var BaseType = typeSymbol.BaseType;

        // IsVisual was successful, therefore typeSymbol has a base type.
        var BaseTypeAssembly = Contract.AssertNotNull(BaseType).ContainingAssembly;

        return SymbolEqualityComparer.Default.Equals(BaseTypeAssembly, FrameworkAssemblySymbol);
    }

    /// <summary>
    /// Gets the root of the block of instructions embedding <paramref name="syntaxNode"/>.
    /// </summary>
    /// <param name="syntaxNode">The node.</param>
    public static SyntaxNode? GetBlockRoot(SyntaxNode syntaxNode)
    {
        SyntaxNode? Parent = syntaxNode.Parent;

        while (Parent is not null)
        {
            if (Parent is MethodDeclarationSyntax or
                          ConstructorDeclarationSyntax or
                          LambdaExpressionSyntax or
                          ArrowExpressionClauseSyntax)
                break;

            Parent = Parent.Parent;
        }

        return Parent;
    }
}
