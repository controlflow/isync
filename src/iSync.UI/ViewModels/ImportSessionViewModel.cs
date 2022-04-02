using System.IO;
using Windows.Media.Import;
using Windows.Storage;
using iSync.UI.Helpers;

namespace iSync.UI.ViewModels;

public sealed class ImportSessionViewModel : ObservableObject, IDisposable
{
  private readonly PhotoImportSession _importSession;

  public ImportSessionViewModel(PhotoImportSession importSession)
  {
    _importSession = importSession;
    //Contents = new List<string>() { "aaa", "bbb", "ccc" };
  }

  public NotifyTaskCompletion<object> Contents => new(LoadContents())
  {
    NotCompletedValue = "Loading...",
    FailedValue = "Failed"
  };

  private async Task<object> LoadContents()
  {
    _importSession.SubfolderCreationMode = PhotoImportSubfolderCreationMode.KeepOriginalFolderStructure;
    _importSession.DestinationFolder = await StorageFolder.GetFolderFromPathAsync("C:\\Work\\iSync");

    var itemsResult = await _importSession.FindItemsAsync(
      PhotoImportContentTypeFilter.ImagesAndVideos, PhotoImportItemSelectionMode.SelectAll);

    var groupsList = new List<ImportGroupViewModel>();

    foreach (var importGroup in itemsResult.FoundItems.GroupBy(GetImportItemDirectory))
    {
      var itemList = new List<ImportItemViewModel>();
      foreach (var importItem in importGroup)
      {
        itemList.Add(new ImportItemViewModel(importItem));
      }

      groupsList.Add(new ImportGroupViewModel(importGroup.Key, itemList));
    }

    return groupsList;
  }

  private string GetImportItemDirectory(PhotoImportItem importItem)
  {
    return importItem.Path;

    try
    {
      var directoryName = Path.GetDirectoryName(importItem.Path);
      if (!string.IsNullOrWhiteSpace(directoryName))
      {
        return directoryName;
      }
    }
    catch
    {
      // ignored
    }

    return "<No directory>";
  }

  public void Dispose()
  {
    _importSession.Dispose();
  }
}