namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CheckedStatementSyntax : StatementSyntax
{
    public CheckedStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CheckedStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Keyword = node.Keyword;
        Block = new BlockSyntax(node.Block, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken Keyword { get; }
    public BlockSyntax Block { get; }
    public SyntaxNode? Parent { get; }

}
