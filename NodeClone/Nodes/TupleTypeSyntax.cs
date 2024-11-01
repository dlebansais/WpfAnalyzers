namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TupleTypeSyntax : TypeSyntax
{
    public TupleTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Elements = Cloner.SeparatedListFrom<TupleElementSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.TupleElementSyntax>(node.Elements, parent);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public SeparatedSyntaxList<TupleElementSyntax> Elements { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
