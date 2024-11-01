namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SelectClauseSyntax : SelectOrGroupClauseSyntax
{
    public SelectClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SelectClauseSyntax node, SyntaxNode? parent)
    {
        SelectKeyword = node.SelectKeyword;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken SelectKeyword { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
