namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AnonymousObjectCreationExpressionSyntax : ExpressionSyntax
{
    public AnonymousObjectCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectCreationExpressionSyntax node, SyntaxNode? parent)
    {
        NewKeyword = node.NewKeyword;
        OpenBraceToken = node.OpenBraceToken;
        Initializers = Cloner.SeparatedListFrom<AnonymousObjectMemberDeclaratorSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousObjectMemberDeclaratorSyntax>(node.Initializers, parent);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public SyntaxToken NewKeyword { get; }
    public SyntaxToken OpenBraceToken { get; }
    public SeparatedSyntaxList<AnonymousObjectMemberDeclaratorSyntax> Initializers { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
