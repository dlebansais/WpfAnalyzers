namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public abstract class StatementSyntax : SyntaxNode
{
    public static StatementSyntax From(Microsoft.CodeAnalysis.CSharp.Syntax.StatementSyntax node, SyntaxNode? parent)
    {
        return node switch
        {
            Microsoft.CodeAnalysis.CSharp.Syntax.BlockSyntax AsBlockSyntax => new BlockSyntax(AsBlockSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.BreakStatementSyntax AsBreakStatementSyntax => new BreakStatementSyntax(AsBreakStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.CheckedStatementSyntax AsCheckedStatementSyntax => new CheckedStatementSyntax(AsCheckedStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ContinueStatementSyntax AsContinueStatementSyntax => new ContinueStatementSyntax(AsContinueStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.LocalDeclarationStatementSyntax AsLocalDeclarationStatementSyntax => new LocalDeclarationStatementSyntax(AsLocalDeclarationStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.DoStatementSyntax AsDoStatementSyntax => new DoStatementSyntax(AsDoStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.EmptyStatementSyntax AsEmptyStatementSyntax => new EmptyStatementSyntax(AsEmptyStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ExpressionStatementSyntax AsExpressionStatementSyntax => new ExpressionStatementSyntax(AsExpressionStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.FixedStatementSyntax AsFixedStatementSyntax => new FixedStatementSyntax(AsFixedStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ForEachStatementSyntax AsForEachStatementSyntax => new ForEachStatementSyntax(AsForEachStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ForEachVariableStatementSyntax AsForEachVariableStatementSyntax => new ForEachVariableStatementSyntax(AsForEachVariableStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ForStatementSyntax AsForStatementSyntax => new ForStatementSyntax(AsForStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.GotoStatementSyntax AsGotoStatementSyntax => new GotoStatementSyntax(AsGotoStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.IfStatementSyntax AsIfStatementSyntax => new IfStatementSyntax(AsIfStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.LabeledStatementSyntax AsLabeledStatementSyntax => new LabeledStatementSyntax(AsLabeledStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.LocalFunctionStatementSyntax AsLocalFunctionStatementSyntax => new LocalFunctionStatementSyntax(AsLocalFunctionStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.LockStatementSyntax AsLockStatementSyntax => new LockStatementSyntax(AsLockStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ReturnStatementSyntax AsReturnStatementSyntax => new ReturnStatementSyntax(AsReturnStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.SwitchStatementSyntax AsSwitchStatementSyntax => new SwitchStatementSyntax(AsSwitchStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.ThrowStatementSyntax AsThrowStatementSyntax => new ThrowStatementSyntax(AsThrowStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.TryStatementSyntax AsTryStatementSyntax => new TryStatementSyntax(AsTryStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.UnsafeStatementSyntax AsUnsafeStatementSyntax => new UnsafeStatementSyntax(AsUnsafeStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.UsingStatementSyntax AsUsingStatementSyntax => new UsingStatementSyntax(AsUsingStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.WhileStatementSyntax AsWhileStatementSyntax => new WhileStatementSyntax(AsWhileStatementSyntax, parent),
            Microsoft.CodeAnalysis.CSharp.Syntax.YieldStatementSyntax AsYieldStatementSyntax => new YieldStatementSyntax(AsYieldStatementSyntax, parent),
            _ => null!,
        };
    }
}
