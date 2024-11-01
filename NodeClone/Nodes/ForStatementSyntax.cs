namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ForStatementSyntax : StatementSyntax
{
    public ForStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        ForKeyword = node.ForKeyword;
        OpenParenToken = node.OpenParenToken;
        Declaration = node.Declaration is null ? null : new VariableDeclarationSyntax(node.Declaration, this);
        Initializers = Cloner.SeparatedListFrom<ExpressionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax>(node.Initializers, parent);
        FirstSemicolonToken = node.FirstSemicolonToken;
        Condition = node.Condition is null ? null : ExpressionSyntax.From(node.Condition, this);
        SecondSemicolonToken = node.SecondSemicolonToken;
        Incrementors = Cloner.SeparatedListFrom<ExpressionSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionSyntax>(node.Incrementors, parent);
        CloseParenToken = node.CloseParenToken;
        Statement = StatementSyntax.From(node.Statement, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken ForKeyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public VariableDeclarationSyntax? Declaration { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Initializers { get; }
    public SyntaxToken FirstSemicolonToken { get; }
    public ExpressionSyntax? Condition { get; }
    public SyntaxToken SecondSemicolonToken { get; }
    public SeparatedSyntaxList<ExpressionSyntax> Incrementors { get; }
    public SyntaxToken CloseParenToken { get; }
    public StatementSyntax Statement { get; }
    public SyntaxNode? Parent { get; }

}
