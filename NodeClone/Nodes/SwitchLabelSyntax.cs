namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class SwitchLabelSyntax : SyntaxNode
{
    public static SwitchLabelSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.SwitchLabelSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.CasePatternSwitchLabelSyntax AsCasePatternSwitchLabelSyntax => new CasePatternSwitchLabelSyntax(AsCasePatternSwitchLabelSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.CaseSwitchLabelSyntax AsCaseSwitchLabelSyntax => new CaseSwitchLabelSyntax(AsCaseSwitchLabelSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.DefaultSwitchLabelSyntax AsDefaultSwitchLabelSyntax => new DefaultSwitchLabelSyntax(AsDefaultSwitchLabelSyntax, parent),
            _ => null!,
        };
    }
}
