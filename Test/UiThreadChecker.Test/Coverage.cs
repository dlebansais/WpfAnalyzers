namespace UiThreadChecker.Test;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
[Category("UiThreadChecker")]
public partial class Coverage
{
    [Test]
    public async Task AppMultipleClasses()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true, AssertUnknownCaller = true };
        await Helper.CheckSolutionAsync("Coverage-MultipleClasses", checkContext);
    }

    [Test]
    public async Task AppNotVisual()
    {
        CheckContext checkContext = new() { AssertNoCaller = false, AssertBadCaller = true, AssertUnknownCaller = true };
        await Helper.CheckSolutionAsync("Coverage-NotVisual", checkContext);
    }

    [Test]
    public async Task AppEmptyInstructions()
    {
        CheckContext checkContext = new() { AssertNoCaller = false, AssertBadCaller = true, AssertUnknownCaller = true };
        await Helper.CheckSolutionAsync("Coverage-EmptyInstructions", checkContext);
    }
}
