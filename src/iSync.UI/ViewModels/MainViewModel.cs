namespace iSync.UI.ViewModels;

public sealed class MainViewModel : ObservableObject
{
  private ImportSessionViewModel? _importSession;

  public SourceListViewModel SourceList { get; } = new();
  public FolderChooserViewModel FolderChooser { get; } = new();

  public ImportSessionViewModel? ImportSession
  {
    get => _importSession;
    set => SetField(ref _importSession, value);
  }

  public MainViewModel()
  {
    SourceList.CurrentDevice.Advice(deviceViewModel =>
    {
      ImportSession?.Dispose();

      if (deviceViewModel == null)
      {
        ImportSession = null;
      }
      else
      {
        var importSession = deviceViewModel.PhotoImportSource.CreateImportSession();
        ImportSession = new ImportSessionViewModel(importSession);
      }
    });
  }
}

