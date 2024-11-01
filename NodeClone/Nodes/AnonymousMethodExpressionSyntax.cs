namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AnonymousMethodExpressionSyntax : AnonymousFunctionExpressionSyntax
{
    public AnonymousMethodExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AnonymousMethodExpressionSyntax node, SyntaxNode? parent)
    {
        AsyncKeyword = node.AsyncKeyword;
        DelegateKeyword = node.DelegateKeyword;
        ParameterList = node.ParameterList is null ? null : new ParameterListSyntax(node.ParameterList, this);
        Block = new BlockSyntax(node.Block, this);
        ExpressionBody = node.ExpressionBody is null ? null : ExpressionSyntax.From(node.ExpressionBody, this);
        Parent = parent;
    }

    public SyntaxToken AsyncKeyword { get; }
    public SyntaxToken DelegateKeyword { get; }
    public ParameterListSyntax? ParameterList { get; }
    public BlockSyntax Block { get; }
    public ExpressionSyntax? ExpressionBody { get; }
    public SyntaxNode? Parent { get; }

}
