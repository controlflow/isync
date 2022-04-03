using System.Globalization;
using Humanizer;

namespace iSync.UI.ViewModels;

public sealed class ImportGroupViewModel : ObservableObject
{
  private readonly string _groupName;

  public ImportGroupViewModel(string groupName, List<ImportItemViewModel> items)
  {
    _groupName = groupName;
    Items = items;
  }

  public string GroupName => _groupName;
  public string Size => Items.Sum(x => (double) x.ImportItem.SizeInBytes).Bytes().ToString(CultureInfo.InvariantCulture);

  public string Count => Items.Count + " item(s)";

  public bool IsVisible { get; set; }

  public IReadOnlyList<ImportItemViewModel> Items { get; }

  public IReadOnlyList<ImportItemViewModel> Items2 => Items.Take(6).ToList();
}