namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class CompilationUnitSyntax : SyntaxNode
{
    public CompilationUnitSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.CompilationUnitSyntax node, SyntaxNode? parent)
    {
        Externs = Cloner.ListFrom<ExternAliasDirectiveSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.ExternAliasDirectiveSyntax>(node.Externs, parent);
        Usings = Cloner.ListFrom<UsingDirectiveSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.UsingDirectiveSyntax>(node.Usings, parent);
        AttributeLists = Cloner.ListFrom<AttributeListSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.AttributeListSyntax>(node.AttributeLists, parent);
        Members = Cloner.ListFrom<MemberDeclarationSyntax, Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax>(node.Members, parent);
        EndOfFileToken = node.EndOfFileToken;
        Parent = parent;
    }

    public SyntaxList<ExternAliasDirectiveSyntax> Externs { get; }
    public SyntaxList<UsingDirectiveSyntax> Usings { get; }
    public SyntaxList<AttributeListSyntax> AttributeLists { get; }
    public SyntaxList<MemberDeclarationSyntax> Members { get; }
    public SyntaxToken EndOfFileToken { get; }
    public SyntaxNode? Parent { get; }

}
