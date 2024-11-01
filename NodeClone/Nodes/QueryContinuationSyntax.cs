namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class QueryContinuationSyntax : SyntaxNode
{
    public QueryContinuationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.QueryContinuationSyntax node, SyntaxNode? parent)
    {
        IntoKeyword = node.IntoKeyword;
        Identifier = node.Identifier;
        Body = new QueryBodySyntax(node.Body, this);
        Parent = parent;
    }

    public SyntaxToken IntoKeyword { get; }
    public SyntaxToken Identifier { get; }
    public QueryBodySyntax Body { get; }
    public SyntaxNode? Parent { get; }

}
