namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ObjectCreationExpressionSyntax : BaseObjectCreationExpressionSyntax
{
    public ObjectCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ObjectCreationExpressionSyntax node, SyntaxNode? parent)
    {
        NewKeyword = node.NewKeyword;
        Type = TypeSyntax.From(node.Type, this);
        ArgumentList = node.ArgumentList is null ? null : new ArgumentListSyntax(node.ArgumentList, this);
        Initializer = node.Initializer is null ? null : new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken NewKeyword { get; }
    public TypeSyntax Type { get; }
    public ArgumentListSyntax? ArgumentList { get; }
    public InitializerExpressionSyntax? Initializer { get; }
    public SyntaxNode? Parent { get; }

}
