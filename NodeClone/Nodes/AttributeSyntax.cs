namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AttributeSyntax : SyntaxNode
{
    public AttributeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AttributeSyntax node, SyntaxNode? parent)
    {
        Name = NameSyntax.From(node.Name, this);
        ArgumentList = node.ArgumentList is null ? null : new AttributeArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public NameSyntax Name { get; }
    public AttributeArgumentListSyntax? ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
