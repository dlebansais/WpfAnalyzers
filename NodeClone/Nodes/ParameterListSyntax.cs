namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ParameterListSyntax : BaseParameterListSyntax
{
    public ParameterListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ParameterListSyntax node, SyntaxNode? parent)
    {
        OpenParenToken = node.OpenParenToken;
        Parameters = Cloner.SeparatedListFrom<ParameterSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ParameterSyntax>(node.Parameters, parent);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken OpenParenToken { get; }
    public SeparatedSyntaxList<ParameterSyntax> Parameters { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
