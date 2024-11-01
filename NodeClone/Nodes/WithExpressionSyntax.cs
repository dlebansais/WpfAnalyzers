namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class WithExpressionSyntax : ExpressionSyntax
{
    public WithExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.WithExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        WithKeyword = node.WithKeyword;
        Initializer = new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken WithKeyword { get; }
    public InitializerExpressionSyntax Initializer { get; }
    public SyntaxNode? Parent { get; }

}
