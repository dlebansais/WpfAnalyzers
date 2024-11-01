namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class PatternSyntax : ExpressionOrPatternSyntax
{
    public static PatternSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.PatternSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.DiscardPatternSyntax AsDiscardPatternSyntax => new DiscardPatternSyntax(AsDiscardPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.DeclarationPatternSyntax AsDeclarationPatternSyntax => new DeclarationPatternSyntax(AsDeclarationPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.VarPatternSyntax AsVarPatternSyntax => new VarPatternSyntax(AsVarPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.RecursivePatternSyntax AsRecursivePatternSyntax => new RecursivePatternSyntax(AsRecursivePatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ConstantPatternSyntax AsConstantPatternSyntax => new ConstantPatternSyntax(AsConstantPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ParenthesizedPatternSyntax AsParenthesizedPatternSyntax => new ParenthesizedPatternSyntax(AsParenthesizedPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.RelationalPatternSyntax AsRelationalPatternSyntax => new RelationalPatternSyntax(AsRelationalPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.TypePatternSyntax AsTypePatternSyntax => new TypePatternSyntax(AsTypePatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.BinaryPatternSyntax AsBinaryPatternSyntax => new BinaryPatternSyntax(AsBinaryPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.UnaryPatternSyntax AsUnaryPatternSyntax => new UnaryPatternSyntax(AsUnaryPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ListPatternSyntax AsListPatternSyntax => new ListPatternSyntax(AsListPatternSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.SlicePatternSyntax AsSlicePatternSyntax => new SlicePatternSyntax(AsSlicePatternSyntax, parent),
            _ => null!,
        };
    }
}
