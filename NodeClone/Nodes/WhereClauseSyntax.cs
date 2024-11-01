namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class WhereClauseSyntax : QueryClauseSyntax
{
    public WhereClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.WhereClauseSyntax node, SyntaxNode? parent)
    {
        WhereKeyword = node.WhereKeyword;
        Condition = ExpressionSyntax.From(node.Condition, this);
        Parent = parent;
    }

    public SyntaxToken WhereKeyword { get; }
    public ExpressionSyntax Condition { get; }
    public SyntaxNode? Parent { get; }

}
