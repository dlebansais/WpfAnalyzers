namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AttributeArgumentSyntax : SyntaxNode
{
    public AttributeArgumentSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax node, SyntaxNode? parent)
    {
        NameEquals = node.NameEquals is null ? null : new NameEqualsSyntax(node.NameEquals, this);
        NameColon = node.NameColon is null ? null : new NameColonSyntax(node.NameColon, this);
        Expression = ExpressionSyntax.From(node.Expression, this);
        Parent = parent;
    }

    public NameEqualsSyntax? NameEquals { get; }
    public NameColonSyntax? NameColon { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxNode? Parent { get; }

}
