namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SubpatternSyntax : SyntaxNode
{
    public SubpatternSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SubpatternSyntax node, SyntaxNode? parent)
    {
        NameColon = node.NameColon is null ? null : new NameColonSyntax(node.NameColon, this);
        ExpressionColon = node.ExpressionColon is null ? null : BaseExpressionColonSyntax.From(node.ExpressionColon, this);
        Pattern = PatternSyntax.From(node.Pattern, this);
        Parent = parent;
    }

    public NameColonSyntax? NameColon { get; }
    public BaseExpressionColonSyntax? ExpressionColon { get; }
    public PatternSyntax Pattern { get; }
    public SyntaxNode? Parent { get; }

}
