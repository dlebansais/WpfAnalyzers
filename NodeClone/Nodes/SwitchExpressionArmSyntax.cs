namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SwitchExpressionArmSyntax : SyntaxNode
{
    public SwitchExpressionArmSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax node, SyntaxNode? parent)
    {
        Pattern = PatternSyntax.From(node.Pattern, this);
        WhenClause = node.WhenClause is null ? null : new WhenClauseSyntax(node.WhenClause, this);
        EqualsGreaterThanToken = node.EqualsGreaterThanToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public PatternSyntax Pattern { get; }
    public WhenClauseSyntax? WhenClause { get; }
    public SyntaxToken EqualsGreaterThanToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
