namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class BaseListSyntax : SyntaxNode
{
    public BaseListSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.BaseListSyntax node, SyntaxNode? parent)
    {
        ColonToken = node.ColonToken;
        Types = Cloner.SeparatedListFrom<BaseTypeSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.BaseTypeSyntax>(node.Types, parent);
        Parent = parent;
    }

    public SyntaxToken ColonToken { get; }
    public SeparatedSyntaxList<BaseTypeSyntax> Types { get; }
    public SyntaxNode? Parent { get; }

}
