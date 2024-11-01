namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class BinaryPatternSyntax : PatternSyntax
{
    public BinaryPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.BinaryPatternSyntax node, SyntaxNode? parent)
    {
        Left = PatternSyntax.From(node.Left, this);
        OperatorToken = node.OperatorToken;
        Right = PatternSyntax.From(node.Right, this);
        Parent = parent;
    }

    public PatternSyntax Left { get; }
    public SyntaxToken OperatorToken { get; }
    public PatternSyntax Right { get; }
    public SyntaxNode? Parent { get; }

}
