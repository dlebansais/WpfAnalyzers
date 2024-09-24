﻿namespace Contracts.Analyzers.Test;

using System.Threading.Tasks;
using NUnit.Framework;
using VerifyTests;

[TestFixture]
public class TestRequireNotNull
{
    [Test]
    public async Task TestCommand()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    [Access(""public"", ""static"")]
    [RequireNotNull(""text"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestQuery()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        string Text = HelloFrom(""Hello, World"");
        Console.WriteLine(Text);
    }

    [Access(""public"", ""static"")]
    [RequireNotNull(""text"")]
    private static string HelloFromVerified(string text)
    {
        return text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestAccessLast()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text"")]
    [Access(""public"", ""static"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDefaultAccess()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDefaultAsyncCommand()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using System.Threading.Tasks;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Task.Run(async () => {
            await HelloFrom(""Hello, World"", out string Text);
            Console.WriteLine(Text);
        });
    }

    [RequireNotNull(""text"")]
    private static async Task HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
        return Task.CompletedTask;
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestExplicitAsyncCommand()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using System.Threading.Tasks;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Task.Run(async () => {
            await HelloFrom(""Hello, World"", out string Text);
            Console.WriteLine(Text);
        });
    }

    [Access(""public"", ""static"", ""async"")]
    [RequireNotNull(""text"")]
    private static async Task HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
        return Task.CompletedTask;
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestExplicitAsyncCommandNoUsing()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        System.Threading.Tasks.Task.Run(async () => {
            await HelloFrom(""Hello, World"", out string Text);
            Console.WriteLine(Text);
        });
    }

    [Access(""public"", ""static"", ""async"")]
    [RequireNotNull(""text"")]
    private static async System.Threading.Tasks.Task HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
        return System.Threading.Tasks.Task.CompletedTask;
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDefaultAsyncQuery()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using System.Threading.Tasks;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Task.Run(async () => {
            string Text = await HelloFrom(""Hello, World"");
            Console.WriteLine(Text);
        });
    }

    [RequireNotNull(""text"")]
    private async static Task<string> HelloFromVerified(string text)
    {
        return await Task.FromResult(text + ""!"");
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestExplicitAsyncQuery()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using System.Threading.Tasks;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Task.Run(async () => {
            string Text = await HelloFrom(""Hello, World"");
            Console.WriteLine(Text);
        });
    }

    [Access(""public"", ""static"", ""async"")]
    [RequireNotNull(""text"")]
    private async static Task<string> HelloFromVerified(string text)
    {
        return await Task.FromResult(text + ""!"");
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestMultipleArguments()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, "", ""World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text1"", ""text2"")]
    private static void HelloFromVerified(string text1, string text2, out string textPlus)
    {
        textPlus = text1 + text2 + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestMultipleAttributes()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, "", ""World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text1"")]
    [RequireNotNull(""text2"")]
    private static void HelloFromVerified(string text1, string text2, out string textPlus)
    {
        textPlus = text1 + text2 + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestUpperCaseParameter()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        string Text = HelloFrom(""Hello, World"");
        Console.WriteLine(Text);
    }

    [Access(""public"", ""static"")]
    [RequireNotNull(""Text"")]
    private static string HelloFromVerified(string Text)
    {
        return Text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestNameof()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(nameof(text))]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestArgumentsOfDifferentTypes()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, "", ""World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text1"", ""text2"")]
    private static void HelloFromVerified(string text1, object text2, out string textPlus)
    {
        textPlus = $""{text1}{text2}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestArgumentsWithNullableTypes()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, "", ""World"", out string Text);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""text1"")]
    private static void HelloFromVerified(string text1, object? text2, out string textPlus)
    {
        textPlus = $""{text1}{text2}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDisposable()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using System.IO;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        Stream Stream = new FileStream(""foo.txt"", FileMode.Open, FileAccess.Read);
        string Text = HelloFrom(Stream);
        Console.WriteLine(Text);
    }

    [RequireNotNull(""stream"")]
    private static void HelloFromVerified(Stream stream)
    {
        StreamReader sr = new(stream);
        return sr.ReadToEnd();
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDefaultNameAlias()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", AliasName = ""Text"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }

    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestAliasName()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", AliasName = ""Foo"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestDefaultTypeAlias()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", Type = ""string"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = text + ""!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestTypeNoName()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", Type = ""object"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = $""{text}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestNameOnly()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", Name = ""newText"")]
    private static void HelloFromVerified(string text, out string textPlus)
    {
        textPlus = $""{text}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestTypeAndName()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""s"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""s""/>.</param>
    [RequireNotNull(""s"", Type = ""object"", Name = ""text"")]
    private static void HelloFromVerified(string s, out string textPlus)
    {
        textPlus = $""{s}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestTypeAndNameAndAlias()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""s"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""s""/>.</param>
    [RequireNotNull(""s"", Type = ""object"", Name = ""text"", AliasName = ""Foo"")]
    private static void HelloFromVerified(string s, out string textPlus)
    {
        textPlus = $""{s}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }

    [Test]
    public async Task TestTypeAndAlias()
    {
        // The source code to test
        const string Source = @"
namespace Contracts.TestSuite;

using System;
using Contracts;

internal partial class Program
{
    public static void Main(string[] args)
    {
        HelloFrom(""Hello, World"", out string Text);
        Console.WriteLine(Text);
    }

    /// <summary>
    /// Test doc.
    /// </summary>
    /// <param name=""text"">Test parameter 1.</param>
    /// <param name=""textPlus"">Test parameter 2, a copy of <paramref name=""text""/>.</param>
    [RequireNotNull(""text"", Type = ""string"", AliasName = ""Foo"")]
    private static void HelloFromVerified(object text, out string textPlus)
    {
        textPlus = $""{text}!"";
    }
}
";

        // Pass the source code to the helper and snapshot test the output.
        var Driver = TestHelper.GetDriver(Source);
        VerifyResult Result = await VerifyRequireNotNull.Verify(Driver).ConfigureAwait(false);

        Assert.That(Result.Files, Has.Exactly(1).Items);
    }
}
