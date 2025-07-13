namespace WpfAnalyze;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.FindSymbols;
using Microsoft.CodeAnalysis.MSBuild;

internal class Program
{
    private static async Task Main(string[] args)
    {
        const string solutionPath = @"C:\Projects\PgSearch\PgSearch.sln";
        const string projectName = "PgSearch";

        var properties = new Dictionary<string, string>
        {
            { "DesignTimeBuild", "true" },
            { "BuildingInsideVisualStudio", "true" },
            { "AlwaysCompileMarkupFilesInSeparateDomain", "false" },
            { "ProvideCommandLineArgs", "true" },
            { "Configuration", "Debug" },
            { "TargetFramework", "net481" },
        };

        var msWorkspace = MSBuildWorkspace.Create(properties);
        var solution = msWorkspace.OpenSolutionAsync(solutionPath).Result;
        var project = solution.Projects.First(proj => proj.Name == projectName);
        var compilation = project.GetCompilationAsync().Result ?? throw new InvalidOperationException();

        Dictionary<VariableDeclaratorSyntax, string> wpfFieldDeclarations = [];
        foreach (var doc in project.Documents)
        {
            var syntaxRoot = await doc.GetSyntaxRootAsync();
            if (syntaxRoot is CompilationUnitSyntax compilationUnit)
            {
                Dictionary<ClassDeclarationSyntax, string> classToNamespace = [];
                EnumerateClassDeclarations(compilationUnit.Members, string.Empty, classToNamespace);

                foreach (KeyValuePair<ClassDeclarationSyntax, string> entry in classToNamespace)
                {
                    foreach (var member in entry.Key.Members)
                    {
                        if (member is FieldDeclarationSyntax FieldDeclaration &&
                            FieldDeclaration.Declaration.Variables.Count == 1 &&
                            IsWpfGeneratedField(FieldDeclaration))
                        {
                            VariableDeclaratorSyntax variableDeclarator = FieldDeclaration.Declaration.Variables.First();
                            wpfFieldDeclarations.Add(variableDeclarator, entry.Value);
                            Console.WriteLine($"Found WPF field declaration: {variableDeclarator.Identifier.Text} in class {entry.Value}");
                        }
                    }
                }
            }
        }

        foreach (KeyValuePair<VariableDeclaratorSyntax, string> entry in wpfFieldDeclarations)
        {
            VariableDeclaratorSyntax variableDeclarator = entry.Key;
            string classPath = entry.Value;
            string clasName = classPath.Split('.').Last();

            var classDeclarations = await SymbolFinder.FindDeclarationsAsync(project, clasName, ignoreCase: false);

            foreach (var classDeclaration in classDeclarations)
            {
                if (classDeclaration is ITypeSymbol typeDeclaration && typeDeclaration.ContainingNamespace.ToString() is string containingNamespace)
                {
                    string symbolClassPath = containingNamespace + "." + typeDeclaration.Name;
                    if (classPath == symbolClassPath)
                    {
                        var members = typeDeclaration.GetMembers();
                        var symbol = (IFieldSymbol)members.First(m => m.Name == variableDeclarator.Identifier.Text);
                        var allReferences = await SymbolFinder.FindReferencesAsync(symbol, solution);

                        Console.WriteLine($"Found {allReferences.Count()} use(s) for field {variableDeclarator.Identifier.Text}");
                    }
                }
            }
        }
    }

    private static void EnumerateClassDeclarations(SyntaxList<MemberDeclarationSyntax> members, string namespacePath, Dictionary<ClassDeclarationSyntax, string> classToNamespace)
    {
        foreach (MemberDeclarationSyntax member in members)
        {
            if (member is NamespaceDeclarationSyntax namespaceDeclaration)
            {
                string newPath = namespaceDeclaration.Name.ToString();
                string localPath = namespacePath + "." + newPath;
                EnumerateClassDeclarations(namespaceDeclaration.Members, localPath, classToNamespace);
            }
            else if (member is FileScopedNamespaceDeclarationSyntax fileScopedNamespaceDeclaration)
            {
                string newPath = fileScopedNamespaceDeclaration.Name.ToString();
                string localPath = namespacePath + "." + newPath;
                EnumerateClassDeclarations(fileScopedNamespaceDeclaration.Members, localPath, classToNamespace);
            }
            else if (member is ClassDeclarationSyntax classDeclaration)
            {
                string classPath = namespacePath + "." + classDeclaration.Identifier.Text;
                classPath = classPath.TrimStart('.'); // Remove leading dot if any
                classToNamespace.Add(classDeclaration, classPath);
            }
        }
    }

    private static bool IsWpfGeneratedField(FieldDeclarationSyntax fieldDeclaration)
    {
        var leadingTrivia = fieldDeclaration.GetLeadingTrivia();

        foreach (var trivia in leadingTrivia)
        {
            if (trivia.IsKind(SyntaxKind.LineDirectiveTrivia) &&
                trivia.HasStructure &&
                trivia.GetStructure() is LineDirectiveTriviaSyntax LineDirectiveTrivia &&
                LineDirectiveTrivia.File.IsKind(SyntaxKind.StringLiteralToken))
            {
                string text = LineDirectiveTrivia.File.ToString().Trim('"');
                if (text.EndsWith(".xaml"))
                {
                    return true;
                }
            }
        }

        return false;
    }
}

