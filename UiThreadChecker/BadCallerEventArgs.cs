namespace UiThreadChecker;

using System;

public class BadCallerEventArgs(bool isAwaiter, string variableName, string methodName, int lineNumber) : EventArgs
{
    public bool IsAwaiter { get; } = isAwaiter;
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = methodName;
    public int LineNumber { get; } = lineNumber;
}
