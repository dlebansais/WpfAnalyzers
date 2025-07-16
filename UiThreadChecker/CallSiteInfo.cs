namespace UiThreadChecker;

using Microsoft.CodeAnalysis.FindSymbols;

internal class CallSiteInfo
{
    public required ResolvedCallType ResolvedCallType { get; init; }

    public SymbolCallerInfo Caller { get; init; }

    public int Indentation { get; init; }

    public int LineNumber { get; init; } = -1;

    public string VariableName { get; init; } = string.Empty;
}
