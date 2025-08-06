namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class MemberLocator
{
    public static async Task<List<ThreadRestrictedMember>> LocateWpfThreadRestrictedObjects(SolutionCompilation solutionCompilation, Project project)
    {
        List<ThreadRestrictedMember> result =
        [
            .. await LocateWpfNamedVisuals(project),
            .. await LocateObservableCollections(solutionCompilation, project)
        ];

        return result;
    }

    public static async Task<List<WpfVisualMember>> LocateWpfNamedVisuals(Project project)
    {
        List<WpfVisualMember> wpfVisualMembers = [];

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
                            wpfVisualMembers.Add(new WpfVisualMember(variableDeclarator.Identifier, entry.Value));
                            Console.WriteLine($"Found WPF field declaration: {variableDeclarator.Identifier.Text} in class {entry.Value}");
                        }
                    }
                }
            }
        }

        return wpfVisualMembers;
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

    public static async Task<List<ObservableCollectionMember>> LocateObservableCollections(SolutionCompilation solutionCompilation, Project project)
    {
        INamedTypeSymbol? ObservableCollectionTypeSymbol = solutionCompilation.GetTypeSymbol(project, typeof(ObservableCollection<>));
        Debug.Assert(ObservableCollectionTypeSymbol != null);

        List<ObservableCollectionMember> observableCollectionMembers = [];

        foreach (Document document in project.Documents)
        {
            if (await document.GetSyntaxRootAsync() is SyntaxNode root &&
                await document.GetSemanticModelAsync() is SemanticModel semanticModel)
            {
                IEnumerable<PropertyDeclarationSyntax> propertyDeclarations = root.DescendantNodes().OfType<PropertyDeclarationSyntax>();

                foreach (PropertyDeclarationSyntax declaration in propertyDeclarations)
                {
                    TypeInfo typeInfo = semanticModel.GetTypeInfo(declaration.Type);

                    if (typeInfo.Type is ITypeSymbol typeSymbol)
                    {
                        bool isObservableCollection = IsObservableCollection(typeSymbol.OriginalDefinition, ObservableCollectionTypeSymbol);
                        if (isObservableCollection)
                        {
                            if (semanticModel.GetDeclaredSymbol(declaration) is ISymbol variableInfo)
                            {
                                observableCollectionMembers.Add(new ObservableCollectionMember(declaration.Identifier, variableInfo.ContainingType.ToDisplayString()));
                            }
                        }
                    }
                }
            }
        }

        return observableCollectionMembers;
    }

    private static bool IsObservableCollection(ITypeSymbol typeSymbol, INamedTypeSymbol ObservableCollectionTypeSymbol)
    {
        if (typeSymbol is INamedTypeSymbol namedType)
        {
            string displayString = namedType.OriginalDefinition.ToDisplayString();
            return displayString is "System.Collections.ObjectModel.ObservableCollection<T>"
                                 or "ObservableCollection<>";
        }

        return false;
    }
}
