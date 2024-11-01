namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ElementAccessExpressionSyntax : ExpressionSyntax
{
    public ElementAccessExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ElementAccessExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        ArgumentList = new BracketedArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public BracketedArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
