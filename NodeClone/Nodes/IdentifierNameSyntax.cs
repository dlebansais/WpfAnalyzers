namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class IdentifierNameSyntax : SimpleNameSyntax
{
    public IdentifierNameSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax node, SyntaxNode? parent)
    {
        Identifier = node.Identifier;
        Parent = parent;
    }

    public SyntaxToken Identifier { get; }
    public SyntaxNode? Parent { get; }

}
