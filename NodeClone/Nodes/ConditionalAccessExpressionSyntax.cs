namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConditionalAccessExpressionSyntax : ExpressionSyntax
{
    public ConditionalAccessExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalAccessExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        OperatorToken = node.OperatorToken;
        WhenNotNull = ExpressionSyntax.From(node.WhenNotNull, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax WhenNotNull { get; }
    public SyntaxNode? Parent { get; }

}
