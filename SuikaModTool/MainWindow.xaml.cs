using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;

namespace SuikaModTool;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void TitleChanged(object sender, TextChangedEventArgs args)
    {
        Debug.WriteLine($"Title has changed to: {((TextBox) sender).Text}");
    }
}