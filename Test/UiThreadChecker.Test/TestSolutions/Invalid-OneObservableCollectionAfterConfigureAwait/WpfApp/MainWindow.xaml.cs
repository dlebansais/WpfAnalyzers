﻿namespace WpfApp1;

using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        Dispatcher.Invoke(TestControlMethod1);
    }

    public async Task TestControlMethod1()
    {
        await Task.Run(() =>
        {
        }).ConfigureAwait(false);

        observableCollection.Add("Test item");
    }

    public ObservableCollection<string> observableCollection = new();
}
