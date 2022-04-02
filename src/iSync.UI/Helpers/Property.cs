using System.ComponentModel;
using JetBrains.Annotations;

namespace iSync.UI.Helpers;

public sealed class Property<T> : INotifyPropertyChanged
{
  // ReSharper disable once StaticMemberInGenericType
  private static readonly PropertyChangedEventArgs Args = new(nameof(Value));

  public Property()
  {
    _value = default!;
  }

  public Property(T value)
  {
    _value = value;
  }

  private T _value;
  // ReSharper disable once InconsistentNaming
  private event PropertyChangedEventHandler? _changed;

  public T Value
  {
    get => _value;
    set
    {
      if (!EqualityComparer<T>.Default.Equals(_value, value))
      {
        _value = value;
        _changed?.Invoke(this, Args);
      }
    }
  }

  event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
  {
    add => _changed += value;
    remove => _changed -= value;
  }

  public void Advice(Action<T> onChanged)
  {
    _changed += delegate
    {
      onChanged(_value);
    };
  }

  [MustUseReturnValue]
  public Property<TResult> Select<TResult>(Func<T, TResult> selector)
  {
    ArgumentNullException.ThrowIfNull(selector);

    var property = new Property<TResult>();

    _changed += delegate
    {
      property.Value = selector(Value);
    };

    return property;
  }
}