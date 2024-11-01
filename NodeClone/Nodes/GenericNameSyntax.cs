namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class GenericNameSyntax : SimpleNameSyntax
{
    public GenericNameSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax node, SyntaxNode? parent)
    {
        Identifier = node.Identifier;
        TypeArgumentList = new TypeArgumentListSyntax(node.TypeArgumentList, this);
        Parent = parent;
    }

    public SyntaxToken Identifier { get; }
    public TypeArgumentListSyntax TypeArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
