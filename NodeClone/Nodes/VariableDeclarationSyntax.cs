namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class VariableDeclarationSyntax : SyntaxNode
{
    public VariableDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclarationSyntax node, SyntaxNode? parent)
    {
        Type = TypeSyntax.From(node.Type, this);
        Variables = Cloner.SeparatedListFrom<VariableDeclaratorSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax>(node.Variables, parent);
        Parent = parent;
    }

    public TypeSyntax Type { get; }
    public SeparatedSyntaxList<VariableDeclaratorSyntax> Variables { get; }
    public SyntaxNode? Parent { get; }

}
