namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class IfStatementSyntax : StatementSyntax
{
    public IfStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.IfStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        IfKeyword = node.IfKeyword;
        OpenParenToken = node.OpenParenToken;
        Condition = ExpressionSyntax.From(node.Condition, this);
        CloseParenToken = node.CloseParenToken;
        Statement = StatementSyntax.From(node.Statement, this);
        Else = node.Else is null ? null : new ElseClauseSyntax(node.Else, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken IfKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxToken CloseParenToken { get; }
    public StatementSyntax Statement { get; }
    public ElseClauseSyntax? Else { get; }
    public SyntaxNode? Parent { get; }

}
