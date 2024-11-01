namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class JoinIntoClauseSyntax : SyntaxNode
{
    public JoinIntoClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.JoinIntoClauseSyntax node, SyntaxNode? parent)
    {
        IntoKeyword = node.IntoKeyword;
        Identifier = node.Identifier;
        Parent = parent;
    }

    public SyntaxToken IntoKeyword { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxNode? Parent { get; }

}
