namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ThrowExpressionSyntax : ExpressionSyntax
{
    public ThrowExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ThrowExpressionSyntax node, SyntaxNode? parent)
    {
        ThrowKeyword = node.ThrowKeyword;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken ThrowKeyword { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
