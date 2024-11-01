namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class AllowsConstraintClauseSyntax : TypeParameterConstraintSyntax
{
    public AllowsConstraintClauseSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.AllowsConstraintClauseSyntax node, SyntaxNode? parent)
    {
        AllowsKeyword = node.AllowsKeyword;
        Constraints = Cloner.SeparatedListFrom<AllowsConstraintSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AllowsConstraintSyntax>(node.Constraints, parent);
        Parent = parent;
    }

    public SyntaxToken AllowsKeyword { get; }
    public SeparatedSyntaxList<AllowsConstraintSyntax> Constraints { get; }
    public SyntaxNode? Parent { get; }

}
