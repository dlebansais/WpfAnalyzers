namespace WpfCrashDemo;

using System.Collections.ObjectModel;
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
        DataContext = this;
    }

    private async void OnButtonClick1(object sender, RoutedEventArgs e)
    {
        await OnControlButtonClick(testGrid1, continueOnCapturedContext: true);
    }

    private async void OnButtonClick2(object sender, RoutedEventArgs e)
    {
        await OnControlButtonClick(testGrid2, continueOnCapturedContext: false);
    }

    private async void OnButtonClick3(object sender, RoutedEventArgs e)
    {
        await OnCollectionButtonClick(Items3, continueOnCapturedContext: true);
    }

    private async void OnButtonClick4(object sender, RoutedEventArgs e)
    {
        await OnCollectionButtonClick(Items4, continueOnCapturedContext: false);
    }

    private async Task OnControlButtonClick(Grid grid, bool continueOnCapturedContext)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);
        }).ConfigureAwait(continueOnCapturedContext);

        grid.Visibility = Visibility.Collapsed;
    }

    private async Task OnCollectionButtonClick(ObservableCollection<string> items, bool continueOnCapturedContext)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);
        }).ConfigureAwait(continueOnCapturedContext);

        items.Add("Clicked");
    }

    public ObservableCollection<string> Items3 { get; } = new();
    public ObservableCollection<string> Items4 { get; } = new();
}
