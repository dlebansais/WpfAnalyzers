namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InterpolationSyntax : InterpolatedStringContentSyntax
{
    public InterpolationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationSyntax node, SyntaxNode? parent)
    {
        OpenBraceToken = node.OpenBraceToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        AlignmentClause = node.AlignmentClause is null ? null : new InterpolationAlignmentClauseSyntax(node.AlignmentClause, this);
        FormatClause = node.FormatClause is null ? null : new InterpolationFormatClauseSyntax(node.FormatClause, this);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public SyntaxToken OpenBraceToken { get; }
    public ExpressionSyntax Expression { get; }
    public InterpolationAlignmentClauseSyntax? AlignmentClause { get; }
    public InterpolationFormatClauseSyntax? FormatClause { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
