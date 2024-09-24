﻿namespace Contracts.Analyzers;

using System.Collections.Generic;

/// <summary>
/// Represents the model of a method contract.
/// </summary>
/// <param name="Namespace">The namespace containing the class that contains the method.</param>
/// <param name="UsingsBeforeNamespace">Using directives before the namespace declaration.</param>
/// <param name="UsingsAfterNamespace">Using directives after the namespace declaration.</param>
/// <param name="ClassName">The name of the class containing the method.</param>
/// <param name="DeclarationTokens">The token(s) to use for declaration (either 'class', 'struct', 'record' or 'record struct').</param>
/// <param name="FullClassName">The name of the class with type parameter and constraints.</param>
/// <param name="ShortMethodName">The method name, without the expected suffix.</param>
/// <param name="UniqueOverloadIdentifier">The unique identifier used to identify each overload of a multiply generated method.</param>
/// <param name="Documentation">The method documentation, if any.</param>
/// <param name="Attributes">The contract as attributes.</param>
/// <param name="GeneratedMethodDeclaration">The generated method.</param>
/// <param name="IsAsync">Whether the generated method is asynchronous.</param>
internal record ContractModel(string Namespace,
                              string UsingsBeforeNamespace,
                              string UsingsAfterNamespace,
                              string ClassName,
                              string DeclarationTokens,
                              string FullClassName,
                              string ShortMethodName,
                              string UniqueOverloadIdentifier,
                              string Documentation,
                              List<AttributeModel> Attributes,
                              string GeneratedMethodDeclaration,
                              bool IsAsync);
