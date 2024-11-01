namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EnumMemberDeclarationSyntax : MemberDeclarationSyntax
{
    public EnumMemberDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Identifier = node.Identifier;
        EqualsValue = node.EqualsValue is null ? null : new EqualsValueClauseSyntax(node.EqualsValue, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken Identifier { get; }
    public EqualsValueClauseSyntax? EqualsValue { get; }
    public SyntaxNode? Parent { get; }

}
