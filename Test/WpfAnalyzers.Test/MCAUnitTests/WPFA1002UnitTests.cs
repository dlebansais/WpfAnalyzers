namespace WpfAnalyzers.Test;

extern alias Analyzers;

using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = CSharpAnalyzerVerifier<Analyzers.WpfAnalyzers.WPFA1002AccessToObjectIsForbidden>;

[TestClass]
//[Ignore("Not working yet")]
public partial class WPFA1002UnitTests
{
    [TestMethod]
    public async Task NotUiThreadSet_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() => Access());
    }

    private void Access()
    {
        [|testBorder.Background|] = System.Windows.Media.Brushes.Black;
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task UiThreadSet_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        testBorder.Background = System.Windows.Media.Brushes.Black;
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task UnknownSymbol_NoDiagnostic()
    {
        var DescriptorCS0103 = new DiagnosticDescriptor(
            "CS0103",
            "title",
            "The name 'foo' does not exist in the current context",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var Expected = new DiagnosticResult(DescriptorCS0103);
        Expected = Expected.WithLocation("/0/Test0.cs", 30, 9);

        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() => Access());
    }

    private void Access()
    {
        foo.Background = System.Windows.Media.Brushes.Black;
    }
}
", expected: Expected).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task NotInVisual_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class OtherWindow
{
    public OtherWindow()
    {
        System.Threading.Tasks.Task.Run(() => Access());
    }

    private void Access()
    {
        testBorder.Background = System.Windows.Media.Brushes.Black;
    }

    private System.Windows.Controls.Border testBorder;
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task UnknownProperty_NoDiagnostic()
    {
        var DescriptorCS1061 = new DiagnosticDescriptor(
            "CS1061",
            "title",
            "'Border' does not contain a definition for 'Foo' and no accessible extension method 'Foo' accepting a first argument of type 'Border' could be found (are you missing a using directive or an assembly reference?)",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var Expected = new DiagnosticResult(DescriptorCS1061);
        Expected = Expected.WithLocation("/0/Test0.cs", 30, 20);

        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() => Access());
    }

    private void Access()
    {
        testBorder.Foo = System.Windows.Media.Brushes.Black;
    }
}
", expected: Expected).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task CallFromNonUiThreadToConstructor_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() => Access());
    }

    public MainWindow(int n)
    {
        InitializeComponent();
        DataContext = this;

        [|testBorder.Background|] = System.Windows.Media.Brushes.Black;
    }

    private void Access()
    {
        MainWindow obj = new(0);
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task CallFromNonUiThreadToLambda_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() =>
        {
            [|testBorder.Background|] = System.Windows.Media.Brushes.Black;
        });
    }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task CallFromNonUiThreadToExpressionBody_Diagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        System.Threading.Tasks.Task.Run(() =>
        {
            _ = Foo;
        });
    }

    private static System.Windows.Media.Brush Foo { get => [|testBorder.Background|]; }
}
").ConfigureAwait(false);
    }

    [TestMethod]
    public async Task CallFromGlobalStatement_NoDiagnostic()
    {
        var DescriptorCS0103 = new DiagnosticDescriptor(
            "CS0103",
            "title",
            "The name 'testBorder' does not exist in the current context",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var DescriptorCS8803 = new DiagnosticDescriptor(
            "CS8803",
            "title",
            "Top-level statements must precede namespace and type declarations.",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var DescriptorCS8805 = new DiagnosticDescriptor(
            "CS8805",
            "title",
            "Program using top-level statements must be an executable.",
            "description",
            DiagnosticSeverity.Error,
            true
            );

        var Expected1 = new DiagnosticResult(DescriptorCS0103);
        Expected1 = Expected1.WithLocation("/0/Test0.cs", 18, 1);

        var Expected2 = new DiagnosticResult(DescriptorCS8803);
        Expected2 = Expected2.WithLocation("/0/Test0.cs", 18, 1);

        var Expected3 = new DiagnosticResult(DescriptorCS8805);
        Expected3 = Expected3.WithLocation("/0/Test0.cs", 18, 1);

        await VerifyCS.VerifyAnalyzerAsync(@"
testBorder.Background = System.Windows.Media.Brushes.Black;
", includeCore: true, includeFramework: true, Expected1, Expected2, Expected3).ConfigureAwait(false);
    }
}
