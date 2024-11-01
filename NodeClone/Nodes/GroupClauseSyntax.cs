namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class GroupClauseSyntax : SelectOrGroupClauseSyntax
{
    public GroupClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.GroupClauseSyntax node, SyntaxNode? parent)
    {
        GroupKeyword = node.GroupKeyword;
        GroupExpression = ExpressionSyntax.From(node.GroupExpression, this);
        ByKeyword = node.ByKeyword;
        ByExpression = ExpressionSyntax.From(node.ByExpression, this);
        Parent = parent;
    }

    public SyntaxToken GroupKeyword { get; }
    public ExpressionSyntax GroupExpression { get; }
    public SyntaxToken ByKeyword { get; }
    public ExpressionSyntax ByExpression { get; }
    public SyntaxNode? Parent { get; }

}
