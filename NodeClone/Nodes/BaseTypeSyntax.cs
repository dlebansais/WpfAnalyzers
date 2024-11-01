namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class BaseTypeSyntax : SyntaxNode
{
    public static BaseTypeSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.SimpleBaseTypeSyntax AsSimpleBaseTypeSyntax => new SimpleBaseTypeSyntax(AsSimpleBaseTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax AsPrimaryConstructorBaseTypeSyntax => new PrimaryConstructorBaseTypeSyntax(AsPrimaryConstructorBaseTypeSyntax, parent),
            _ => null!,
        };
    }
}
