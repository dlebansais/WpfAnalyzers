namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class InstanceExpressionSyntax : ExpressionSyntax
{
    public static InstanceExpressionSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.InstanceExpressionSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.ThisExpressionSyntax AsThisExpressionSyntax => new ThisExpressionSyntax(AsThisExpressionSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.BaseExpressionSyntax AsBaseExpressionSyntax => new BaseExpressionSyntax(AsBaseExpressionSyntax, parent),
            _ => null!,
        };
    }
}
