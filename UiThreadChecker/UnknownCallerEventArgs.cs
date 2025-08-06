namespace UiThreadChecker;

using System;
using Microsoft.CodeAnalysis;

public class UnknownCallerEventArgs(string variableName, ISymbol callerSymbol, int lineNumber) : EventArgs
{
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = callerSymbol.ToDisplayString();
    public int LineNumber { get; } = lineNumber;
}
