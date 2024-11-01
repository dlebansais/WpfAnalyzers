namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class NullableTypeSyntax : TypeSyntax
{
    public NullableTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax node, SyntaxNode? parent)
    {
        ElementType = TypeSyntax.From(node.ElementType, this);
        QuestionToken = node.QuestionToken;
        Parent = parent;
    }

    public TypeSyntax ElementType { get; }
    public SyntaxToken QuestionToken { get; }
    public SyntaxNode? Parent { get; }

}
