namespace GenerateNodeClones;

using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

internal class NodeCloneInfo(Type type, Dictionary<string, NodePropertyInfo> propertiesInfo)
{
    public Dictionary<string, NodePropertyInfo> PropertiesInfo { get; } = propertiesInfo;
    public bool IsAbstract { get; } = type.IsAbstract;
    public string? BaseClassName { get; } = GetBaseClassName(type);

    private static string? GetBaseClassName(Type type)
    {
        if (type.BaseType is null || type.BaseType.Name == nameof(CSharpSyntaxNode))
            return null;

        return type.BaseType.Name;
    }
}
