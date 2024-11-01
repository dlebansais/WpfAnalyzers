namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CatchDeclarationSyntax : SyntaxNode
{
    public CatchDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CatchDeclarationSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Type = TypeSyntax.From(node.Type, this);
        Identifier = node.Identifier;
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public TypeSyntax Type { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
