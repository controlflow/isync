using System.Windows.Input;

namespace iSync.UI.Helpers;

public sealed class DelegateCommand : ICommand
{
  private readonly object? _execute;
  private bool _isEnabled = true;

  public DelegateCommand() { }

  public DelegateCommand(Action execute)
  {
    _execute = execute;
  }

  public DelegateCommand(Action<object> execute)
  {
    _execute = execute;
  }

  public bool IsEnabled
  {
    get => _isEnabled;
    set
    {
      if (value != _isEnabled)
      {
        _isEnabled = value;
        CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      }
    }
  }

  bool ICommand.CanExecute(object? parameter) => IsEnabled;

  public void Execute(object? parameter)
  {
    if (_execute is Action<object?> withParameter)
    {
      withParameter(parameter);
    }
    else if (_execute is Action execute)
    {
      execute();
    }
  }

  public event EventHandler? CanExecuteChanged;
}