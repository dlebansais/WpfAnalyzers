namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class MemberLocator
{
    public static async Task<Dictionary<VariableDeclaratorSyntax, string>> LocateWpfThreadRestrictedObjects(Project project)
    {
        Dictionary<VariableDeclaratorSyntax, string> result = new();
        foreach (KeyValuePair<VariableDeclaratorSyntax, string> entry in await LocateWpfNamedVisuals(project))
            _ = result.TryAdd(entry.Key, entry.Value);
        foreach (KeyValuePair<VariableDeclaratorSyntax, string> entry in await LocateObservableCollections(project))
            _ = result.TryAdd(entry.Key, entry.Value);

        return result;
    }

    public static async Task<Dictionary<VariableDeclaratorSyntax, string>> LocateWpfNamedVisuals(Project project)
    {
        Dictionary<VariableDeclaratorSyntax, string> namedVisuals = [];

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
                            namedVisuals.Add(variableDeclarator, entry.Value);
                            Console.WriteLine($"Found WPF field declaration: {variableDeclarator.Identifier.Text} in class {entry.Value}");
                        }
                    }
                }
            }
        }

        return namedVisuals;
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

    public static async Task<Dictionary<VariableDeclaratorSyntax, string>> LocateObservableCollections(Project project)
    {
        Dictionary<VariableDeclaratorSyntax, string> observableCollections = [];

        foreach (Document document in project.Documents)
        {
            if (await document.GetSyntaxRootAsync() is SyntaxNode root &&
                await document.GetSemanticModelAsync() is SemanticModel semanticModel)
            {
                IEnumerable<VariableDeclarationSyntax> variableDeclarations = root.DescendantNodes().OfType<VariableDeclarationSyntax>();

                foreach (VariableDeclarationSyntax declaration in variableDeclarations)
                {
                    TypeInfo typeInfo = semanticModel.GetTypeInfo(declaration.Type);

                    if (typeInfo.Type is ITypeSymbol typeSymbol && IsObservableCollection(typeSymbol))
                    {
                        foreach (var variable in declaration.Variables)
                        {
                            if (semanticModel.GetDeclaredSymbol(variable) is ISymbol variableInfo)
                            {
                                observableCollections.Add(variable, variableInfo.ContainingType.ToDisplayString());
                            }
                        }
                    }
                }
            }
        }

        return observableCollections;
    }

    private static bool IsObservableCollection(ITypeSymbol typeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedType)
        {
            return namedType.OriginalDefinition.ToDisplayString() ==
                   "ObservableCollection<>";
        }
        return false;
    }
}
