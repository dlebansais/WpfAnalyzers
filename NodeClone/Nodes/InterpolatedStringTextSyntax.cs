namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InterpolatedStringTextSyntax : InterpolatedStringContentSyntax
{
    public InterpolatedStringTextSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringTextSyntax node, SyntaxNode? parent)
    {
        TextToken = node.TextToken;
        Parent = parent;
    }

    public SyntaxToken TextToken { get; }
    public SyntaxNode? Parent { get; }

}
