namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParameterSyntax : BaseParameterSyntax
{
    public ParameterSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Type = node.Type is null ? null : TypeSyntax.From(node.Type, this);
        Identifier = node.Identifier;
        Default = node.Default is null ? null : new EqualsValueClauseSyntax(node.Default, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public TypeSyntax? Type { get; }
    public SyntaxToken Identifier { get; }
    public EqualsValueClauseSyntax? Default { get; }
    public SyntaxNode? Parent { get; }

}
