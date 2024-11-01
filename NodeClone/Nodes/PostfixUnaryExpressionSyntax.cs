namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PostfixUnaryExpressionSyntax : ExpressionSyntax
{
    public PostfixUnaryExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PostfixUnaryExpressionSyntax node, SyntaxNode? parent)
    {
        Operand = ExpressionSyntax.From(node.Operand, this);
        OperatorToken = node.OperatorToken;
        Parent = parent;
    }

    public ExpressionSyntax Operand { get; }
    public SyntaxToken OperatorToken { get; }
    public SyntaxNode? Parent { get; }

}
