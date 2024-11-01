namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConstantPatternSyntax : PatternSyntax
{
    public ConstantPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConstantPatternSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
