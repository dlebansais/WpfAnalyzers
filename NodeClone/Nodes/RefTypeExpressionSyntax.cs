namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RefTypeExpressionSyntax : ExpressionSyntax
{
    public RefTypeExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeExpressionSyntax node, SyntaxNode? parent)
    {
        Keyword = node.Keyword;
        OpenParenToken = node.OpenParenToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken Keyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
