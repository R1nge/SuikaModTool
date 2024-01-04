using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SuikaModTool;

public partial class MainWindow : Window
{
    private GameConfig _configToSave = new();
    private string _savePath = null;
    
    public MainWindow()
    {
        InitializeComponent();
    }
    
    private void TitleChanged(object sender, TextChangedEventArgs args)
    {
        _configToSave.ModName = ((TextBox) sender).Text;
        Debug.WriteLine($"Title has changed to: {((TextBox) sender).Text}");
    }

    private void SelectPreviewImageClicked(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Preview Image",
            DefaultExt = ".jpg",
            Filter = "Image Files(*.jpg;*.jpeg;*.bmp)|*.jpg;*.jpeg;.bmp;"
        };
        
        bool? result = dialog.ShowDialog();
        
        if (result == true)
        {
            //TODO: append save file directory
            var path = dialog.SafeFileName;//Path.GetDirectoryName(dialog.FileName)!;;
            _configToSave.ModIconPath = path;
            Debug.WriteLine($"File Path: {path}");
        }
    }

    private void CreateModClicked(object sender, RoutedEventArgs e)
    {
        ShowSaveFileDialog();
        if (Validate())
        {
            CreateMod();    
        }
    }
    
    private void ShowSaveFileDialog()
    {
        string dummyFileName = "Save Here";

        SaveFileDialog sf = new SaveFileDialog
        {
            FileName = dummyFileName
        };

        if (sf.ShowDialog() == true)
        {
            string savePath = Path.GetDirectoryName(sf.FileName)!;
            _savePath = savePath;
            Debug.WriteLine($"SAVE PATH {_savePath}");
        }
    }

    private bool Validate()
    {
        return true;
    }

    private void CreateMod()
    {
        var json = JsonConvert.SerializeObject(_configToSave);

        var fullSavePath = Path.Combine(_savePath, _configToSave.ModName);
        if (!Directory.Exists(fullSavePath))
        {
            Directory.CreateDirectory(fullSavePath);
        }

        var fullJsonPath = Path.Combine(fullSavePath, "config.json");
        
        File.WriteAllText(fullJsonPath, json);
        Debug.WriteLine("Mod has been created");
    }
}