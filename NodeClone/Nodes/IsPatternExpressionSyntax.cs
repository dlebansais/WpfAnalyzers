namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class IsPatternExpressionSyntax : ExpressionSyntax
{
    public IsPatternExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.IsPatternExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        IsKeyword = node.IsKeyword;
        Pattern = PatternSyntax.From(node.Pattern, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken IsKeyword { get; }
    public PatternSyntax Pattern { get; }
    public SyntaxNode? Parent { get; }

}
