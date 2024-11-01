namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RangeExpressionSyntax : ExpressionSyntax
{
    public RangeExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RangeExpressionSyntax node, SyntaxNode? parent)
    {
        LeftOperand = node.LeftOperand is null ? null : ExpressionSyntax.From(node.LeftOperand, this);
        OperatorToken = node.OperatorToken;
        RightOperand = node.RightOperand is null ? null : ExpressionSyntax.From(node.RightOperand, this);
        Parent = parent;
    }

    public ExpressionSyntax? LeftOperand { get; }
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax? RightOperand { get; }
    public SyntaxNode? Parent { get; }

}
