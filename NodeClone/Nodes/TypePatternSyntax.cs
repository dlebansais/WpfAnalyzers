namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class TypePatternSyntax : PatternSyntax
{
    public TypePatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.TypePatternSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
