namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InterpolatedStringExpressionSyntax : ExpressionSyntax
{
    public InterpolatedStringExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringExpressionSyntax node, SyntaxNode? parent)
    {
        StringStartToken = node.StringStartToken;
        Contents = Cloner.ListFrom<InterpolatedStringContentSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax>(node.Contents, parent);
        StringEndToken = node.StringEndToken;
        Parent = parent;
    }

    public SyntaxToken StringStartToken { get; }
    public SyntaxList<InterpolatedStringContentSyntax> Contents { get; }
    public SyntaxToken StringEndToken { get; }
    public SyntaxNode? Parent { get; }

}
