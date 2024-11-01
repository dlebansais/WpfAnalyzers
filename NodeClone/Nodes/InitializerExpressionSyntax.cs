namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InitializerExpressionSyntax : ExpressionSyntax
{
    public InitializerExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InitializerExpressionSyntax node, SyntaxNode? parent)
    {
        OpenBraceToken = node.OpenBraceToken;
        Expressions = Cloner.SeparatedListFrom<ExpressionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax>(node.Expressions, parent);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public SyntaxToken OpenBraceToken { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Expressions { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
