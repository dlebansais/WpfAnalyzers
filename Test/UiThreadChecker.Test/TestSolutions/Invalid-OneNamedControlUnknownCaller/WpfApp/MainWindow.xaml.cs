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

    public async Task TestControlMethod1()
    {
        var func = () =>
        {
            testControl.Visibility = Visibility.Visible;
        };

        await Task.Run(func);
    }

    public void TestControlMethod2()
    {
        RunAction(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }

    private static void RunAction(Action action)
    {
        action();
    }

    public void TestControlMethod3()
    {
        Runner.Run(() =>
        {
            testControl.Visibility = Visibility.Visible;
        });
    }

    private static void RunAction(Action action)
    {
        action();
    }
}
