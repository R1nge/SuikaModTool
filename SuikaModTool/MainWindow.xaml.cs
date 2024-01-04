using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SuikaModTool;

public partial class MainWindow : Window
{
    private int _suikaCount = 12;
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
        if (OpenImageFileDialog(out var dialog) == true)
        {
            _configWithFullPathToCopyFilesFrom.ModIconPath = dialog.FileName;
            _configToSave.ModIconPath = dialog.SafeFileName;
            var button = (Button)sender;
            button.Content = _configWithFullPathToCopyFilesFrom.ModIconPath;
        }
    }

    private void SelectContainer(object sender, RoutedEventArgs e)
    {
        if (OpenImageFileDialog(out var dialog) == true)
        {
            _configWithFullPathToCopyFilesFrom.ContainerImagePath = dialog.FileName;
            _configToSave.ContainerImagePath = dialog.SafeFileName;
            var button = (Button)sender;
            button.Content = _configWithFullPathToCopyFilesFrom.ContainerImagePath;
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
        if (StringIsInvalid(_configToSave.ModName))
        {
            MessageBox.Show("Mod name is empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (StringIsInvalid(_configToSave.ModIconPath))
        {
            MessageBox.Show("Mod icon path is empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (StringIsInvalid(_configToSave.ContainerImagePath))
        {
            MessageBox.Show("Container image path is empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;

        if (_configToSave.SuikaSkinsImagesPaths.Length != _suikaCount)
        {
            MessageBox.Show("Suika skins images paths are empty", "Save error", MessageBoxButton.OK,
                MessageBoxImage.Error);
            return false;
        }

        if (_configToSave.SuikaIconsPaths.Length != _suikaCount)
        {
            MessageBox.Show("Suika icons paths are empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (_configToSave.SuikaAudioPaths.Length != _suikaCount)
        {
            MessageBox.Show("Suika audio paths are empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        if (_configToSave.SuikaDropChances.Length != _suikaCount)
        {
            MessageBox.Show("Suika drop chances are empty", "Save error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        return true;
    }

    private bool StringIsInvalid(string str)
    {
        return string.IsNullOrEmpty(str) || string.IsNullOrWhiteSpace(str);
    }

    private bool? OpenImageFileDialog(out OpenFileDialog imageDialog)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Preview Image",
            DefaultExt = ".jpg",
            Filter = "Image Files(*.jpg;*.jpeg;*.bmp)|*.jpg;*.jpeg;.bmp;"
        };

        imageDialog = dialog;
        
        bool? result = dialog.ShowDialog();

        return result;
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
        File.Copy(_configWithFullPathToCopyFilesFrom.ModIconPath, fullIconPath, true);
        
        
        var fullContainerPath = Path.Combine(fullSavePath, _configToSave.ContainerImagePath);
        File.Copy(_configWithFullPathToCopyFilesFrom.ContainerImagePath, fullContainerPath, true);
    }
}