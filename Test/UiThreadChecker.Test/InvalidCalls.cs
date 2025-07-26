namespace UiThreadChecker.Test;

using System.Threading.Tasks;
using NUnit.Framework;

[TestFixture]
public partial class InvalidCalls
{
    [Test]
    public async Task AppOneNamedControlAfterConfigureAwait()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = false };
        await Helper.CheckSolutionAsync("Invalid-OneNamedControlAfterConfigureAwait", checkContext);

        Assert.That(checkContext.BadCallerCount, Is.GreaterThanOrEqualTo(1));
    }

    [Test]
    public async Task AppOneNamedControlCallInTaskRun()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = false };
        await Helper.CheckSolutionAsync("Invalid-OneNamedControlCallInTaskRun", checkContext);

        Assert.That(checkContext.BadCallerCount, Is.EqualTo(1));
    }

    [Test]
    public async Task AppOneObservableCollectionAfterConfigureAwait()
    {
        CheckContext checkContext = new() { AssertNoCaller = true, AssertBadCaller = false };
        await Helper.CheckSolutionAsync("Invalid-OneObservableCollectionAfterConfigureAwait", checkContext);

        Assert.That(checkContext.BadCallerCount, Is.EqualTo(1));
    }
}
