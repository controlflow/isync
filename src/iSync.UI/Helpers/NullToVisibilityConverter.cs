using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace iSync.UI.Helpers;

public class NullToVisibilityConverter : IValueConverter
{
  public object Convert(object? value, Type targetType, object parameter, CultureInfo language)
  {
    return value is null ? Visibility.Visible : Visibility.Collapsed;
  }

  public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
  {
    throw new InvalidOperationException();
  }
}