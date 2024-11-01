namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CatchClauseSyntax : SyntaxNode
{
    public CatchClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CatchClauseSyntax node, SyntaxNode? parent)
    {
        CatchKeyword = node.CatchKeyword;
        Declaration = node.Declaration is null ? null : new CatchDeclarationSyntax(node.Declaration, this);
        Filter = node.Filter is null ? null : new CatchFilterClauseSyntax(node.Filter, this);
        Block = new BlockSyntax(node.Block, this);
        Parent = parent;
    }

    public SyntaxToken CatchKeyword { get; }
    public CatchDeclarationSyntax? Declaration { get; }
    public CatchFilterClauseSyntax? Filter { get; }
    public BlockSyntax Block { get; }
    public SyntaxNode? Parent { get; }

}
