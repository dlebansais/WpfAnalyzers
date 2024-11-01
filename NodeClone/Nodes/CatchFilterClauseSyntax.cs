namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CatchFilterClauseSyntax : SyntaxNode
{
    public CatchFilterClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CatchFilterClauseSyntax node, SyntaxNode? parent)
    {
        WhenKeyword = node.WhenKeyword;
        OpenParenToken = node.OpenParenToken;
        FilterExpression = ExpressionSyntax.From(node.FilterExpression, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken WhenKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax FilterExpression { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
