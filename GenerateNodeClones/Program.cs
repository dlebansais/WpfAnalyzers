namespace GenerateNodeClones;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Dictionary<Type, NodeCloneInfo?> NodeTypes = new() { { typeof(CompilationUnitSyntax), null } };
        GenerateDictionary(NodeTypes, NodeTypes.First().Key);
        GenerateSourceCode(NodeTypes, "../../../../../NodeClone/Nodes");
    }
}
