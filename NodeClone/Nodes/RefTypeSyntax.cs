namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RefTypeSyntax : TypeSyntax
{
    public RefTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax node, SyntaxNode? parent)
    {
        RefKeyword = node.RefKeyword;
        ReadOnlyKeyword = node.ReadOnlyKeyword;
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public SyntaxToken RefKeyword { get; }
    public SyntaxToken ReadOnlyKeyword { get; }
    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
