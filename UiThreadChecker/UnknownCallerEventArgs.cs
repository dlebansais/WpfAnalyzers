namespace UiThreadChecker;

using System;

public class UnknownCallerEventArgs(string variableName, string methodName, int lineNumber) : EventArgs
{
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = methodName;
    public int LineNumber { get; } = lineNumber;
}
