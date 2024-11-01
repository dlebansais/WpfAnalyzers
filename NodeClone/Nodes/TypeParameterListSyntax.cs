namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TypeParameterListSyntax : SyntaxNode
{
    public TypeParameterListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterListSyntax node, SyntaxNode? parent)
    {
        LessThanToken = node.LessThanToken;
        Parameters = Cloner.SeparatedListFrom<TypeParameterSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax>(node.Parameters, parent);
        GreaterThanToken = node.GreaterThanToken;
        Parent = parent;
    }

    public SyntaxToken LessThanToken { get; }
    public SeparatedSyntaxList<TypeParameterSyntax> Parameters { get; }
    public SyntaxToken GreaterThanToken { get; }
    public SyntaxNode? Parent { get; }

}
