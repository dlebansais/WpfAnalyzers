namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AssignmentExpressionSyntax : ExpressionSyntax
{
    public AssignmentExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AssignmentExpressionSyntax node, SyntaxNode? parent)
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
