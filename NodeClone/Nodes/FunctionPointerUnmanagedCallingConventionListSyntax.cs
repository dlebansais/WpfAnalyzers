namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerUnmanagedCallingConventionListSyntax : SyntaxNode
{
    public FunctionPointerUnmanagedCallingConventionListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionListSyntax node, SyntaxNode? parent)
    {
        OpenBracketToken = node.OpenBracketToken;
        CallingConventions = Cloner.SeparatedListFrom<FunctionPointerUnmanagedCallingConventionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerUnmanagedCallingConventionSyntax>(node.CallingConventions, parent);
        CloseBracketToken = node.CloseBracketToken;
        Parent = parent;
    }

    public SyntaxToken OpenBracketToken { get; }
    public SeparatedSyntaxList<FunctionPointerUnmanagedCallingConventionSyntax> CallingConventions { get; }
    public SyntaxToken CloseBracketToken { get; }
    public SyntaxNode? Parent { get; }

}
