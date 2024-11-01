namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DeclarationExpressionSyntax : ExpressionSyntax
{
    public DeclarationExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationExpressionSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Designation = VariableDesignationSyntax.From(node.Designation, this);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public VariableDesignationSyntax Designation { get; }
    public SyntaxNode? Parent { get; }

}
