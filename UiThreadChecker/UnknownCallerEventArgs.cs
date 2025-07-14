namespace UiThreadChecker;

using System;

public class UnknownCallerEventArgs(string name, int lineNumber) : EventArgs
{
    public string Name { get; } = name;
    public int LineNumber { get; } = lineNumber;
}
