namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DiscardDesignationSyntax : VariableDesignationSyntax
{
    public DiscardDesignationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DiscardDesignationSyntax node, SyntaxNode? parent)
    {
        UnderscoreToken = node.UnderscoreToken;
        Parent = parent;
    }

    public SyntaxToken UnderscoreToken { get; }
    public SyntaxNode? Parent { get; }

}
