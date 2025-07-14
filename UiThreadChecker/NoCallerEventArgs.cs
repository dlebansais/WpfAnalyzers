namespace UiThreadChecker;

using System;

public class NoCallerEventArgs(string name) : EventArgs
{
    public string Name { get; } = name;
}
