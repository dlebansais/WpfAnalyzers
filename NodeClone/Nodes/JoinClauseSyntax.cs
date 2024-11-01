namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class JoinClauseSyntax : QueryClauseSyntax
{
    public JoinClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.JoinClauseSyntax node, SyntaxNode? parent)
    {
        JoinKeyword = node.JoinKeyword;
        Type = node.Type is null ? null : TypeSyntax.From(node.Type, this);
        Identifier = node.Identifier;
        InKeyword = node.InKeyword;
        InExpression = ExpressionSyntax.From(node.InExpression, this);
        OnKeyword = node.OnKeyword;
        LeftExpression = ExpressionSyntax.From(node.LeftExpression, this);
        EqualsKeyword = node.EqualsKeyword;
        RightExpression = ExpressionSyntax.From(node.RightExpression, this);
        Into = node.Into is null ? null : new JoinIntoClauseSyntax(node.Into, this);
        Parent = parent;
    }

    public SyntaxToken JoinKeyword { get; }
    public TypeSyntax? Type { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken InKeyword { get; }
    public ExpressionSyntax InExpression { get; }
    public SyntaxToken OnKeyword { get; }
    public ExpressionSyntax LeftExpression { get; }
    public SyntaxToken EqualsKeyword { get; }
    public ExpressionSyntax RightExpression { get; }
    public JoinIntoClauseSyntax? Into { get; }
    public SyntaxNode? Parent { get; }

}
