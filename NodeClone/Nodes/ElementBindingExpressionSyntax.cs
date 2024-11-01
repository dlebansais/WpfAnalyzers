namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ElementBindingExpressionSyntax : ExpressionSyntax
{
    public ElementBindingExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ElementBindingExpressionSyntax node, SyntaxNode? parent)
    {
        ArgumentList = new BracketedArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public BracketedArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
