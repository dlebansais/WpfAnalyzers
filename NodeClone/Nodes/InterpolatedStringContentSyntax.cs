namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class InterpolatedStringContentSyntax : SyntaxNode
{
    public static InterpolatedStringContentSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringContentSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.InterpolatedStringTextSyntax AsInterpolatedStringTextSyntax => new InterpolatedStringTextSyntax(AsInterpolatedStringTextSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.InterpolationSyntax AsInterpolationSyntax => new InterpolationSyntax(AsInterpolationSyntax, parent),
            _ => null!,
        };
    }
}
