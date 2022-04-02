using Windows.Media.Import;

namespace iSync.UI.ViewModels;

public sealed class PhotoImportSourceComparerById : IEqualityComparer<PhotoImportSource>
{
  private PhotoImportSourceComparerById() { }

  public static IEqualityComparer<PhotoImportSource> Instance { get; } = new PhotoImportSourceComparerById();

  public bool Equals(PhotoImportSource? x, PhotoImportSource? y)
  {
    if (ReferenceEquals(x, y)) return true;
    if (ReferenceEquals(x, null)) return false;
    if (ReferenceEquals(y, null)) return false;
    if (x.GetType() != y.GetType()) return false;

    return x.Id == y.Id;
  }

  public int GetHashCode(PhotoImportSource obj)
  {
    return obj.Id.GetHashCode();
  }
}