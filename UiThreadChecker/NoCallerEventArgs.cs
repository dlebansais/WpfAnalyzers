namespace UiThreadChecker;

using System;

public class NoCallerEventArgs(string variableName, string methodName) : EventArgs
{
    public string VariableName { get; } = variableName;
    public string MethodName { get; } = methodName;
}
