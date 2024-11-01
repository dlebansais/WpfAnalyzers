namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class FunctionPointerParameterSyntax : BaseParameterSyntax
{
    public FunctionPointerParameterSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerParameterSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Type = TypeSyntax.From(node.Type, this);
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public TypeSyntax Type { get; }
    public SyntaxNode? Parent { get; }

}
