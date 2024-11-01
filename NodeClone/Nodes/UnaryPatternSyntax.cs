namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class UnaryPatternSyntax : PatternSyntax
{
    public UnaryPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.UnaryPatternSyntax node, SyntaxNode? parent)
    {
        OperatorToken = node.OperatorToken;
        Pattern = PatternSyntax.From(node.Pattern, this);
        Parent = parent;
    }

    public SyntaxToken OperatorToken { get; }
    public PatternSyntax Pattern { get; }
    public SyntaxNode? Parent { get; }

}
