namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class GlobalStatementSyntax : MemberDeclarationSyntax
{
    public GlobalStatementSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.GlobalStatementSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Statement = StatementSyntax.From(node.Statement, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public StatementSyntax Statement { get; }
    public SyntaxNode? Parent { get; }

}
