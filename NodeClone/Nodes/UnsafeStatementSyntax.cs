namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class UnsafeStatementSyntax : StatementSyntax
{
    public UnsafeStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.UnsafeStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        UnsafeKeyword = node.UnsafeKeyword;
        Block = new BlockSyntax(node.Block, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken UnsafeKeyword { get; }
    public BlockSyntax Block { get; }
    public SyntaxNode? Parent { get; }

}
