namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SlicePatternSyntax : PatternSyntax
{
    public SlicePatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SlicePatternSyntax node, SyntaxNode? parent)
    {
        DotDotToken = node.DotDotToken;
        Pattern = node.Pattern is null ? null : PatternSyntax.From(node.Pattern, this);
        Parent = parent;
    }

    public SyntaxToken DotDotToken { get; }
    public PatternSyntax? Pattern { get; }
    public SyntaxNode? Parent { get; }

}
