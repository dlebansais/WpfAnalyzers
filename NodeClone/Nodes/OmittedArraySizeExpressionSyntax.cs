namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class OmittedArraySizeExpressionSyntax : ExpressionSyntax
{
    public OmittedArraySizeExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.OmittedArraySizeExpressionSyntax node, SyntaxNode? parent)
    {
        OmittedArraySizeExpressionToken = node.OmittedArraySizeExpressionToken;
        Parent = parent;
    }

    public SyntaxToken OmittedArraySizeExpressionToken { get; }
    public SyntaxNode? Parent { get; }

}
