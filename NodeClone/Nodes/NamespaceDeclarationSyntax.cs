namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class NamespaceDeclarationSyntax : BaseNamespaceDeclarationSyntax
{
    public NamespaceDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.NamespaceDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        NamespaceKeyword = node.NamespaceKeyword;
        Name = NameSyntax.From(node.Name, this);
        OpenBraceToken = node.OpenBraceToken;
        Externs = Cloner.ListFrom<ExternAliasDirectiveSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax>(node.Externs, parent);
        Usings = Cloner.ListFrom<UsingDirectiveSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax>(node.Usings, parent);
        Members = Cloner.ListFrom<MemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax>(node.Members, parent);
        CloseBraceToken = node.CloseBraceToken;
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken NamespaceKeyword { get; }
    public NameSyntax Name { get; }
    public SyntaxToken OpenBraceToken { get; }
    public SyntaxList<ExternAliasDirectiveSyntax> Externs { get; }
    public SyntaxList<UsingDirectiveSyntax> Usings { get; }
    public SyntaxList<MemberDeclarationSyntax> Members { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
