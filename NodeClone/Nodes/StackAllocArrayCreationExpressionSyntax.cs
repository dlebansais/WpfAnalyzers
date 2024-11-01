namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class StackAllocArrayCreationExpressionSyntax : ExpressionSyntax
{
    public StackAllocArrayCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.StackAllocArrayCreationExpressionSyntax node, SyntaxNode? parent)
    {
        StackAllocKeyword = node.StackAllocKeyword;
        Type = TypeSyntax.From(node.Type, this);
        Initializer = node.Initializer is null ? null : new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken StackAllocKeyword { get; }
    public TypeSyntax Type { get; }
    public InitializerExpressionSyntax? Initializer { get; }
    public SyntaxNode? Parent { get; }

}
