/*

using Windows.Media.Import;
using Windows.Storage;

var sources = await PhotoImportManager.FindAllSourcesAsync();

foreach (var importSource in sources)
{
  var displayName = importSource.DisplayName;
  if (displayName == "Apple iPhone")
  {
    using var importSession = importSource.CreateImportSession();

    importSession.SubfolderCreationMode = PhotoImportSubfolderCreationMode.KeepOriginalFolderStructure;

    importSession.DestinationFolder = await StorageFolder.GetFolderFromPathAsync("C:\\Work\\iSync\\out");

    var importFindItemsResult = await importSession.FindItemsAsync(PhotoImportContentTypeFilter.ImagesAndVideos, PhotoImportItemSelectionMode.SelectAll);
    if (importFindItemsResult.HasSucceeded)
    {
      var totalBytes = importFindItemsResult.SelectedTotalSizeInBytes;

      // foreach (var importItem in importFindItemsResult.FoundItems)
      // {
      //   Console.WriteLine($"{importItem.Name} {importItem.ContentType} {importItem.Path}");
      // }

      int index = 1;
      var total = importFindItemsResult.TotalCount;

      foreach (var importItem in importFindItemsResult.FoundItems)
      {
        //using var randomAccessStreamWithContentType = await importItem.Thumbnail.OpenReadAsync();
        //var contentType = randomAccessStreamWithContentType.ContentType;
      }

      importFindItemsResult.ItemImported +=
        delegate(PhotoImportFindItemsResult _, PhotoImportItemImportedEventArgs args1)
        {
          Console.WriteLine($"{index:D4}/{total:D4}: {args1.ImportedItem.Name} imported");


          index++;
        };

      await importFindItemsResult.ImportItemsAsync();


    }
  }
}

return 0;

*/