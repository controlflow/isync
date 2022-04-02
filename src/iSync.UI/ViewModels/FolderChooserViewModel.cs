using System.Globalization;
using System.IO;
using System.Windows.Input;
using Humanizer;
using iSync.UI.Helpers;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace iSync.UI.ViewModels;

public sealed class FolderChooserViewModel : ObservableObject
{
  private readonly FileSystemWatcher _fileSystemWatcher = new() { EnableRaisingEvents = false };

  public FolderChooserViewModel()
  {
    try
    {
      FolderPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    }
    catch
    {
      // ignored
    }

    //_fileSystemWatcher.
    //_fileSystemWatcher.Path = "";
  }

  private string _folderPath = "";

  public string FolderPath
  {
    get => _folderPath;
    set
    {
      if (SetField(ref _folderPath, value))
      {
        OnPropertyChanged(nameof(FolderHint));
      }
    }
  }

  public string FolderHint
  {
    get
    {
      try
      {
        if (!Directory.Exists(FolderPath))
        {
          return "Destination path do not exists";
        }

        var pathRoot = Path.GetPathRoot(FolderPath);
        if (pathRoot != null)
        {
          var driveInfo = new DriveInfo(pathRoot);
          return $"Available disk space: {driveInfo.AvailableFreeSpace.Bytes().ToString(CultureInfo.InvariantCulture)}";
        }
      }
      catch
      {
        return "Error accessing the destination folder";
      }

      return "";
    }
  }

  public ICommand BrowseCommand => new DelegateCommand(() =>
  {
    using var dialog = new CommonOpenFileDialog();

    dialog.IsFolderPicker = true;
    dialog.EnsurePathExists = true;
    dialog.Title = "Choose a destination folder";

    var result = dialog.ShowDialog();
    if (result == CommonFileDialogResult.Ok)
    {
      FolderPath = dialog.FileName;
    }
  });
}