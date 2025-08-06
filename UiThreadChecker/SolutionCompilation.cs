namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Microsoft.CodeAnalysis;

internal class SolutionCompilation(Solution solution)
{
    public Solution Solution { get; } = solution;

    private readonly List<Project> projects = new(solution.Projects);
    private readonly Dictionary<Project, Compilation> compilations = new();
    private readonly Dictionary<SyntaxTree, Compilation> syntaxTrees = new();

    public async Task Init()
    {
        foreach (Project project in projects)
        {
            Console.WriteLine($"Compiling project {project.Name}...");

            Compilation compilation = await project.GetCompilationAsync().ConfigureAwait(false) ?? throw new InvalidOperationException();
            compilations.Add(project, compilation);

            foreach (SyntaxTree syntaxTree in compilation.SyntaxTrees)
                syntaxTrees.Add(syntaxTree, compilation);
        }
    }

    public bool IsWpfProject(Project project)
    {
        Debug.Assert(compilations.ContainsKey(project));

        return GetTypeSymbol(project, typeof(Visual)) is not null;
    }

    public Compilation GetCompilation(Project project)
    {
        Debug.Assert(projects.Contains(project));

        return compilations[project];
    }

    public SemanticModel GetSemanticModel(SyntaxTree syntaxTree)
    {
        Debug.Assert(syntaxTrees.ContainsKey(syntaxTree));

        Compilation compilation = syntaxTrees[syntaxTree];
        SemanticModel? semanticModel = compilation.GetSemanticModel(syntaxTree);

        return semanticModel;
    }

    public INamedTypeSymbol? GetTypeSymbol(Project project, Type type)
    {
        string? fullName = type.FullName;
        Debug.Assert(fullName is not null);

        Compilation compilation = compilations[project];
        return compilation.GetTypeByMetadataName(fullName);
    }
}
