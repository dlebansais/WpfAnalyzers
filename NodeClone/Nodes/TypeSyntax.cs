namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class TypeSyntax : ExpressionSyntax
{
    public static TypeSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.TypeSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.AliasQualifiedNameSyntax AsAliasQualifiedNameSyntax => new AliasQualifiedNameSyntax(AsAliasQualifiedNameSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.GenericNameSyntax AsGenericNameSyntax => new GenericNameSyntax(AsGenericNameSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.IdentifierNameSyntax AsIdentifierNameSyntax => new IdentifierNameSyntax(AsIdentifierNameSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.QualifiedNameSyntax AsQualifiedNameSyntax => new QualifiedNameSyntax(AsQualifiedNameSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.RefTypeSyntax AsRefTypeSyntax => new RefTypeSyntax(AsRefTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.PredefinedTypeSyntax AsPredefinedTypeSyntax => new PredefinedTypeSyntax(AsPredefinedTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ArrayTypeSyntax AsArrayTypeSyntax => new ArrayTypeSyntax(AsArrayTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.PointerTypeSyntax AsPointerTypeSyntax => new PointerTypeSyntax(AsPointerTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.FunctionPointerTypeSyntax AsFunctionPointerTypeSyntax => new FunctionPointerTypeSyntax(AsFunctionPointerTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.NullableTypeSyntax AsNullableTypeSyntax => new NullableTypeSyntax(AsNullableTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.TupleTypeSyntax AsTupleTypeSyntax => new TupleTypeSyntax(AsTupleTypeSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.OmittedTypeArgumentSyntax AsOmittedTypeArgumentSyntax => new OmittedTypeArgumentSyntax(AsOmittedTypeArgumentSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ScopedTypeSyntax AsScopedTypeSyntax => new ScopedTypeSyntax(AsScopedTypeSyntax, parent),
            _ => null!,
        };
    }
}
