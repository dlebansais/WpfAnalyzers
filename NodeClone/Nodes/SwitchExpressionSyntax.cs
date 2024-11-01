namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SwitchExpressionSyntax : ExpressionSyntax
{
    public SwitchExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionSyntax node, SyntaxNode? parent)
    {
        GoverningExpression = ExpressionSyntax.From(node.GoverningExpression, this);
        SwitchKeyword = node.SwitchKeyword;
        OpenBraceToken = node.OpenBraceToken;
        Arms = Cloner.SeparatedListFrom<SwitchExpressionArmSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.SwitchExpressionArmSyntax>(node.Arms, parent);
        CloseBraceToken = node.CloseBraceToken;
        Parent = parent;
    }

    public ExpressionSyntax GoverningExpression { get; }
    public SyntaxToken SwitchKeyword { get; }
    public SyntaxToken OpenBraceToken { get; }
    public SeparatedSyntaxList<SwitchExpressionArmSyntax> Arms { get; }
    public SyntaxToken CloseBraceToken { get; }
    public SyntaxNode? Parent { get; }

}
