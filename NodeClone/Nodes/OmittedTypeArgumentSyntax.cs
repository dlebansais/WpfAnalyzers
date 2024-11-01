namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class OmittedTypeArgumentSyntax : TypeSyntax
{
    public OmittedTypeArgumentSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax node, SyntaxNode? parent)
    {
        OmittedTypeArgumentToken = node.OmittedTypeArgumentToken;
        Parent = parent;
    }

    public SyntaxToken OmittedTypeArgumentToken { get; }
    public SyntaxNode? Parent { get; }

}
