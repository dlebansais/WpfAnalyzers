namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class VariableDeclaratorSyntax : SyntaxNode
{
    public VariableDeclaratorSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.VariableDeclaratorSyntax node, SyntaxNode? parent)
    {
        Identifier = node.Identifier;
        ArgumentList = node.ArgumentList is null ? null : new BracketedArgumentListSyntax(node.ArgumentList, this);
        Initializer = node.Initializer is null ? null : new EqualsValueClauseSyntax(node.Initializer, this);
        Parent = parent;
    }

    public SyntaxToken Identifier { get; }
    public BracketedArgumentListSyntax? ArgumentList { get; }
    public EqualsValueClauseSyntax? Initializer { get; }
    public SyntaxNode? Parent { get; }

}
