namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FromClauseSyntax : QueryClauseSyntax
{
    public FromClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FromClauseSyntax node, SyntaxNode? parent)
    {
        FromKeyword = node.FromKeyword;
        Type = node.Type is null ? null : TypeSyntax.From(node.Type, this);
        Identifier = node.Identifier;
        InKeyword = node.InKeyword;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken FromKeyword { get; }
    public TypeSyntax? Type { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken InKeyword { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
