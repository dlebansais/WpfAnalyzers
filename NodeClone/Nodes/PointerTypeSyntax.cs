namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PointerTypeSyntax : TypeSyntax
{
    public PointerTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax node, SyntaxNode? parent)
    {
        ElementType = TypeSyntax.From(node.ElementType, this);
        AsteriskToken = node.AsteriskToken;
        Parent = parent;
    }

    public TypeSyntax ElementType { get; }
    public SyntaxToken AsteriskToken { get; }
    public SyntaxNode? Parent { get; }

}
