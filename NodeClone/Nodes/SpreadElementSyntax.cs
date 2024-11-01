namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SpreadElementSyntax : CollectionElementSyntax
{
    public SpreadElementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SpreadElementSyntax node, SyntaxNode? parent)
    {
        OperatorToken = node.OperatorToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
