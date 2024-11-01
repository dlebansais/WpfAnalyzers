namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerUnmanagedCallingConventionSyntax : SyntaxNode
{
    public FunctionPointerUnmanagedCallingConventionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax node, SyntaxNode? parent)
    {
        Name = node.Name;
        Parent = parent;
    }

    public SyntaxToken Name { get; }
    public SyntaxNode? Parent { get; }

}
