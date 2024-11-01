namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class WhenClauseSyntax : SyntaxNode
{
    public WhenClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.WhenClauseSyntax node, SyntaxNode? parent)
    {
        WhenKeyword = node.WhenKeyword;
        Condition = ExpressionSyntax.From(node.Condition, this);
        Parent = parent;
    }

    public SyntaxToken WhenKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxNode? Parent { get; }

}
