namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class NameEqualsSyntax : SyntaxNode
{
    public NameEqualsSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.NameEqualsSyntax node, SyntaxNode? parent)
    {
        Name = new IdentifierNameSyntax(node.Name, this);
        EqualsToken = node.EqualsToken;
        Parent = parent;
    }

    public IdentifierNameSyntax Name { get; }
    public SyntaxToken EqualsToken { get; }
    public SyntaxNode? Parent { get; }

}
