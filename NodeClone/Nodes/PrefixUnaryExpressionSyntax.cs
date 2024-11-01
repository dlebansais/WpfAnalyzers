namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PrefixUnaryExpressionSyntax : ExpressionSyntax
{
    public PrefixUnaryExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PrefixUnaryExpressionSyntax node, SyntaxNode? parent)
    {
        OperatorToken = node.OperatorToken;
        Operand = ExpressionSyntax.From(node.Operand, this);
        Parent = parent;
    }

    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Operand { get; }
    public SyntaxNode? Parent { get; }

}
