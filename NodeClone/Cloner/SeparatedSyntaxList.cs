namespace NodeClones;

using System.Collections.Generic;

public class SeparatedSyntaxList<TClone> : List<TClone>
    where TClone : SyntaxNode
{
}
