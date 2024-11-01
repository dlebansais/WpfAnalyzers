namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TypeConstraintSyntax : TypeParameterConstraintSyntax
{
    public TypeConstraintSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TypeConstraintSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
