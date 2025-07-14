namespace UiThreadChecker;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

internal static class MemberLocator
{
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
}
