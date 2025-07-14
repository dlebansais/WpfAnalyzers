namespace WpfAnalyze;

using System;
using System.Diagnostics;

/// <summary>
/// Writes text to the console.
/// </summary>
internal static partial class ConsoleDebug
{
    /// <summary>
    /// Writes text to the console.
    /// </summary>
    /// <param name="text">The text to write.</param>
    /// <param name="isWarning">True if the text to write is a warning message.</param>
    /// <param name="isError">True if the text to write is an error message.</param>
    public static void Write(string text, bool isWarning = false, bool isError = false)
    {
        ConsoleColor OldColor = Console.ForegroundColor;

        if (isWarning)
            Console.ForegroundColor = ConsoleColor.Yellow;
        else if (isError)
            Console.ForegroundColor = ConsoleColor.Red;

        Console.WriteLine(text);
        Debug.WriteLine(text);

        if (isWarning || isError)
            Console.ForegroundColor = OldColor;
    }
}
