#pragma warning disable CA1000 // Do not declare static members on generic types

namespace WpfAnalyzers.Test;

using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Testing;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.CodeAnalysis.Testing;

internal static partial class CSharpAnalyzerVerifier<TAnalyzer>
    where TAnalyzer : DiagnosticAnalyzer, new()
{
    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    public static async Task VerifyAnalyzerAsync(string source, bool includeCore = true, bool includeFramework = true, params DiagnosticResult[] expected)
        => await VerifyAnalyzerAsync(Prologs.Default, source, includeCore, includeFramework, expected).ConfigureAwait(true);

    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    public static async Task VerifyAnalyzerAsync(string prolog, string source, bool includeCore = true, bool includeFramework = true, params DiagnosticResult[] expected)
        => await VerifyAnalyzerAsync(prolog, source, LanguageVersion.Default, includeCore, includeFramework, expected).ConfigureAwait(true);

    /// <inheritdoc cref="AnalyzerVerifier{TAnalyzer, TTest, TVerifier}.VerifyAnalyzerAsync(string, DiagnosticResult[])"/>
    public static async Task VerifyAnalyzerAsync(string prolog, string source, LanguageVersion languageVersion = LanguageVersion.Default, bool includeCore = true, bool includeFramework = true, params DiagnosticResult[] expected)
    {
        var test = new Test
        {
            TestCode = prolog + source,
            Version = languageVersion,
            IncludeCore = includeCore,
            IncludeFramework = includeFramework,
        };

        test.ExpectedDiagnostics.AddRange(expected);
        await test.RunAsync(CancellationToken.None).ConfigureAwait(true);
    }
}
