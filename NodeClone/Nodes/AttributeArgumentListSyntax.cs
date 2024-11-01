namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AttributeArgumentListSyntax : SyntaxNode
{
    public AttributeArgumentListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentListSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Arguments = Cloner.SeparatedListFrom<AttributeArgumentSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeArgumentSyntax>(node.Arguments, parent);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public SeparatedSyntaxList<AttributeArgumentSyntax> Arguments { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
