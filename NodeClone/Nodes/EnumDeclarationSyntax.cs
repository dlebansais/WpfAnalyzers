namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EnumDeclarationSyntax : BaseTypeDeclarationSyntax
{
    public EnumDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.EnumDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        EnumKeyword = node.EnumKeyword;
        Identifier = node.Identifier;
        BaseList = node.BaseList is null ? null : new BaseListSyntax(node.BaseList, this);
        OpenBraceToken = node.OpenBraceToken;
        Members = Cloner.SeparatedListFrom<EnumMemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.EnumMemberDeclarationSyntax>(node.Members, parent);
        CloseBraceToken = node.CloseBraceToken;
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken EnumKeyword { get; }
    public SyntaxToken Identifier { get; }
    public BaseListSyntax? BaseList { get; }
    public SyntaxToken OpenBraceToken { get; }
    public SeparatedSyntaxList<EnumMemberDeclarationSyntax> Members { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
