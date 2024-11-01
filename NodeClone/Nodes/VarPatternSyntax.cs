namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class VarPatternSyntax : PatternSyntax
{
    public VarPatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.VarPatternSyntax node, SyntaxNode? parent)
    {
        VarKeyword = node.VarKeyword;
        Designation = VariableDesignationSyntax.From(node.Designation, this);
        Parent = parent;
    }

    public SyntaxToken VarKeyword { get; }
    public VariableDesignationSyntax Designation { get; }
    public SyntaxNode? Parent { get; }

}
