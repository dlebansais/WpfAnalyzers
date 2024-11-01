namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class IncompleteMemberSyntax : MemberDeclarationSyntax
{
    public IncompleteMemberSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.IncompleteMemberSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Type = node.Type is null ? null : TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public TypeSyntax? Type { get; }
    public SyntaxNode? Parent { get; }

}
