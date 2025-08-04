namespace UiThreadChecker.Test;

using System;
using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
internal static class Helper
{
    public static async Task CheckSolutionAsync(string testName, CheckContext checkContext)
    {
        string solutionPath = @$"..\..\..\..\TestSolutions\{testName}\WpfApp.sln";
        const string projectName = "WpfApp";

        UiThreadChecker uiThreadChecker = new();
        uiThreadChecker.NoCallerEvent += (sender, e) =>
        {
            string message = $"No caller found for {e.MethodName}  while checking '{e.VariableName}'.";

            if (checkContext.AssertNoCaller)
                Assert.Fail(message);
            else
                Console.WriteLine(message);

            checkContext.NoCallerCount++;
        };
        uiThreadChecker.BadCallerEvent += (sender, e) =>
        {
            string message = $"Bad {(e.IsAwaiter ? "awaiter" : "caller")} found in {e.MethodName}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}, while checking '{e.VariableName}'.";

            if (checkContext.AssertBadCaller)
                Assert.Fail(message);
            else
                Console.WriteLine(message);

            checkContext.BadCallerCount++;
        };
        uiThreadChecker.UnknownCallerEvent += (sender, e) =>
        {
            string message = $"Unknown caller found for {e.MethodName}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}, while checking '{e.VariableName}'.";

            if (checkContext.AssertUnknownCaller)
                Assert.Fail(message);
            else
                Console.WriteLine(message);

            checkContext.UnknownCallerCount++;
        };

        await uiThreadChecker.CheckSolutionAsync(solutionPath, "Debug", "net8.0-windows", projectName);
    }
}
