namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerCallingConventionSyntax : SyntaxNode
{
    public FunctionPointerCallingConventionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerCallingConventionSyntax node, SyntaxNode? parent)
    {
        ManagedOrUnmanagedKeyword = node.ManagedOrUnmanagedKeyword;
        UnmanagedCallingConventionList = node.UnmanagedCallingConventionList is null ? null : new FunctionPointerUnmanagedCallingConventionListSyntax(node.UnmanagedCallingConventionList, this);
        Parent = parent;
    }

    public SyntaxToken ManagedOrUnmanagedKeyword { get; }
    public FunctionPointerUnmanagedCallingConventionListSyntax? UnmanagedCallingConventionList { get; }
    public SyntaxNode? Parent { get; }

}
