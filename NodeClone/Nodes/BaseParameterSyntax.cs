namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class BaseParameterSyntax : SyntaxNode
{
    public static BaseParameterSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.BaseParameterSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax AsParameterSyntax => new ParameterSyntax(AsParameterSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax AsFunctionPointerParameterSyntax => new FunctionPointerParameterSyntax(AsFunctionPointerParameterSyntax, parent),
            _ => null!,
        };
    }
}
