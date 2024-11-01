namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class BaseFieldDeclarationSyntax : MemberDeclarationSyntax
{
    public static BaseFieldDeclarationSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.BaseFieldDeclarationSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax AsFieldDeclarationSyntax => new FieldDeclarationSyntax(AsFieldDeclarationSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.EventFieldDeclarationSyntax AsEventFieldDeclarationSyntax => new EventFieldDeclarationSyntax(AsEventFieldDeclarationSyntax, parent),
            _ => null!,
        };
    }
}
