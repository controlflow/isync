using iSync.UI.ViewModels;

namespace iSync.UI.Helpers;

public sealed class NotifyTaskCompletion<T> : ObservableObject
{
  private readonly Task<T> _task;

  public NotifyTaskCompletion(Task<T> task)
  {
    _task = task;
    _task.ContinueWith(t =>
    {
      if (t.IsCanceled || t.IsFaulted)
      {
        OnPropertyChanged(nameof(Exception));
      }

      OnPropertyChanged(nameof(Result));
    }, TaskScheduler.FromCurrentSynchronizationContext());
  }

  public T? NotCompletedValue { get; init; }
  public T? FailedValue { get; init; }

  public Exception? Exception => _task.Exception;

  public T? Result
  {
    get
    {
      if (_task.IsCompletedSuccessfully)
        return _task.Result;

      if (_task.IsFaulted || _task.IsCanceled)
        return FailedValue;

      return NotCompletedValue;
    }
  }
}