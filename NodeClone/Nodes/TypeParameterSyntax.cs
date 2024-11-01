namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TypeParameterSyntax : SyntaxNode
{
    public TypeParameterSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TypeParameterSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        VarianceKeyword = node.VarianceKeyword;
        Identifier = node.Identifier;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken VarianceKeyword { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxNode? Parent { get; }

}
