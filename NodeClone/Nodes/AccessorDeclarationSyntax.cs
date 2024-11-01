namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AccessorDeclarationSyntax : SyntaxNode
{
    public AccessorDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AccessorDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Keyword = node.Keyword;
        Body = node.Body is null ? null : new BlockSyntax(node.Body, this);
        ExpressionBody = node.ExpressionBody is null ? null : new ArrowExpressionClauseSyntax(node.ExpressionBody, this);
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken Keyword { get; }
    public BlockSyntax? Body { get; }
    public ArrowExpressionClauseSyntax? ExpressionBody { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
