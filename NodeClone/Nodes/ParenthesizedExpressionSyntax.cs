namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParenthesizedExpressionSyntax : ExpressionSyntax
{
    public ParenthesizedExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedExpressionSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
