namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ArrayCreationExpressionSyntax : ExpressionSyntax
{
    public ArrayCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ArrayCreationExpressionSyntax node, SyntaxNode? parent)
    {
        NewKeyword = node.NewKeyword;
        Type = new ArrayTypeSyntax(node.Type, this);
        Initializer = node.Initializer is null ? null : new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken NewKeyword { get; }
    public ArrayTypeSyntax Type { get; }
    public InitializerExpressionSyntax? Initializer { get; }
    public SyntaxNode? Parent { get; }

}
