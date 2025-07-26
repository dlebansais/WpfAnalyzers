namespace UiThreadChecker.Test;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public partial class ValidCalls
{
    [Test]
    public async Task AppNoNamedControl()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-NoNamedControl", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlNoCall()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlNoCall", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlNoCallTransitive()
    {
        CheckContext checkContext = new() { AssertNoCaller = false, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlNoCallTransitive", checkContext);

        Assert.That(checkContext.NoCallerCount, Is.GreaterThan(1));
    }

    [Test]
    public async Task AppOneNamedControlCallInConstructor()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlCallInConstructor", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlCallInEventHandler()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlCallInEventHandler", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlCallInOverride()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlCallInOverride", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlCallInDispatcher()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlCallInDispatcher", checkContext);
    }

    [Test]
    public async Task AppOneNamedControlAfterConfigureAwait()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = true };
        await Helper.CheckSolutionAsync("Valid-OneNamedControlAfterConfigureAwait", checkContext);
    }
}
