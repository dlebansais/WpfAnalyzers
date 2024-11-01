namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PositionalPatternClauseSyntax : SyntaxNode
{
    public PositionalPatternClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PositionalPatternClauseSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Subpatterns = Cloner.SeparatedListFrom<SubpatternSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax>(node.Subpatterns, parent);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public SeparatedSyntaxList<SubpatternSyntax> Subpatterns { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
