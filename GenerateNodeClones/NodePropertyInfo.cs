namespace GenerateNodeClones;

using System;

internal class NodePropertyInfo
{
    public bool IsToken { get; init; }
    public Type? Node { get; init; }
    public Type? List { get; init; }
    public Type? SeparatedList { get; init; }
    public bool IsNullable { get; init; }
}
