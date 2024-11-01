namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FixedStatementSyntax : StatementSyntax
{
    public FixedStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FixedStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        FixedKeyword = node.FixedKeyword;
        OpenParenToken = node.OpenParenToken;
        Declaration = new VariableDeclarationSyntax(node.Declaration, this);
        CloseParenToken = node.CloseParenToken;
        Statement = StatementSyntax.From(node.Statement, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken FixedKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public VariableDeclarationSyntax Declaration { get; }
    public SyntaxToken CloseParenToken { get; }
    public StatementSyntax Statement { get; }
    public SyntaxNode? Parent { get; }

}
