namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class BaseExpressionSyntax : InstanceExpressionSyntax
{
    public BaseExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionSyntax node, SyntaxNode? parent)
    {
        Token = node.Token;
        Parent = parent;
    }

    public SyntaxToken Token { get; }
    public SyntaxNode? Parent { get; }

}
