namespace WpfAnalyzers.Test;

extern alias Analyzers;

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = CSharpAnalyzerVerifier<Analyzers.WpfAnalyzers.WPFA1001MissingInitializeComponents>;

[TestClass]
public partial class WPFA1001UnitTests
{
    [TestMethod]
    public async Task NoInitializeComponent_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        DataContext = this;
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task InitializeComponent_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task NotVisual_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class Foo
{
    public Foo()
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionBodyWithInitializeComponent_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task NoInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        _foo = 0;
    }

    private int _foo;
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionNotInvocation_Diagnostic()
    {
        var DescriptorCS0201 = new DiagnosticDescriptor(
            "CS0201",
            "title",
            "Only assignment, call, increment, decrement, await, and new object expressions can be used as a statement",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var Expected = new DiagnosticResult(DescriptorCS0201);
        Expected = Expected.WithLocation("/0/Test0.cs", 20, 9);

        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        (long)Initialize();
    }

    private int Initialize()
    {
        return 0;
    }
}
", Expected).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ComplexInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        Foo.Bar();
    }
}

public static class Foo
{
    public static void Bar()
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task OtherInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        Initialize();
    }

    private void Initialize()
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task InvocationWithParameter_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]()
    {
        InitializeComponent(0);
    }

    public void InitializeComponent(int n)
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionBodyWithNoInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]() => _foo = 0;

    private int _foo;
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionBodyWithComplexInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]() => Foo.Bar();
}

public static class Foo
{
    public static void Bar()
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionBodyWithOtherInvocation_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]() => Initialize();

    private void Initialize()
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task ExpressionBodyInvocationWithParameter_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public [|MainWindow|]() => InitializeComponent(0);

    public void InitializeComponent(int n)
    {
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task InstructionBefore_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        for (;;) {}
        InitializeComponent();
        DataContext = this;
    }
}
").ConfigureAwait(false);
    }
}
