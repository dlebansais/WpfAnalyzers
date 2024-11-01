namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ImplicitStackAllocArrayCreationExpressionSyntax : ExpressionSyntax
{
    public ImplicitStackAllocArrayCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitStackAllocArrayCreationExpressionSyntax node, SyntaxNode? parent)
    {
        StackAllocKeyword = node.StackAllocKeyword;
        OpenBracketToken = node.OpenBracketToken;
        CloseBracketToken = node.CloseBracketToken;
        Initializer = new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken StackAllocKeyword { get; }
    public SyntaxToken OpenBracketToken { get; }
    public SyntaxToken CloseBracketToken { get; }
    public InitializerExpressionSyntax Initializer { get; }
    public SyntaxNode? Parent { get; }

}
