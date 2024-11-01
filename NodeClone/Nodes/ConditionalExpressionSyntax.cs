namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConditionalExpressionSyntax : ExpressionSyntax
{
    public ConditionalExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConditionalExpressionSyntax node, SyntaxNode? parent)
    {
        Condition = ExpressionSyntax.From(node.Condition, this);
        QuestionToken = node.QuestionToken;
        WhenTrue = ExpressionSyntax.From(node.WhenTrue, this);
        ColonToken = node.ColonToken;
        WhenFalse = ExpressionSyntax.From(node.WhenFalse, this);
        Parent = parent;
    }

    public ExpressionSyntax Condition { get; }
    public SyntaxToken QuestionToken { get; }
    public ExpressionSyntax WhenTrue { get; }
    public SyntaxToken ColonToken { get; }
    public ExpressionSyntax WhenFalse { get; }
    public SyntaxNode? Parent { get; }

}
