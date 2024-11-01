﻿namespace NodeClones;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

public class DefaultExpressionSyntax : ExpressionSyntax
{
    public DefaultExpressionSyntax(Microsoft.CodeAnalysis.CSharp.Syntax.DefaultExpressionSyntax node, SyntaxNode? parent)
    {
        Keyword = node.Keyword;
        OpenParenToken = node.OpenParenToken;
        Type = TypeSyntax.From(node.Type, this);
        CloseParenToken = node.CloseParenToken;
        Parent = parent;
    }

    public SyntaxToken Keyword { get; }
    public SyntaxToken OpenParenToken { get; }
    public TypeSyntax Type { get; }
    public SyntaxToken CloseParenToken { get; }
    public SyntaxNode? Parent { get; }

}
