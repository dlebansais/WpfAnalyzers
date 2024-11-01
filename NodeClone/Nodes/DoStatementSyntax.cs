namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DoStatementSyntax : StatementSyntax
{
    public DoStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DoStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        DoKeyword = node.DoKeyword;
        Statement = StatementSyntax.From(node.Statement, this);
        WhileKeyword = node.WhileKeyword;
        OpenParenToken = node.OpenParenToken;
        Condition = ExpressionSyntax.From(node.Condition, this);
        CloseParenToken = node.CloseParenToken;
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken DoKeyword { get; }
    public StatementSyntax Statement { get; }
    public SyntaxToken WhileKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
