using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using iSync.UI.ViewModels;

namespace iSync.UI;

public partial class MainWindow
{
  public MainWindow()
  {
    InitializeComponent();
    DataContext = new MainViewModel();
  }

  private void FolderPathGotFocus(object sender, RoutedEventArgs e)
  {
    ((TextBox)sender).SelectAll();
  }

  private void FolderPathMouseUp(object sender, MouseButtonEventArgs e)
  {
    ((TextBox)sender).SelectAll();
    e.Handled = true;
  }
}