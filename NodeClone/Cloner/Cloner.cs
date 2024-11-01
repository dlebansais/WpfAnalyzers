namespace NodeClones;

using System.Reflection;
using Microsoft.CodeAnalysis;

public static class Cloner
{
    public static SyntaxList<TClone> ListFrom<TClone, TNode>(Microsoft.CodeAnalysis.SyntaxList<TNode> items, SyntaxNode? parent)
        where TClone : SyntaxNode
        where TNode : Microsoft.CodeAnalysis.SyntaxNode
    {
        SyntaxList<TClone> Result = new();

        foreach (var Item in items)
            Result.Add(Clone<TClone, TNode>(Item, parent));

        return Result;
    }

    public static SeparatedSyntaxList<TClone> SeparatedListFrom<TClone, TNode>(Microsoft.CodeAnalysis.SeparatedSyntaxList<TNode> items, SyntaxNode? parent)
        where TClone : SyntaxNode
        where TNode : Microsoft.CodeAnalysis.SyntaxNode
    {
        SeparatedSyntaxList<TClone> Result = new();

        foreach (var Item in items)
            Result.Add(Clone<TClone, TNode>(Item, parent));

        return Result;
    }

    public static TClone Clone<TClone, TNode>(TNode node, SyntaxNode? parent)
        where TClone : SyntaxNode
    {
        if (typeof(TNode).IsAbstract)
        {
            var StaticMethod = typeof(TClone).GetMethod("From", BindingFlags.Public | BindingFlags.Static)!;
            return (TClone)StaticMethod.Invoke(null, [node, parent])!;
        }
        else
        {
            var Constructor = typeof(TClone).GetConstructors(BindingFlags.Public | BindingFlags.Instance)[0];
            return (TClone)Constructor.Invoke(null, [node, parent])!;
        }
    }
}
