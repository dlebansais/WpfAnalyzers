namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class OrderByClauseSyntax : QueryClauseSyntax
{
    public OrderByClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.OrderByClauseSyntax node, SyntaxNode? parent)
    {
        OrderByKeyword = node.OrderByKeyword;
        Orderings = Cloner.SeparatedListFrom<OrderingSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.OrderingSyntax>(node.Orderings, parent);
        Parent = parent;
    }

    public SyntaxToken OrderByKeyword { get; }
    public SeparatedSyntaxList<OrderingSyntax> Orderings { get; }
    public SyntaxNode? Parent { get; }

}
