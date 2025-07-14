namespace WpfAnalyze;

using UiThreadChecker;

internal class Program
{
    private static async Task Main(string[] args)
    {
        const string solutionPath = @"C:\Projects\PgSearch\PgSearch.sln";
        const string projectName = "PgSearch";

        UiThreadChecker checker = new();
        checker.NoCallerEvent += (sender, e) =>
        {
            ConsoleDebug.Write($"No caller found for {e.Name}", isWarning: true);
        };
        checker.BadCallerEvent += (sender, e) =>
        {
            string message = $"Bad caller found in {e.Name}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}";
            ConsoleDebug.Write(message, isError: true);
        };
        checker.UnknownCallerEvent += (sender, e) =>
        {
            string message = $"Unknown caller found for {e.Name}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}";
            ConsoleDebug.Write(message, isError: true);
        };

        await checker.CheckSolutionAsync(solutionPath, "Debug", "net481", projectName).ConfigureAwait(false);
    }
}
