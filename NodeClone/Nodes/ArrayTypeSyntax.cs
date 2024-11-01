namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ArrayTypeSyntax : TypeSyntax
{
    public ArrayTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax node, SyntaxNode? parent)
    {
        ElementType = TypeSyntax.From(node.ElementType, this);
        RankSpecifiers = Cloner.ListFrom<ArrayRankSpecifierSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArrayRankSpecifierSyntax>(node.RankSpecifiers, parent);
        Parent = parent;
    }

    public TypeSyntax ElementType { get; }
    public SyntaxList<ArrayRankSpecifierSyntax> RankSpecifiers { get; }
    public SyntaxNode? Parent { get; }

}
