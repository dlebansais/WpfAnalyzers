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
            if (checkContext.AssertNoCaller)
                Assert.Fail($"No caller found for {e.Name}");
            else
                Console.WriteLine($"No caller found for {e.Name}");

            checkContext.NoCallerCount++;
        };
        uiThreadChecker.BadCallerEvent += (sender, e) =>
        {
            string message = $"Bad caller found in {e.Name}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}";

            if (checkContext.AssertBadCaller)
                Assert.Fail(message);
            else
                Console.WriteLine(message);

            checkContext.BadCallerCount++;
        };
        uiThreadChecker.UnknownCallerEvent += (sender, e) =>
        {
            string message = $"Unknown caller found for {e.Name}{(e.LineNumber > 0 ? $", line {e.LineNumber}" : string.Empty)}";

            Assert.Fail(message);
        };

        await uiThreadChecker.CheckSolutionAsync(solutionPath, "Debug", "net8.0-windows", projectName);
    }
}
