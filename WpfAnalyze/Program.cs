namespace WpfAnalyze;

using UiThreadChecker;

internal partial class Program
{
    private static async Task<bool> ExecuteProgram(string solutionPath, string? projectName = null)
    {
        UiThreadChecker checker = new();
        checker.NoCallerEvent += (sender, e) =>
        {
            ConsoleDebug.Write($"No caller found for {e.MethodName} while checking '{e.VariableName}'.", isWarning: true);
        };
        checker.BadCallerEvent += (sender, e) =>
        {
            string message = $"Bad {(e.IsAwaiter ? "awaiter" : "caller")} found in {e.MethodName}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}, while checking '{e.VariableName}'.";
            ConsoleDebug.Write(message, isError: true);
        };
        checker.UnknownCallerEvent += (sender, e) =>
        {
            string message = $"Unknown caller found for {e.MethodName}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}, while checking '{e.VariableName}'.";
            ConsoleDebug.Write(message, isError: true);
        };

        await checker.CheckSolutionAsync(solutionPath, "Debug", "net481", projectName).ConfigureAwait(false);

        return false;
    }
}
