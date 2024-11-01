namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class ConversionOperatorDeclarationSyntax : BaseMethodDeclarationSyntax
{
    public ConversionOperatorDeclarationSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.ConversionOperatorDeclarationSyntax node, SyntaxNode? parent)
    {
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        ImplicitOrExplicitKeyword = node.ImplicitOrExplicitKeyword;
        ExplicitInterfaceSpecifier = node.ExplicitInterfaceSpecifier is null ? null : new ExplicitInterfaceSpecifierSyntax(node.ExplicitInterfaceSpecifier, this);
        OperatorKeyword = node.OperatorKeyword;
        CheckedKeyword = node.CheckedKeyword;
        Type = TypeSyntax.From(node.Type, this);
        ParameterList = new ParameterListSyntax(node.ParameterList, this);
        Body = node.Body is null ? null : new BlockSyntax(node.Body, this);
        ExpressionBody = node.ExpressionBody is null ? null : new ArrowExpressionClauseSyntax(node.ExpressionBody, this);
        SemicolonToken = node.SemicolonToken;
        Parent = parent;
    }

    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxToken ImplicitOrExplicitKeyword { get; }
    public ExplicitInterfaceSpecifierSyntax? ExplicitInterfaceSpecifier { get; }
    public SyntaxToken OperatorKeyword { get; }
    public SyntaxToken CheckedKeyword { get; }
    public TypeSyntax Type { get; }
    public ParameterListSyntax ParameterList { get; }
    public BlockSyntax? Body { get; }
    public ArrowExpressionClauseSyntax? ExpressionBody { get; }
    public SyntaxToken SemicolonToken { get; }
    public SyntaxNode? Parent { get; }

}
