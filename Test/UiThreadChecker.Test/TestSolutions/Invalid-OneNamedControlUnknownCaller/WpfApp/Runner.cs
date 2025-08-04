namespace WpfApp1;

using System;

public class Runner
{
    public static void Run(Action action)
    {
        action();
    }
}