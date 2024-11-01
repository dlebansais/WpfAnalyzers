namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class LetClauseSyntax : QueryClauseSyntax
{
    public LetClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.LetClauseSyntax node, SyntaxNode? parent)
    {
        LetKeyword = node.LetKeyword;
        Identifier = node.Identifier;
        EqualsToken = node.EqualsToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken LetKeyword { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
