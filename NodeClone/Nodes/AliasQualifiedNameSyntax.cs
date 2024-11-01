namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AliasQualifiedNameSyntax : NameSyntax
{
    public AliasQualifiedNameSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax node, SyntaxNode? parent)
    {
        Alias = new IdentifierNameSyntax(node.Alias, this);
        ColonColonToken = node.ColonColonToken;
        Name = SimpleNameSyntax.From(node.Name, this);
        Parent = parent;
    }

    public IdentifierNameSyntax Alias { get; }
    public SyntaxToken ColonColonToken { get; }
    public SimpleNameSyntax Name { get; }
    public SyntaxNode? Parent { get; }

}
