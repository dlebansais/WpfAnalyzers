namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FieldDeclarationSyntax : BaseFieldDeclarationSyntax
{
    public FieldDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Declaration = new VariableDeclarationSyntax(node.Declaration, this);
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public VariableDeclarationSyntax Declaration { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
