namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class BinaryExpressionSyntax : ExpressionSyntax
{
    public BinaryExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.BinaryExpressionSyntax node, SyntaxNode? parent)
    {
        Left = ExpressionSyntax.From(node.Left, this);
        OperatorToken = node.OperatorToken;
        Right = ExpressionSyntax.From(node.Right, this);
        Parent = parent;
    }

    public ExpressionSyntax Left { get; }
    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Right { get; }
    public SyntaxNode? Parent { get; }

}
