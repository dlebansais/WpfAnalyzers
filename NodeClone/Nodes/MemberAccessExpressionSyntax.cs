namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class MemberAccessExpressionSyntax : ExpressionSyntax
{
    public MemberAccessExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.MemberAccessExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        OperatorToken = node.OperatorToken;
        Name = SimpleNameSyntax.From(node.Name, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken OperatorToken { get; }
    public SimpleNameSyntax Name { get; }
    public SyntaxNode? Parent { get; }

}
