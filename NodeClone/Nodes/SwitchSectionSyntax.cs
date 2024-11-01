namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class SwitchSectionSyntax : SyntaxNode
{
    public SwitchSectionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchSectionSyntax node, SyntaxNode? parent)
    {
        Labels = Cloner.ListFrom<SwitchLabelSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax>(node.Labels, parent);
        Statements = Cloner.ListFrom<StatementSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax>(node.Statements, parent);
        Parent = parent;
    }

    public SyntaxList<SwitchLabelSyntax> Labels { get; }
    public SyntaxList<StatementSyntax> Statements { get; }
    public SyntaxNode? Parent { get; }

}
