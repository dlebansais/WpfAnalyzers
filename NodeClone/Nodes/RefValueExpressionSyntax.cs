namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RefValueExpressionSyntax : ExpressionSyntax
{
    public RefValueExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RefValueExpressionSyntax node, SyntaxNode? parent)
    {
        Keyword = node.Keyword;
        OpenParenToken = node.OpenParenToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Comma = node.Comma;
        Type = TypeSyntax.From(node.Type, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken Keyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken Comma { get; }
    public TypeSyntax Type { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
