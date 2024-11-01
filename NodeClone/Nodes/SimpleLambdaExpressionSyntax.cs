namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SimpleLambdaExpressionSyntax : LambdaExpressionSyntax
{
    public SimpleLambdaExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SimpleLambdaExpressionSyntax node, SyntaxNode? parent)
    {
        AsyncKeyword = node.AsyncKeyword;
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Parameter = new ParameterSyntax(node.Parameter, this);
        ArrowToken = node.ArrowToken;
        Block = node.Block is null ? null : new BlockSyntax(node.Block, this);
        ExpressionBody = node.ExpressionBody is null ? null : ExpressionSyntax.From(node.ExpressionBody, this);
        Parent = parent;
    }

    public SyntaxToken AsyncKeyword { get; }
    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public ParameterSyntax Parameter { get; }
    public SyntaxToken ArrowToken { get; }
    public BlockSyntax? Block { get; }
    public ExpressionSyntax? ExpressionBody { get; }
    public SyntaxNode? Parent { get; }

}
