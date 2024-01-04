using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SuikaModTool;

public partial class MainWindow : Window
{
    private GameConfig _configWithFullPathToCopyFilesFrom = new();
    private GameConfig _configToSave = new();
    private string _savePath = null;

    //TODO: load data from already existing mod

    public MainWindow()
    {
        InitializeComponent();
    }

    private void TitleChanged(object sender, TextChangedEventArgs args)
    {
        _configWithFullPathToCopyFilesFrom.ModName = ((TextBox)sender).Text;
        _configToSave.ModName = ((TextBox)sender).Text;
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
            _configWithFullPathToCopyFilesFrom.ModIconPath = dialog.FileName;
            _configToSave.ModIconPath = dialog.SafeFileName;
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
        if (string.IsNullOrEmpty(_configToSave.ModName) || string.IsNullOrWhiteSpace(_configToSave.ModName))
        {
            MessageBox.Show("Mod name is empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (string.IsNullOrEmpty(_configToSave.ModIconPath) || string.IsNullOrWhiteSpace(_configToSave.ModIconPath))
        {
            MessageBox.Show("Mod icon path is empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

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
        
        CopyFiles(fullSavePath);
        
        Debug.WriteLine("Mod has been created");
    }

    private void CopyFiles(string fullSavePath)
    {
        var fullIconPath = Path.Combine(fullSavePath, _configToSave.ModIconPath);
        File.Copy(_configWithFullPathToCopyFilesFrom.ModIconPath, fullIconPath);
    }
}