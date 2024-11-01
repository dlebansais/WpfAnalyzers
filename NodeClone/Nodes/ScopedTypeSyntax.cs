namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ScopedTypeSyntax : TypeSyntax
{
    public ScopedTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ScopedTypeSyntax node, SyntaxNode? parent)
    {
        ScopedKeyword = node.ScopedKeyword;
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public SyntaxToken ScopedKeyword { get; }
    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
