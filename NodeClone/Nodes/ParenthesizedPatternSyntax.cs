namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParenthesizedPatternSyntax : PatternSyntax
{
    public ParenthesizedPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedPatternSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Pattern = PatternSyntax.From(node.Pattern, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public PatternSyntax Pattern { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
