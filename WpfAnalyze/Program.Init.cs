namespace WpfAnalyze;

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using McMaster.Extensions.CommandLineUtils;

/// <summary>
/// Generates a .nuspec file based on project .csproj content.
/// </summary>
[Command(ExtendedHelpText = @"
Analyzes a WPF projet to find controls and other components that are not called from the GUI thread, making the application crash.
")]
internal partial class Program
{
    /// <summary>
    /// Program entry point.
    /// </summary>
    /// <param name="arguments">Command-line arguments.</param>
    /// <returns>-1 in case of error; otherwise 0.</returns>
    public static int Main(string[] arguments) => RunAndSetResult(CommandLineApplication.Execute<Program>(arguments));

    [Argument(0, Description = "The path to the solution file.")]
    [Required]
    private string SolutionPath { get; set; } = string.Empty;

    [Option(CommandOptionType.SingleOrNoValue, Description = "The project name. If not provided, all projects are analyzed.", ShortName = "p", LongName = "project", ValueName = "Project Name")]
    private string? ProjectName { get; set; }

#pragma warning disable IDE0060 // Remove unused parameter
    private static int RunAndSetResult(int ignored) => ExecuteResult;
#pragma warning restore IDE0060 // Remove unused parameter

    private static int ExecuteResult = -1;

#pragma warning disable IDE0051 // Remove unused private members: this member is called through reflection from the McMaster.Extensions.CommandLineUtils tool.
    private void OnExecute()
#pragma warning restore IDE0051 // Remove unused private members
    {
        try
        {
            ShowCommandLineArguments();
            bool HasErrors = ExecuteProgram(SolutionPath, ProjectName).Result;

            ExecuteResult = HasErrors ? -1 : 0;
        }
        catch (Exception e)
        {
            PrintException(e);
        }
    }

    private void ShowCommandLineArguments()
    {
        if (SolutionPath.Length > 0)
            ConsoleDebug.Write($"Path: '{SolutionPath}'.");

        if (ProjectName is not null && ProjectName.Length > 0)
            ConsoleDebug.Write($"Project Name: '{ProjectName}'.");
    }

    private static void PrintException(Exception e)
    {
        Exception? CurrentException = e;

        do
        {
            ConsoleDebug.Write("***************");
            ConsoleDebug.Write(CurrentException.Message);

            // If CurrentException.StackTrace is null, StackTrace is set to string.Empty.
            string? StackTrace = Convert.ToString((object?)CurrentException.StackTrace, CultureInfo.InvariantCulture);
            ConsoleDebug.Write($"{StackTrace}");

            CurrentException = CurrentException.InnerException;
        }
        while (CurrentException is not null);
    }
}
