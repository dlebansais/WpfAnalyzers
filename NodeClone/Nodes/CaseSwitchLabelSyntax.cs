namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CaseSwitchLabelSyntax : SwitchLabelSyntax
{
    public CaseSwitchLabelSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CaseSwitchLabelSyntax node, SyntaxNode? parent)
    {
        Keyword = node.Keyword;
        Value = ExpressionSyntax.From(node.Value, this);
        ColonToken = node.ColonToken;
        Parent = parent;
    }

    public SyntaxToken Keyword { get; }
    public ExpressionSyntax Value { get; }
    public SyntaxToken ColonToken { get; }
    public SyntaxNode? Parent { get; }

}
