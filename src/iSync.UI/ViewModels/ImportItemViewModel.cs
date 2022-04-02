using System.IO;
using Windows.Media.Import;
using iSync.UI.Helpers;

namespace iSync.UI.ViewModels;

public sealed class ImportItemViewModel : ObservableObject
{
  private readonly PhotoImportItem _importItem;

  public ImportItemViewModel(PhotoImportItem importItem)
  {
    _importItem = importItem;
  }

  public string Name => _importItem.Path;

  public NotifyTaskCompletion<object> Thumbnail => new(LoadThumbnail());

  private async Task<object> LoadThumbnail()
  {
    using var streamWithContentType = await _importItem.Thumbnail.OpenReadAsync();

    await using var stream = streamWithContentType.AsStreamForRead();
    await using var bufferStream = new MemoryStream();

    await stream.CopyToAsync(bufferStream).ConfigureAwait(false);

    await Task.Delay(3000);

    return bufferStream.ToArray();
  }
}