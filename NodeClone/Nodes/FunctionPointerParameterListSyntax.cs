namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerParameterListSyntax : SyntaxNode
{
    public FunctionPointerParameterListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterListSyntax node, SyntaxNode? parent)
    {
        LessThanToken = node.LessThanToken;
        Parameters = Cloner.SeparatedListFrom<FunctionPointerParameterSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax>(node.Parameters, parent);
        GreaterThanToken = node.GreaterThanToken;
        Parent = parent;
    }

    public SyntaxToken LessThanToken { get; }
    public SeparatedSyntaxList<FunctionPointerParameterSyntax> Parameters { get; }
    public SyntaxToken GreaterThanToken { get; }
    public SyntaxNode? Parent { get; }

}
