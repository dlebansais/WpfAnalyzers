namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConstructorConstraintSyntax : TypeParameterConstraintSyntax
{
    public ConstructorConstraintSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorConstraintSyntax node, SyntaxNode? parent)
    {
        NewKeyword = node.NewKeyword;
        OpenParenToken = node.OpenParenToken;
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken NewKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
