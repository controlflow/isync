using System.IO;
using System.Text;
using Windows.Media.Import;
using Windows.Storage;
using iSync.UI.Helpers;

namespace iSync.UI.ViewModels;

public class DeviceViewModel : ObservableObject, IEquatable<DeviceViewModel>
{
  private readonly PhotoImportSource _photoImportSource;

  public DeviceViewModel(PhotoImportSource photoImportSource)
  {
    _photoImportSource = photoImportSource;

    Image = new NotifyTaskCompletion<object>(LoadThumbnail())
    {
      NotCompletedValue = "Resources/icon.png",
      FailedValue = "Resources/icon.png"
    };
  }

  public PhotoImportSource PhotoImportSource => _photoImportSource;
  public string DisplayName => _photoImportSource.DisplayName;

  public NotifyTaskCompletion<object> Image { get; }

  private async Task<object> LoadThumbnail()
  {
    using var streamWithContentType = await _photoImportSource.Thumbnail.OpenReadAsync();

    await using var stream = streamWithContentType.AsStreamForRead();
    await using var bufferStream = new MemoryStream();

    await stream.CopyToAsync(bufferStream).ConfigureAwait(false);

    await Task.Delay(3000);

    return bufferStream.ToArray();
  }

  public string Tip
  {
    get
    {
      var builder = new StringBuilder();
      builder.AppendLine($"Name: {_photoImportSource.DisplayName}");
      builder.AppendLine($"Manufacturer: {_photoImportSource.Manufacturer}");
      builder.AppendLine($"Model: {_photoImportSource.Model}");
      builder.AppendLine($"Description: {_photoImportSource.Description}");

      var batteryLevelPercent = _photoImportSource.BatteryLevelPercent;
      if (batteryLevelPercent != null) builder.AppendLine($"Battery: {batteryLevelPercent}%");

      var isLocked = _photoImportSource.IsLocked;
      if (isLocked != null) builder.AppendLine($"IsLocked: {isLocked}%");

      builder.AppendLine($"Type: {_photoImportSource.Type}");
      builder.AppendLine($"Serial number: {_photoImportSource.SerialNumber}");
      builder.AppendLine($"Protocol: {_photoImportSource.ConnectionProtocol}");
      builder.Append($"Transport: {_photoImportSource.ConnectionTransport}");

      return builder.ToString();
    }
  }

  public NotifyTaskCompletion<object> Contents => new(LoadContents())
  {
    NotCompletedValue = "Loading...",
    FailedValue = "Failed"
  };

  private async Task<object> LoadContents()
  {
    using var importSession = _photoImportSource.CreateImportSession();

    importSession.SubfolderCreationMode = PhotoImportSubfolderCreationMode.KeepOriginalFolderStructure;
    importSession.DestinationFolder = await StorageFolder.GetFolderFromPathAsync("C:\\Work\\iSync");

    var itemsResult = await importSession.FindItemsAsync(PhotoImportContentTypeFilter.ImagesAndVideos, PhotoImportItemSelectionMode.SelectAll);

    var list = new List<ImportItemViewModel>();

    foreach (var foundItem in itemsResult.FoundItems)
    {
      list.Add(new ImportItemViewModel(foundItem));
    }

    return list;
  }

  public override bool Equals(object? obj)
  {
    return obj is DeviceViewModel other && Equals(other);
  }

  public bool Equals(DeviceViewModel? other)
  {
    if (ReferenceEquals(null, other)) return false;
    if (ReferenceEquals(this, other)) return true;
    return _photoImportSource.Equals(other._photoImportSource);
  }

  public override int GetHashCode()
  {
    return _photoImportSource.GetHashCode();
  }
}