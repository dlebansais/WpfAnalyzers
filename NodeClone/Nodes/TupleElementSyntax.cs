namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TupleElementSyntax : SyntaxNode
{
    public TupleElementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Identifier = node.Identifier;
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxNode? Parent { get; }

}
