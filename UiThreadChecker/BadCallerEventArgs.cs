namespace UiThreadChecker;

using System;
using Microsoft.CodeAnalysis;

public class BadCallerEventArgs(bool isAwaiter, string variableName, ISymbol callerSymbol, int lineNumber) : EventArgs
{
    public bool IsAwaiter { get; } = isAwaiter;
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = callerSymbol.ToDisplayString();
    public int LineNumber { get; } = lineNumber;
}
