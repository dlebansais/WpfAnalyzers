namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParenthesizedLambdaExpressionSyntax : LambdaExpressionSyntax
{
    public ParenthesizedLambdaExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedLambdaExpressionSyntax node, SyntaxNode? parent)
    {
        AsyncKeyword = node.AsyncKeyword;
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        ReturnType = node.ReturnType is null ? null : TypeSyntax.From(node.ReturnType, this);
        ParameterList = new ParameterListSyntax(node.ParameterList, this);
        ArrowToken = node.ArrowToken;
        Block = node.Block is null ? null : new BlockSyntax(node.Block, this);
        ExpressionBody = node.ExpressionBody is null ? null : ExpressionSyntax.From(node.ExpressionBody, this);
        Parent = parent;
    }

    public SyntaxToken AsyncKeyword { get; }
    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public TypeSyntax? ReturnType { get; }
    public ParameterListSyntax ParameterList { get; }
    public SyntaxToken ArrowToken { get; }
    public BlockSyntax? Block { get; }
    public ExpressionSyntax? ExpressionBody { get; }
    public SyntaxNode? Parent { get; }

}
