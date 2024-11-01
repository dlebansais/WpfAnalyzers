namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class BaseParameterListSyntax : SyntaxNode
{
    public static BaseParameterListSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterListSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax AsParameterListSyntax => new ParameterListSyntax(AsParameterListSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.BracketedParameterListSyntax AsBracketedParameterListSyntax => new BracketedParameterListSyntax(AsBracketedParameterListSyntax, parent),
            _ => null!,
        };
    }
}
