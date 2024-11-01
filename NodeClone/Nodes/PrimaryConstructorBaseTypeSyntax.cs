namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class PrimaryConstructorBaseTypeSyntax : BaseTypeSyntax
{
    public PrimaryConstructorBaseTypeSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.PrimaryConstructorBaseTypeSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        ArgumentList = new ArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public ArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
