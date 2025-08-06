namespace UiThreadChecker;

using Microsoft.CodeAnalysis;

internal class ThreadRestrictedMember(SyntaxToken identifier, string memberPath)
{
    public SyntaxToken Identifier { get; } = identifier;

    public string Name { get; } = identifier.Text;

    public string MemberPath { get; } = memberPath;
}
