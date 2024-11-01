namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class RecursivePatternSyntax : PatternSyntax
{
    public RecursivePatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.RecursivePatternSyntax node, SyntaxNode? parent)
    {
        Type = node.Type is null ? null : TypeSyntax.From(node.Type, this);
        PositionalPatternClause = node.PositionalPatternClause is null ? null : new PositionalPatternClauseSyntax(node.PositionalPatternClause, this);
        PropertyPatternClause = node.PropertyPatternClause is null ? null : new PropertyPatternClauseSyntax(node.PropertyPatternClause, this);
        Designation = node.Designation is null ? null : VariableDesignationSyntax.From(node.Designation, this);
        Parent = parent;
    }

    public TypeSyntax? Type { get; }
    public PositionalPatternClauseSyntax? PositionalPatternClause { get; }
    public PropertyPatternClauseSyntax? PropertyPatternClause { get; }
    public VariableDesignationSyntax? Designation { get; }
    public SyntaxNode? Parent { get; }

}
