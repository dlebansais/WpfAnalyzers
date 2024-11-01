namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class CommonForEachStatementSyntax : StatementSyntax
{
    public static CommonForEachStatementSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.CommonForEachStatementSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax AsForEachStatementSyntax => new ForEachStatementSyntax(AsForEachStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ForEachVariableStatementSyntax AsForEachVariableStatementSyntax => new ForEachVariableStatementSyntax(AsForEachVariableStatementSyntax, parent),
            _ => null!,
        };
    }
}
