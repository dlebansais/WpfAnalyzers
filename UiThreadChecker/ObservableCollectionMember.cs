namespace UiThreadChecker;

using Microsoft.CodeAnalysis;

internal class ObservableCollectionMember(SyntaxToken identifier, string memberPath) : ThreadRestrictedMember(identifier, memberPath)
{
}
