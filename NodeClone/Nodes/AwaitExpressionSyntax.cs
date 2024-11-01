namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AwaitExpressionSyntax : ExpressionSyntax
{
    public AwaitExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AwaitExpressionSyntax node, SyntaxNode? parent)
    {
        AwaitKeyword = node.AwaitKeyword;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken AwaitKeyword { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
