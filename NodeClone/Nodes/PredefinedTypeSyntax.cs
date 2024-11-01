namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PredefinedTypeSyntax : TypeSyntax
{
    public PredefinedTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax node, SyntaxNode? parent)
    {
        Keyword = node.Keyword;
        Parent = parent;
    }

    public SyntaxToken Keyword { get; }
    public SyntaxNode? Parent { get; }

}
