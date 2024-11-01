namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PropertyPatternClauseSyntax : SyntaxNode
{
    public PropertyPatternClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PropertyPatternClauseSyntax node, SyntaxNode? parent)
    {
        OpenBraceToken = node.OpenBraceToken;
        Subpatterns = Cloner.SeparatedListFrom<SubpatternSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax>(node.Subpatterns, parent);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public SyntaxToken OpenBraceToken { get; }
    public SeparatedSyntaxList<SubpatternSyntax> Subpatterns { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
