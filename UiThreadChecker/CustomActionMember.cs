namespace UiThreadChecker;

using Microsoft.CodeAnalysis;

internal class CustomActionMember(SyntaxToken identifier, ISymbol symbol) : ThreadRestrictedMember(identifier, symbol.ToDisplayString())
{
    public ISymbol Symbol { get; } = symbol;
}
