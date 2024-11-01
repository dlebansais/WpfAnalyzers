namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ClassOrStructConstraintSyntax : TypeParameterConstraintSyntax
{
    public ClassOrStructConstraintSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ClassOrStructConstraintSyntax node, SyntaxNode? parent)
    {
        ClassOrStructKeyword = node.ClassOrStructKeyword;
        QuestionToken = node.QuestionToken;
        Parent = parent;
    }

    public SyntaxToken ClassOrStructKeyword { get; }
    public SyntaxToken QuestionToken { get; }
    public SyntaxNode? Parent { get; }

}
