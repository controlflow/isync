using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace iSync.UI.ViewModels;

public abstract class ObservableObject : INotifyPropertyChanged
{
  public event PropertyChangedEventHandler? PropertyChanged;

  [NotifyPropertyChangedInvocator]
  protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
  {
    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
  }

  [NotifyPropertyChangedInvocator]
  protected bool SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
  {
    if (!EqualityComparer<T>.Default.Equals(field, value))
    {
      field = value;
      OnPropertyChanged(propertyName);
      return true;
    }

    return false;
  }
}