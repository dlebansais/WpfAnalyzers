namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class BracketedArgumentListSyntax : BaseArgumentListSyntax
{
    public BracketedArgumentListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.BracketedArgumentListSyntax node, SyntaxNode? parent)
    {
        OpenBracketToken = node.OpenBracketToken;
        Arguments = Cloner.SeparatedListFrom<ArgumentSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ArgumentSyntax>(node.Arguments, parent);
        CloseBracketToken = node.CloseBracketToken;
        Parent = parent;
    }

    public SyntaxToken OpenBracketToken { get; }
    public SeparatedSyntaxList<ArgumentSyntax> Arguments { get; }
    public SyntaxToken CloseBracketToken { get; }
    public SyntaxNode? Parent { get; }

}
