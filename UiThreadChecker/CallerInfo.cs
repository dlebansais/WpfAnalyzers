namespace UiThreadChecker;

using Microsoft.CodeAnalysis.FindSymbols;

internal record CallerInfo(SymbolCallerInfo SymbolCaller, int Indentation)
{
}
