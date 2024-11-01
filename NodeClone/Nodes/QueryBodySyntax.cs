namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class QueryBodySyntax : SyntaxNode
{
    public QueryBodySyntax(Microsoft.CodeAnalysis.CSharp.Syntax.QueryBodySyntax node, SyntaxNode? parent)
    {
        Clauses = Cloner.ListFrom<QueryClauseSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.QueryClauseSyntax>(node.Clauses, parent);
        SelectOrGroup = SelectOrGroupClauseSyntax.From(node.SelectOrGroup, this);
        Continuation = node.Continuation is null ? null : new QueryContinuationSyntax(node.Continuation, this);
        Parent = parent;
    }

    public SyntaxList<QueryClauseSyntax> Clauses { get; }
    public SelectOrGroupClauseSyntax SelectOrGroup { get; }
    public QueryContinuationSyntax? Continuation { get; }
    public SyntaxNode? Parent { get; }

}
