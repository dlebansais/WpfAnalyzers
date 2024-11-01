namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RefStructConstraintSyntax : AllowsConstraintSyntax
{
    public RefStructConstraintSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RefStructConstraintSyntax node, SyntaxNode? parent)
    {
        RefKeyword = node.RefKeyword;
        StructKeyword = node.StructKeyword;
        Parent = parent;
    }

    public SyntaxToken RefKeyword { get; }
    public SyntaxToken StructKeyword { get; }
    public SyntaxNode? Parent { get; }

}
