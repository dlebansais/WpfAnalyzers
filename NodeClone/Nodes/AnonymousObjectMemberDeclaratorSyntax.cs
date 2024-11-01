namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AnonymousObjectMemberDeclaratorSyntax : SyntaxNode
{
    public AnonymousObjectMemberDeclaratorSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax node, SyntaxNode? parent)
    {
        NameEquals = node.NameEquals is null ? null : new NameEqualsSyntax(node.NameEquals, this);
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public NameEqualsSyntax? NameEquals { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
