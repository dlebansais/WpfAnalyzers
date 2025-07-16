namespace UiThreadChecker;

using Microsoft.CodeAnalysis.FindSymbols;

internal record CallerInfo(string VariableName, SymbolCallerInfo SymbolCaller, int Indentation)
{
}
