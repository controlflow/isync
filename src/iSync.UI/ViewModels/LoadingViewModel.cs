using System.Windows.Threading;

namespace iSync.UI.ViewModels;

public sealed class LoadingViewModel : ObservableObject, IDisposable
{
  private readonly DispatcherTimer _timer;
  private int _dotsCount = 2;

  public string Message => "Loading" + new string('.', _dotsCount + 1);

  public LoadingViewModel()
  {
    _timer = new DispatcherTimer();
    _timer.Interval = TimeSpan.FromMilliseconds(500);
    _timer.Tick += delegate
    {
      _dotsCount = (_dotsCount + 1) % 5;
      OnPropertyChanged(nameof(Message));
    };
    _timer.Start();
  }

  public void Dispose()
  {
    _timer.Stop();
  }
}