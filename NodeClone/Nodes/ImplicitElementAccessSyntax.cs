namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ImplicitElementAccessSyntax : ExpressionSyntax
{
    public ImplicitElementAccessSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ImplicitElementAccessSyntax node, SyntaxNode? parent)
    {
        ArgumentList = new BracketedArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public BracketedArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
