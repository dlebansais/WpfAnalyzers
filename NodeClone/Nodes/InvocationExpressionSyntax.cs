namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class InvocationExpressionSyntax : ExpressionSyntax
{
    public InvocationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.InvocationExpressionSyntax node, SyntaxNode? parent)
    {
        Expression = ExpressionSyntax.From(node.Expression, this);
        ArgumentList = new ArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public ExpressionSyntax Expression { get; }
    public ArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
