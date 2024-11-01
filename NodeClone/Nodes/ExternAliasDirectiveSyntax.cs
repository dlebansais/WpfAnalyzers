namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ExternAliasDirectiveSyntax : SyntaxNode
{
    public ExternAliasDirectiveSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax node, SyntaxNode? parent)
    {
        ExternKeyword = node.ExternKeyword;
        AliasKeyword = node.AliasKeyword;
        Identifier = node.Identifier;
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxToken ExternKeyword { get; }
    public SyntaxToken AliasKeyword { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
