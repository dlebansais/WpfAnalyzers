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
    }

    public async Task TestControlMethod()
    {
        await Task.Run(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }
}
