﻿//HintName: Program_HelloFrom_3311014053.g.cs
#nullable enable

namespace Contracts.TestSuite;

using System;
using System.CodeDom.Compiler;
using Contracts;

partial class Program
{
    [GeneratedCodeAttribute("Method.Contracts.Analyzers","1.6.1.20")]
    public static string HelloFrom(string Text)
    {
        var Result = HelloFromVerified(Text);

        return Result;
    }
}