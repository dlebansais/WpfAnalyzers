namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class QualifiedNameSyntax : NameSyntax
{
    public QualifiedNameSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax node, SyntaxNode? parent)
    {
        Left = NameSyntax.From(node.Left, this);
        DotToken = node.DotToken;
        Right = SimpleNameSyntax.From(node.Right, this);
        Parent = parent;
    }

    public NameSyntax Left { get; }
    public SyntaxToken DotToken { get; }
    public SimpleNameSyntax Right { get; }
    public SyntaxNode? Parent { get; }

}
