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

    public Grid TestControlProperty1 => testControl;

    public Grid TestControlProperty2
    {
        get
        {
            return testControl;
        }
        set
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public Grid TestControlMethod1() => testControl;

    public Grid TestControlMethod2()
    {
        return testControl;
    }

    public void TestControlMethod3()
    {
        testControl.Visibility = Visibility.Visible;
    }

    public async Task<Grid> TestControlMethod4()
    {
        return await Task.FromResult(testControl);
    }

    public async Task TestControlMethod5()
    {
        testControl.Visibility = Visibility.Visible;
        await Task.CompletedTask;
    }

    public Grid this[int index]
    {
        get
        {
            return testControl;
        }
    }
}