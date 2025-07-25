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
        await OnControlButtonClickOutsideTaskRun(testGrid1, continueOnCapturedContext: true);
    }

    private async void OnButtonClick2(object sender, RoutedEventArgs e)
    {
        await OnControlButtonClickOutsideTaskRun(testGrid2, continueOnCapturedContext: false);
    }

    private async void OnButtonClick3(object sender, RoutedEventArgs e)
    {
        await OnControlButtonClickInsideTaskRun(testGrid2);
    }

    private async void OnButtonClick4(object sender, RoutedEventArgs e)
    {
        await OnCollectionButtonClickOutsideTaskRun(Items3, continueOnCapturedContext: true);
    }

    private async void OnButtonClick5(object sender, RoutedEventArgs e)
    {
        await OnCollectionButtonClickOutsideTaskRun(Items4, continueOnCapturedContext: false);
    }

    private async void OnButtonClick6(object sender, RoutedEventArgs e)
    {
        await OnCollectionButtonClickInsideTaskRun(Items4);
    }

    private async Task OnControlButtonClickOutsideTaskRun(Grid grid, bool continueOnCapturedContext)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);
        }).ConfigureAwait(continueOnCapturedContext);

        grid.Visibility = Visibility.Collapsed;
    }

    private async Task OnControlButtonClickInsideTaskRun(Grid grid)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);

            grid.Visibility = Visibility.Collapsed;
        }).ConfigureAwait(false);
    }

    private async Task OnCollectionButtonClickOutsideTaskRun(ObservableCollection<string> items, bool continueOnCapturedContext)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);
        }).ConfigureAwait(continueOnCapturedContext);

        items.Add("Clicked");
    }

    private async Task OnCollectionButtonClickInsideTaskRun(ObservableCollection<string> items)
    {
        await Task.Run(async () =>
        {
            await Task.Delay(1000);

            items.Add("Clicked");
        }).ConfigureAwait(false);
    }

    public ObservableCollection<string> Items3 { get; } = new();
    public ObservableCollection<string> Items4 { get; } = new();
}
