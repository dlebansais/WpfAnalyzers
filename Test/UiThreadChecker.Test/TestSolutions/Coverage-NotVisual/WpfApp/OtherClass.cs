namespace Other;

using System.Collections.ObjectModel;
using System.Windows;

public class MainWindow
{
    public MainWindow()
    {
        Items.Add("Item 1");
    }

    public ObservableCollection<string> Items { get; } = new ObservableCollection<string>();
}
