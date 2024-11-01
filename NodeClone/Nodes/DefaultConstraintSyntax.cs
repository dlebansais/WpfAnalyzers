namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DefaultConstraintSyntax : TypeParameterConstraintSyntax
{
    public DefaultConstraintSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DefaultConstraintSyntax node, SyntaxNode? parent)
    {
        DefaultKeyword = node.DefaultKeyword;
        Parent = parent;
    }

    public SyntaxToken DefaultKeyword { get; }
    public SyntaxNode? Parent { get; }

}
