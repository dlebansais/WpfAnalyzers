namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AttributeListSyntax : SyntaxNode
{
    public AttributeListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax node, SyntaxNode? parent)
    {
        OpenBracketToken = node.OpenBracketToken;
        Target = node.Target is null ? null : new AttributeTargetSpecifierSyntax(node.Target, this);
        Attributes = Cloner.SeparatedListFrom<AttributeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax>(node.Attributes, parent);
        CloseBracketToken = node.CloseBracketToken;
        Parent = parent;
    }

    public SyntaxToken OpenBracketToken { get; }
    public AttributeTargetSpecifierSyntax? Target { get; }
    public SeparatedSyntaxList<AttributeSyntax> Attributes { get; }
    public SyntaxToken CloseBracketToken { get; }
    public SyntaxNode? Parent { get; }

}
