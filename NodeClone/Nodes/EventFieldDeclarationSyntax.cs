namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class EventFieldDeclarationSyntax : BaseFieldDeclarationSyntax
{
    public EventFieldDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        EventKeyword = node.EventKeyword;
        Declaration = new VariableDeclarationSyntax(node.Declaration, this);
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken EventKeyword { get; }
    public VariableDeclarationSyntax Declaration { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
