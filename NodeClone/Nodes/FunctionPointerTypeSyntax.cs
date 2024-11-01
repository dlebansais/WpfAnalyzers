namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerTypeSyntax : TypeSyntax
{
    public FunctionPointerTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax node, SyntaxNode? parent)
    {
        DelegateKeyword = node.DelegateKeyword;
        AsteriskToken = node.AsteriskToken;
        CallingConvention = node.CallingConvention is null ? null : new FunctionPointerCallingConventionSyntax(node.CallingConvention, this);
        ParameterList = new FunctionPointerParameterListSyntax(node.ParameterList, this);
        Parent = parent;
    }

    public SyntaxToken DelegateKeyword { get; }
    public SyntaxToken AsteriskToken { get; }
    public FunctionPointerCallingConventionSyntax? CallingConvention { get; }
    public FunctionPointerParameterListSyntax ParameterList { get; }
    public SyntaxNode? Parent { get; }

}
