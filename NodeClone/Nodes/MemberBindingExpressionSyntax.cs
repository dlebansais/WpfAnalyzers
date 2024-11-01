namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class MemberBindingExpressionSyntax : ExpressionSyntax
{
    public MemberBindingExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.MemberBindingExpressionSyntax node, SyntaxNode? parent)
    {
        OperatorToken = node.OperatorToken;
        Name = SimpleNameSyntax.From(node.Name, this);
        Parent = parent;
    }

    public SyntaxToken OperatorToken { get; }
    public SimpleNameSyntax Name { get; }
    public SyntaxNode? Parent { get; }

}
