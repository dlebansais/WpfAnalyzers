namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class LabeledStatementSyntax : StatementSyntax
{
    public LabeledStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.LabeledStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Identifier = node.Identifier;
        ColonToken = node.ColonToken;
        Statement = StatementSyntax.From(node.Statement, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken Identifier { get; }
    public SyntaxToken ColonToken { get; }
    public StatementSyntax Statement { get; }
    public SyntaxNode? Parent { get; }

}
