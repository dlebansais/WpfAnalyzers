namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TryStatementSyntax : StatementSyntax
{
    public TryStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TryStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        TryKeyword = node.TryKeyword;
        Block = new BlockSyntax(node.Block, this);
        Catches = Cloner.ListFrom<CatchClauseSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax>(node.Catches, parent);
        Finally = node.Finally is null ? null : new FinallyClauseSyntax(node.Finally, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken TryKeyword { get; }
    public BlockSyntax Block { get; }
    public SyntaxList<CatchClauseSyntax> Catches { get; }
    public FinallyClauseSyntax? Finally { get; }
    public SyntaxNode? Parent { get; }

}
