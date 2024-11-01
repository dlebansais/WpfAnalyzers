namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SwitchStatementSyntax : StatementSyntax
{
    public SwitchStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        SwitchKeyword = node.SwitchKeyword;
        OpenParenToken = node.OpenParenToken;
        Expression = ExpressionSyntax.From(node.Expression, this);
        CloseParenToken = node.CloseParenToken;
        OpenBraceToken = node.OpenBraceToken;
        Sections = Cloner.ListFrom<SwitchSectionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax>(node.Sections, parent);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken SwitchKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public ExpressionSyntax Expression { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxToken OpenBraceToken { get; }
    public SyntaxList<SwitchSectionSyntax> Sections { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
