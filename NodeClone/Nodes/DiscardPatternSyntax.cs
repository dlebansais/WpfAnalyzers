namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DiscardPatternSyntax : PatternSyntax
{
    public DiscardPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DiscardPatternSyntax node, SyntaxNode? parent)
    {
        UnderscoreToken = node.UnderscoreToken;
        Parent = parent;
    }

    public SyntaxToken UnderscoreToken { get; }
    public SyntaxNode? Parent { get; }

}
