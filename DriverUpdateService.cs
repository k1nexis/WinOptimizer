using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
namespace WinOptimizer
{
 public static class DriverUpdateService
 {
  public static Task<DriverDownloadResult> SearchAndDownloadAsync(Action<int, string> report)
  {
   var completion = new TaskCompletionSource<DriverDownloadResult>();
   Thread worker = new Thread(() =>
   {
    object sessionObject = null, searcherObject = null, searchResultObject = null;
    object updatesObject = null, collectionObject = null, downloaderObject = null, downloadResultObject = null;
    try
    {
     if (report != null) report(5, Localization.T("Поиск доступных драйверов через Windows Update…"));
     Type sessionType = Type.GetTypeFromProgID("Microsoft.Update.Session");
     if (sessionType == null) throw new InvalidOperationException(Localization.T("Компонент Windows Update Agent недоступен."));
     sessionObject = Activator.CreateInstance(sessionType);
     dynamic session = sessionObject;
     searcherObject = session.CreateUpdateSearcher();
     dynamic searcher = searcherObject;
     searchResultObject = searcher.Search("IsInstalled=0 and Type='Driver' and IsHidden=0 and IsAssigned=1");
     dynamic searchResult = searchResultObject;
     updatesObject = searchResult.Updates;
     dynamic updates = updatesObject;
     int count = Convert.ToInt32(updates.Count);
     if (count == 0)
     {
      completion.SetResult(new DriverDownloadResult { AvailableCount = 0, DownloadedCount = 0, Titles = new string[0], ResultMessage = Localization.T("Windows Update не нашёл доступных драйверов для этого устройства.") });
      return;
     }
     var titles = new List<string>();
     collectionObject = Activator.CreateInstance(Type.GetTypeFromProgID("Microsoft.Update.UpdateColl"));
     dynamic collection = collectionObject;
     for (int i = 0; i < count; i++)
     {
      dynamic update = updates.Item(i);
      titles.Add(Convert.ToString(update.Title));
      collection.Add(update);
      Release(update);
     }
     if (report != null) report(35, Localization.T("Найдено пакетов: ") + count + Localization.T(". Начинаю загрузку…"));
     downloaderObject = session.CreateUpdateDownloader();
     dynamic downloader = downloaderObject;
     downloader.Updates = collectionObject;
     downloadResultObject = downloader.Download();
     dynamic downloadResult = downloadResultObject;
     int resultCode = Convert.ToInt32(downloadResult.ResultCode);
     if (resultCode != 2 && resultCode != 3)
      throw new InvalidOperationException(Localization.T("Windows Update не завершил загрузку драйверов (код ") + resultCode + Localization.T(". Проверьте службу Windows Update и повторите попытку."));
     int downloadedCount = 0;
     for (int i = 0; i < count; i++)
     {
      dynamic update = updates.Item(i);
      if (Convert.ToBoolean(update.IsDownloaded)) downloadedCount++;
      Release(update);
     }
     completion.SetResult(new DriverDownloadResult
     {
      AvailableCount = count,
      DownloadedCount = downloadedCount,
      Titles = titles.ToArray(),
      ResultMessage = Localization.T(resultCode == 2 ? "Загрузка драйверов завершена." : "Загрузка завершена частично. Некоторые пакеты могли не скачаться.")
     });
    }
    catch (Exception ex) { completion.SetException(ex); }
    finally
    {
     Release(downloadResultObject); Release(downloaderObject); Release(collectionObject);
     Release(updatesObject); Release(searchResultObject); Release(searcherObject); Release(sessionObject);
    }
   });
   worker.IsBackground = true;
   worker.SetApartmentState(ApartmentState.STA);
   worker.Start();
   return completion.Task;
  }

  static void Release(object value)
  {
   if (value != null && Marshal.IsComObject(value))
   {
    try { Marshal.FinalReleaseComObject(value); } catch { }
   }
  }
 }
}
