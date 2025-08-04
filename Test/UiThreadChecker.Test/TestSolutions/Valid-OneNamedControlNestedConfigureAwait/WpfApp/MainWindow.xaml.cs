namespace WpfApp1;

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
        _ = await TestControlMethod2().ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;

        _ = await TestControlMethod2().ConfigureAwait(false);
    }

    public async Task TestControlMethod2()
    {
        _ = await Task.Run(() =>
        {
            return 42;
        }).ConfigureAwait(false);
    }
}
