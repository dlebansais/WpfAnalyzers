﻿namespace WpfAnalyzers.Test;

public static class Prologs
{
    public const string Default = @"
using System;
using System.Windows;

public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector
{
    public void InitializeComponent()
    {
    }

    void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target)
    {
    }
}
";
}