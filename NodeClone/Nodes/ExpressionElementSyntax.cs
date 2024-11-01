namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExpressionElementSyntax : CollectionElementSyntax
{
    public ExpressionElementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionElementSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
