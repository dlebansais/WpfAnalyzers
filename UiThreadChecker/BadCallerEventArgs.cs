namespace UiThreadChecker;

using System;

public class BadCallerEventArgs(string name, int lineNumber) : EventArgs
{
    public string Name { get; } = name;
    public int LineNumber { get; } = lineNumber;
}
