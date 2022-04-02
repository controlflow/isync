namespace iSync.UI.ViewModels;

public class NotSelectedDeviceViewModel
{
  private readonly int _count;

  public NotSelectedDeviceViewModel(int count)
  {
    _count = count;
  }

  public string Message => _count == 0 ? "<No devices available>" : $"<{_count} device(s) available>";
}