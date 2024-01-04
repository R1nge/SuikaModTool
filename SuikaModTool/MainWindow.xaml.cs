using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Newtonsoft.Json;

namespace SuikaModTool;

public partial class MainWindow : Window
{
    private int _suikaCount = 12;
    private GameConfig _configWithFullPathToCopyFilesFrom;
    private GameConfig _configToSave;
    private string _savePath = null;
    public ObservableCollection<MyPathData> SuikaSkinsPaths { get; set; }
    public ObservableCollection<MyPathData> SuikaIconsPaths { get; set; }
    public ObservableCollection<MyPathData> SuikaAudioPaths { get; set; }

    public class MyPathData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public MyPathData(string path)
        {
            Path = path;
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public string Path { get; set; }
    }

    //TODO: load data from already existing mod

    public MainWindow()
    {
        InitializeComponent();

        SuikaSkinsPaths = new ObservableCollection<MyPathData>();
        SuikaIconsPaths = new ObservableCollection<MyPathData>();
        SuikaAudioPaths = new ObservableCollection<MyPathData>();
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

    private void SelectSuikaSkinsClicked(object sender, RoutedEventArgs e)
    {
        SelectSuikaSkins();
    }

    private void SelectSuikaSkins()
    {
        if (SuikaSkinsPaths.Count == _suikaCount)
        {
            MessageBox.Show("Max number of suika skins reached");
            return;
        }

        if (OpenMultipleFilesDialogImages(out var dialog) == true)
        {
            _configToSave.SuikaSkinsImagesPaths = new string[_suikaCount];
            _configWithFullPathToCopyFilesFrom.SuikaSkinsImagesPaths = new string[_suikaCount];

            for (var i = 0; i < dialog.SafeFileNames.Length; i++)
            {
                var fileName = dialog.SafeFileNames[i];
                if (i < _suikaCount)
                {
                    _configToSave.SuikaSkinsImagesPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                var fileName = dialog.FileNames[i];
                if (i < _suikaCount)
                {
                    _configWithFullPathToCopyFilesFrom.SuikaSkinsImagesPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                SuikaSkinsPaths.Add(new MyPathData($"{SuikaSkinsPaths.Count + 1} " + dialog.FileNames[i]));
            }
        }
    }

    private void SelectSuikaIconsClicked(object sender, RoutedEventArgs e)
    {
        SelectSuikaIcons();
    }

    private void SelectSuikaIcons()
    {
        if (SuikaIconsPaths.Count == _suikaCount)
        {
            MessageBox.Show("Max number of suika icons reached");
            return;
        }

        if (OpenMultipleFilesDialogImages(out var dialog) == true)
        {
            _configToSave.SuikaIconsPaths = new string[_suikaCount];
            _configWithFullPathToCopyFilesFrom.SuikaIconsPaths = new string[_suikaCount];

            for (var i = 0; i < dialog.SafeFileNames.Length; i++)
            {
                var fileName = dialog.SafeFileNames[i];
                if (i < _suikaCount)
                {
                    _configToSave.SuikaIconsPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                var fileName = dialog.FileNames[i];
                if (i < _suikaCount)
                {
                    _configWithFullPathToCopyFilesFrom.SuikaIconsPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                SuikaIconsPaths.Add(new MyPathData($"{SuikaIconsPaths.Count + 1} " + dialog.FileNames[i]));
            }
        }
    }

    private void SelectAudioClicked(object sender, RoutedEventArgs e)
    {
        SelectAudio();
    }

    private void SelectAudio()
    {
        if (SuikaAudioPaths.Count == _suikaCount)
        {
            MessageBox.Show("Max number of audio reached");
            return;
        }

        if (OpenMultipleFilesDialogAudio(out var dialog) == true)
        {
            _configToSave.SuikaAudioPaths = new string[_suikaCount];
            _configWithFullPathToCopyFilesFrom.SuikaAudioPaths = new string[_suikaCount];

            for (var i = 0; i < dialog.SafeFileNames.Length; i++)
            {
                var fileName = dialog.SafeFileNames[i];
                if (i < _suikaCount)
                {
                    _configToSave.SuikaAudioPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                var fileName = dialog.FileNames[i];
                if (i < _suikaCount)
                {
                    _configWithFullPathToCopyFilesFrom.SuikaAudioPaths[i] = fileName;
                }
            }

            for (int i = 0; i < dialog.FileNames.Length; i++)
            {
                SuikaAudioPaths.Add(new MyPathData($"{SuikaAudioPaths.Count + 1} " + dialog.FileNames[i]));
            }
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

    private bool? OpenMultipleFilesDialogImages(out OpenFileDialog imageDialog)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Preview Image",
            DefaultExt = ".jpg",
            Filter = "Image Files(*.jpg;*.jpeg;*.bmp;*.png)|*.jpg;*.jpeg;*.bmp;*.png",
            Multiselect = true
        };

        imageDialog = dialog;

        bool? result = dialog.ShowDialog();

        return result;
    }

    private bool? OpenMultipleFilesDialogAudio(out OpenFileDialog imageDialog)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Audio",
            DefaultExt = ".mp3",
            Filter = "Audio Files(*.mp3)|*.mp3",
            Multiselect = true
        };

        imageDialog = dialog;

        bool? result = dialog.ShowDialog();

        return result;
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
            if (!ShowContinueDialog("Mod icon path is empty"))
            {
                return false;
            }
        }

        if (StringIsInvalid(_configToSave.ContainerImagePath))
        {
            if (!ShowContinueDialog("Container image path is empty"))
            {
                return false;
            }
        }

        if (_configToSave.SuikaSkinsImagesPaths.Length != _suikaCount)
        {
            if (!ShowContinueDialog($"Suika skins images paths are less than {_suikaCount}"))
            {
                return false;
            }
        }

        if (_configToSave.SuikaIconsPaths.Length != _suikaCount)
        {
            if (!ShowContinueDialog($"Suika icons paths are less than {_suikaCount}"))
            {
                return false;
            }
        }

        if (_configToSave.SuikaAudioPaths.Length != _suikaCount)
        {
            if (!ShowContinueDialog($"Suika audio paths are less than {_suikaCount}"))
            {
                return false;
            }
        }

        return true;

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

    private bool ShowContinueDialog(string message)
    {
        return MessageBox.Show(message + "\nDo you want to continue?", "Save error", MessageBoxButton.YesNo,
            MessageBoxImage.Error) == MessageBoxResult.Yes;
    }

    private bool? OpenImageFileDialog(out OpenFileDialog imageDialog)
    {
        var dialog = new OpenFileDialog
        {
            FileName = "Preview Image",
            DefaultExt = ".jpg",
            Filter = "Image Files(*.jpg;*.jpeg;*.bmp;*.png)|*.jpg;*.jpeg;*.bmp;*.png",
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

        MessageBox.Show($"Mod has been created\n {fullSavePath}", "Save success", MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private void CopyFiles(string fullSavePath)
    {
        var fullIconPath = Path.Combine(fullSavePath, _configToSave.ModIconPath);
        File.Copy(_configWithFullPathToCopyFilesFrom.ModIconPath, fullIconPath, true);

        var fullContainerPath = Path.Combine(fullSavePath, _configToSave.ContainerImagePath);
        File.Copy(_configWithFullPathToCopyFilesFrom.ContainerImagePath, fullContainerPath, true);

        for (int i = 0; i < _configToSave.SuikaSkinsImagesPaths.Length; i++)
        {
            if (StringIsInvalid(_configToSave.SuikaSkinsImagesPaths[i]))
                continue;
            
            var fullSkinPath = Path.Combine(fullSavePath, _configToSave.SuikaSkinsImagesPaths[i]);
            File.Copy(_configWithFullPathToCopyFilesFrom.SuikaSkinsImagesPaths[i], fullSkinPath, true);
        }

        for (int i = 0; i < _configToSave.SuikaIconsPaths.Length; i++)
        {
            if (StringIsInvalid(_configToSave.SuikaIconsPaths[i]))
                continue;
            
            var fullSkinPath = Path.Combine(fullSavePath, _configToSave.SuikaIconsPaths[i]);
            File.Copy(_configWithFullPathToCopyFilesFrom.SuikaIconsPaths[i], fullSkinPath, true);
        }

        for (int i = 0; i < _configToSave.SuikaAudioPaths.Length; i++)
        {
            if(StringIsInvalid(_configToSave.SuikaAudioPaths[i]))
                continue;
            
            var fullSkinPath = Path.Combine(fullSavePath, _configToSave.SuikaAudioPaths[i]);
            File.Copy(_configWithFullPathToCopyFilesFrom.SuikaAudioPaths[i], fullSkinPath, true);
        }
    }
}