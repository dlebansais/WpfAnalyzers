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
        Dispatcher.Invoke(TestControlMethod2);
        Dispatcher.Invoke(TestControlMethod3);
        Dispatcher.Invoke(TestControlMethod4);
        Dispatcher.Invoke(TestControlMethod5);
        Dispatcher.Invoke(TestControlMethod6);
        Dispatcher.Invoke(TestControlMethod7);
        Dispatcher.Invoke(TestControlMethod8);
        Dispatcher.Invoke(TestControlMethod9);
        Dispatcher.Invoke(TestControlMethod10);
        Dispatcher.Invoke(TestControlMethod11);
        Dispatcher.Invoke(TestControlMethod12);
    }

    public async Task TestControlMethod1()
    {
        await Task.Run(() =>
        {
        }).ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod2()
    {
        _ = await Task.Run(() =>
        {
            return 42;
        }).ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod3()
    {
        if (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod4()
    {
        if (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
        }

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod5()
    {
        while (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod6()
    {
        while (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
        }

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod7()
    {
        do
        {
            testControl.Visibility = Visibility.Visible;
        }
        while (await TestControlMethod2().ConfigureAwait(true) == 42);
    }

    public async Task TestControlMethod8()
    {
        do
        {
        }
        while (await TestControlMethod2().ConfigureAwait(true) == 42);

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod9()
    {
        for (int i = 0; i < await TestControlMethod2().ConfigureAwait(true); i++)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod10()
    {
        for (int i = 0; i < await TestControlMethod2().ConfigureAwait(true); i++)
        {
        }

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod11()
    {
        for (int i = await TestControlMethod2().ConfigureAwait(true); i < 10; i++)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod12()
    {
        for (int i = await TestControlMethod2().ConfigureAwait(true); i < 10; i++)
        {
        }

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod12()
    {
        _ = await TestControlMethod2().ConfigureAwait(true) + await TestControlMethod2().ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;
    }
}
