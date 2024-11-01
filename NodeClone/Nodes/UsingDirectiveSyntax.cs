namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class UsingDirectiveSyntax : SyntaxNode
{
    public UsingDirectiveSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax node, SyntaxNode? parent)
    {
        Name = node.Name is null ? null : NameSyntax.From(node.Name, this);
        GlobalKeyword = node.GlobalKeyword;
        UsingKeyword = node.UsingKeyword;
        StaticKeyword = node.StaticKeyword;
        UnsafeKeyword = node.UnsafeKeyword;
        Alias = node.Alias is null ? null : new NameEqualsSyntax(node.Alias, this);
        NamespaceOrType = TypeSyntax.From(node.NamespaceOrType, this);
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public NameSyntax? Name { get; }
    public SyntaxToken GlobalKeyword { get; }
    public SyntaxToken UsingKeyword { get; }
    public SyntaxToken StaticKeyword { get; }
    public SyntaxToken UnsafeKeyword { get; }
    public NameEqualsSyntax? Alias { get; }
    public TypeSyntax NamespaceOrType { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
