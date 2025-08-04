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

        Dispatcher.Invoke(TestControlMethod0);
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
        Dispatcher.Invoke(TestControlMethod13);
        Dispatcher.Invoke(TestControlMethod14);
        Dispatcher.Invoke(TestControlMethod15);
        Dispatcher.Invoke(TestControlMethod16);
        Dispatcher.Invoke(TestControlMethod17);
    }

    public async Task TestControlMethod0()
    {
        await Task.Run(() =>
        {
        }).ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;
    }

    public async Task TestControlMethod1()
    {
        await Task.Run(() =>
        {
        }).ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;

        await Task.Run(() =>
        {
        }).ConfigureAwait(false);
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
        else
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod5()
    {
        if (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
        }

        testControl.Visibility = Visibility.Visible;

        if (await TestControlMethod2().ConfigureAwait(false) == 42)
        {
        }
    }

    public async Task TestControlMethod6()
    {
        while (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod7()
    {
        while (await TestControlMethod2().ConfigureAwait(true) == 42)
        {
        }

        testControl.Visibility = Visibility.Visible;

        while (await TestControlMethod2().ConfigureAwait(false) == 42)
        {
        }
    }

    public async Task TestControlMethod8()
    {
        do
        {
            testControl.Visibility = Visibility.Visible;
        }
        while (await TestControlMethod2().ConfigureAwait(true) == 42);
    }

    public async Task TestControlMethod9()
    {
        do
        {
        }
        while (await TestControlMethod2().ConfigureAwait(true) == 42);

        testControl.Visibility = Visibility.Visible;

        do
        {
        }
        while (await TestControlMethod2().ConfigureAwait(false) == 42);
    }

    public async Task TestControlMethod10()
    {
        for (int i = 0; i < await TestControlMethod2().ConfigureAwait(true); i++)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod11()
    {
        for (int i = 0; i < await TestControlMethod2().ConfigureAwait(true); i++)
        {
        }

        testControl.Visibility = Visibility.Visible;

        for (int i = 0; i < await TestControlMethod2().ConfigureAwait(false); i++)
        {
        }
    }

    public async Task TestControlMethod12()
    {
        for (int i = await TestControlMethod2().ConfigureAwait(true); i < 10; i++)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod13()
    {
        for (int i = await TestControlMethod2().ConfigureAwait(true); i < 10; i++)
        {
        }

        testControl.Visibility = Visibility.Visible;

        for (int i = await TestControlMethod2().ConfigureAwait(false); i < 10; i++)
        {
        }
    }

    public async Task TestControlMethod14()
    {
        for (,await TestControlMethod0().ConfigureAwait(true); i < 10; i++)
        {
            testControl.Visibility = Visibility.Visible;
        }
    }

    public async Task TestControlMethod15()
    {
        for (,await TestControlMethod0().ConfigureAwait(true); i < 10; i++)
        {
        }

        testControl.Visibility = Visibility.Visible;

        for (,await TestControlMethod0().ConfigureAwait(false); i < 10; i++)
        {
        }
    }

    public async Task TestControlMethod16()
    {
        _ = await TestControlMethod2().ConfigureAwait(true) + await TestControlMethod2().ConfigureAwait(true);

        testControl.Visibility = Visibility.Visible;

        _ = await TestControlMethod2().ConfigureAwait(true) + await TestControlMethod2().ConfigureAwait(false);
    }

    public async Task TestControlMethod17()
    {
        _ = (await TestControlMethod2().ConfigureAwait(true)).GetHashCode();

        testControl.Visibility = Visibility.Visible;

        _ = (await TestControlMethod2().ConfigureAwait(false)).GetaHshCode();
    }
}
