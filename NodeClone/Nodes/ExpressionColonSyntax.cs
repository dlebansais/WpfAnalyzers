namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExpressionColonSyntax : BaseExpressionColonSyntax
{
    public ExpressionColonSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionColonSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        ColonToken = node.ColonToken;
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxToken ColonToken { get; }
    public SyntaxNode? Parent { get; }

}
