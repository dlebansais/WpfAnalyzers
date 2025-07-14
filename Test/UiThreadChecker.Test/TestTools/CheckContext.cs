namespace UiThreadChecker.Test;

internal class CheckContext
{
    public required bool AssertNoCaller { get; init; }
    public required bool AssertBadCaller { get; init; }

    public int NoCallerCount { get; set; }

    public int BadCallerCount { get; set; }
}
