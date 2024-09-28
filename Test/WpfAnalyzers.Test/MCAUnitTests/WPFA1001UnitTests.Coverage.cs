namespace WpfAnalyzers.Test;

extern alias Analyzers;

using System.Threading.Tasks;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VerifyCS = CSharpAnalyzerVerifier<Analyzers.WpfAnalyzers.WPFA1001MissingInitializeComponents>;

public partial class WPFA1001UnitTests
{
    [TestMethod]
    public async Task CoverageDirective_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(@"
#define COVERAGE_A25BDFABDDF8402785EB75AD812DA952
" + Prologs.Default, @"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = this;
    }
}
", LanguageVersion.Default, includeCore: true, includeFramework: true).ConfigureAwait(false);
    }

    [TestMethod]
    public async Task OldLanguageVersion_NoDiagnostic()
    {
        await VerifyCS.VerifyAnalyzerAsync(Prologs.Default, @"
public partial class MainWindow : Window
{
    public MainWindow()
    {
        DataContext = this;
    }
}
", Microsoft.CodeAnalysis.CSharp.LanguageVersion.CSharp6).ConfigureAwait(false);
    }
}
