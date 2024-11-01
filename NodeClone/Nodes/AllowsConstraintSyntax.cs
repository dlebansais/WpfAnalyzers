namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class AllowsConstraintSyntax : SyntaxNode
{
    public static AllowsConstraintSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.AllowsConstraintSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.RefStructConstraintSyntax AsRefStructConstraintSyntax => new RefStructConstraintSyntax(AsRefStructConstraintSyntax, parent),
            _ => null!,
        };
    }
}
