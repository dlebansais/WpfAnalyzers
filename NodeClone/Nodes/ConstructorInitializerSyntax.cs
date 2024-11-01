namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConstructorInitializerSyntax : SyntaxNode
{
    public ConstructorInitializerSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConstructorInitializerSyntax node, SyntaxNode? parent)
    {
        ColonToken = node.ColonToken;
        ThisOrBaseKeyword = node.ThisOrBaseKeyword;
        ArgumentList = new ArgumentListSyntax(node.ArgumentList, this);
        Parent = parent;
    }

    public SyntaxToken ColonToken { get; }
    public SyntaxToken ThisOrBaseKeyword { get; }
    public ArgumentListSyntax ArgumentList { get; }
    public SyntaxNode? Parent { get; }

}
