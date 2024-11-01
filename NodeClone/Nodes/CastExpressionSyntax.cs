namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CastExpressionSyntax : ExpressionSyntax
{
    public CastExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CastExpressionSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Type = TypeSyntax.From(node.Type, this);
        CloseParenToken = node.CloseParenToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public TypeSyntax Type { get; }
    public SyntaxToken CloseParenToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
