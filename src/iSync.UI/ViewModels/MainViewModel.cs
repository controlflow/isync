using System.Collections.ObjectModel;
using System.Windows.Threading;
using Windows.Media.Import;

namespace iSync.UI.ViewModels;

internal class MainViewModel : ObservableObject
{
  private DeviceViewModel? _currentDevice;
  private bool _isImportInProgress;

  public ObservableCollection<DeviceViewModel> Devices { get; } = new();

  public DeviceViewModel? CurrentDevice
  {
    get => _currentDevice;
    set => SetField(ref _currentDevice, value);
  }

  public string DevicesCountMessage
  {
    get
    {
      return Devices.Count switch
      {
        0 => "(No devices available)",
        1 => "(1 device available)",
        var count => $"({count} devices available)"
      };
    }
  }

  public FolderChooserViewModel FolderChooser { get; } = new();


  public bool IsImportInProgress
  {
    get => _isImportInProgress;
    set => SetField(ref _isImportInProgress, value);
  }

  

  public MainViewModel()
  {
    var dispatcherTimer = new DispatcherTimer(DispatcherPriority.Background);
    dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
    dispatcherTimer.IsEnabled = true;
    dispatcherTimer.Tick += DeviceListRefreshTick;
    dispatcherTimer.Start();

    Devices.CollectionChanged += delegate { OnPropertyChanged(nameof(DevicesCountMessage)); };

    InitialDeviceListLoad();
  }

  private async void DeviceListRefreshTick(object? sender, EventArgs args)
  {
    var oldSources = Devices.Select(vm => vm.PhotoImportSource).ToHashSet(PhotoImportSourceComparerById.Instance);
    var newSources = await PhotoImportManager.FindAllSourcesAsync();

    foreach (var newSource in newSources)
    {
      if (!oldSources.Remove(newSource))
      {
        Devices.Add(new DeviceViewModel(newSource));
      }
    }

    if (oldSources.Count > 0)
    {
      for (var index = Devices.Count - 1; index >= 0; index--)
      {
        if (oldSources.Remove(Devices[index].PhotoImportSource))
        {
          Devices.RemoveAt(index);
        }
      }
    }

    if (Devices.Count == 0)
    {
      CurrentDevice = null;
    }
  }

  private async void InitialDeviceListLoad()
  {
    var sources = await PhotoImportManager.FindAllSourcesAsync();

    foreach (var photoImportSource in sources)
    {
      Devices.Add(new DeviceViewModel(photoImportSource));
    }
  }
}