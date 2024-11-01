namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RelationalPatternSyntax : PatternSyntax
{
    public RelationalPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RelationalPatternSyntax node, SyntaxNode? parent)
    {
        OperatorToken = node.OperatorToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public SyntaxToken OperatorToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
