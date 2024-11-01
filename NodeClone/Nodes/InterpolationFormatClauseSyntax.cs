namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InterpolationFormatClauseSyntax : SyntaxNode
{
    public InterpolationFormatClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationFormatClauseSyntax node, SyntaxNode? parent)
    {
        ColonToken = node.ColonToken;
        FormatStringToken = node.FormatStringToken;
        Parent = parent;
    }

    public SyntaxToken ColonToken { get; }
    public SyntaxToken FormatStringToken { get; }
    public SyntaxNode? Parent { get; }

}
