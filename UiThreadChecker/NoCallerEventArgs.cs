namespace UiThreadChecker;

using System;
using Microsoft.CodeAnalysis;

public class NoCallerEventArgs(string variableName, ISymbol callerSymbol) : EventArgs
{
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = callerSymbol.ToDisplayString();
}
