namespace UiThreadChecker;

using Microsoft.CodeAnalysis;

internal class WpfVisualMember(SyntaxToken identifier, string memberPath) : ThreadRestrictedMember(identifier, memberPath)
{
}
