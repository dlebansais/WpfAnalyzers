namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EqualsValueClauseSyntax : SyntaxNode
{
    public EqualsValueClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.EqualsValueClauseSyntax node, SyntaxNode? parent)
    {
        EqualsToken = node.EqualsToken;
        Value = ExpressionSyntax.From(node.Value, this);
        Parent = parent;
    }

    public SyntaxToken EqualsToken { get; }
    public ExpressionSyntax Value { get; }
    public SyntaxNode? Parent { get; }

}
