namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SimpleBaseTypeSyntax : BaseTypeSyntax
{
    public SimpleBaseTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
