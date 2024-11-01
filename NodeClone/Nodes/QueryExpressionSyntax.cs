namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class QueryExpressionSyntax : ExpressionSyntax
{
    public QueryExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.QueryExpressionSyntax node, SyntaxNode? parent)
    {
        FromClause = new FromClauseSyntax(node.FromClause, this);
        Body = new QueryBodySyntax(node.Body, this);
        Parent = parent;
    }

    public FromClauseSyntax FromClause { get; }
    public QueryBodySyntax Body { get; }
    public SyntaxNode? Parent { get; }

}
