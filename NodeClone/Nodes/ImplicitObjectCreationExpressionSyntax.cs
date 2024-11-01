namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ImplicitObjectCreationExpressionSyntax : BaseObjectCreationExpressionSyntax
{
    public ImplicitObjectCreationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitObjectCreationExpressionSyntax node, SyntaxNode? parent)
    {
        NewKeyword = node.NewKeyword;
        ArgumentList = new ArgumentListSyntax(node.ArgumentList, this);
        Initializer = node.Initializer is null ? null : new InitializerExpressionSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken NewKeyword { get; }
    public ArgumentListSyntax ArgumentList { get; }
    public InitializerExpressionSyntax? Initializer { get; }
    public SyntaxNode? Parent { get; }

}
