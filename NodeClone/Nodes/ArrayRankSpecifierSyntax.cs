namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ArrayRankSpecifierSyntax : SyntaxNode
{
    public ArrayRankSpecifierSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax node, SyntaxNode? parent)
    {
        OpenBracketToken = node.OpenBracketToken;
        Sizes = Cloner.SeparatedListFrom<ExpressionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax>(node.Sizes, parent);
        CloseBracketToken = node.CloseBracketToken;
        Parent = parent;
    }

    public SyntaxToken OpenBracketToken { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Sizes { get; }
    public SyntaxToken CloseBracketToken { get; }
    public SyntaxNode? Parent { get; }

}
