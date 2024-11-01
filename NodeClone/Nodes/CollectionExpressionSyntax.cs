namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CollectionExpressionSyntax : ExpressionSyntax
{
    public CollectionExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CollectionExpressionSyntax node, SyntaxNode? parent)
    {
        OpenBracketToken = node.OpenBracketToken;
        Elements = Cloner.SeparatedListFrom<CollectionElementSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.CollectionElementSyntax>(node.Elements, parent);
        CloseBracketToken = node.CloseBracketToken;
        Parent = parent;
    }

    public SyntaxToken OpenBracketToken { get; }
    public SeparatedSyntaxList<CollectionElementSyntax> Elements { get; }
    public SyntaxToken CloseBracketToken { get; }
    public SyntaxNode? Parent { get; }

}
