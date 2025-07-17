namespace WpfApp1;

using System.Threading.Tasks;
using System.Windows;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    public void WithInvoke()
    {
        Dispatcher.Invoke(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }

    public void WithTypedInvoke()
    {
        _ = Dispatcher.Invoke(() =>
        {
            testControl.Visibility = Visibility.Visible;
            return true;
        });
    }

    public void WithBeginInvoke()
    {
        _ = Dispatcher.BeginInvoke(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }

    public async Task WithInvokeAsync()
    {
        await Dispatcher.InvokeAsync(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }

    public async Task WithTypedInvokeAsync()
    {
        _ = await Dispatcher.InvokeAsync(async () =>
        {
            testControl.Visibility = Visibility.Visible;
            return await Task.FromResult(true);
        });
    }
}
