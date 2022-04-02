using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;

namespace iSync.UI.Helpers;

[ContentProperty(nameof(Templates))]
public class TemplateByTypeSelector : DataTemplateSelector
{
  public List<DataTemplate> Templates { get; } = new();
  public DataTemplate? TemplateForNullItem { get; set; }

  public override DataTemplate SelectTemplate(object? item, DependencyObject container)
  {
    if (item == null)
      return TemplateForNullItem ?? throw new Exception("TemplateForNullItem is not specified");

    var itemType = item.GetType();

    var possibleTemplates = Templates.Where(mapping => mapping.DataType is Type type && type.IsAssignableFrom(itemType)).ToList();
    if (possibleTemplates.Count > 1)
    {
      var typeNames = string.Join(", ", possibleTemplates.Select(t => t.DataType));
      var errorMessage = string.Format("Item '{0}' can be mapped to multiple types, please specify type TValue mapping more distinctly: " + typeNames, itemType.Name);
      throw new InvalidOperationException(errorMessage);
    }

    if (possibleTemplates.Count == 0)
      throw new InvalidOperationException("No type mapping found for: " + itemType.Name);

    return possibleTemplates[0];
  }
}