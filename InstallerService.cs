using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Threading.Tasks;
namespace WindowsOptimizerForAll
{
 public static class InstallerService
 {
  const long MaxInstallerBytes = 1024L * 1024L * 1024L;
  static readonly Guid GenericVerifyV2 = new Guid("00AAC56B-CD44-11d0-8CC2-00C04FC295EE");

  public static async Task DownloadAndRunAsync(SoftwareItem item, string destination, Action<long, long> progress)
  {
   Uri initialUri;
   if (item == null || !Uri.TryCreate(item.Url, UriKind.Absolute, out initialUri) || !String.Equals(initialUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) || !String.IsNullOrEmpty(initialUri.UserInfo))
    throw new InvalidOperationException(Localization.T("Для программы не задан безопасный HTTPS-источник."));
   if (!IsAllowedHost(initialUri.Host, item.AllowedHostSuffixes))
    throw new InvalidOperationException(Localization.T("Скачивание остановлено: конечный сервер не входит в список разрешённых издателей."));
   ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
   string extension = String.IsNullOrWhiteSpace(item.FileExtension) ? ".exe" : item.FileExtension;
   if ((extension != ".exe" && extension != ".msi") || Path.GetFileName(extension) != extension)
    throw new InvalidOperationException(Localization.T("Разрешены только установщики EXE и MSI."));
   string temp = Path.Combine(Path.GetTempPath(), "WindowsOptimizerForAll_" + Guid.NewGuid().ToString("N") + extension);
   try
   {
    using (var handler = new HttpClientHandler())
    {
     handler.AllowAutoRedirect = false;
     using (var client = new HttpClient(handler))
     {
      client.Timeout = TimeSpan.FromMinutes(30);
      Uri requestUri = initialUri;
      HttpResponseMessage downloadedResponse = null;
      for (int redirects = 0; redirects <= 5; redirects++)
      {
       if (!String.Equals(requestUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase) || !String.IsNullOrEmpty(requestUri.UserInfo))
        throw new InvalidOperationException(Localization.T("Скачивание остановлено: издатель перенаправил запрос на небезопасный адрес."));
       if (!IsAllowedHost(requestUri.Host, item.AllowedHostSuffixes) && String.IsNullOrWhiteSpace(item.ExpectedSha256))
        throw new InvalidOperationException(Localization.T("Скачивание остановлено: конечный сервер не входит в список разрешённых издателей."));
       downloadedResponse = await client.GetAsync(requestUri, HttpCompletionOption.ResponseHeadersRead);
       int status = (int)downloadedResponse.StatusCode;
       if (status < 300 || status >= 400) break;
       Uri location = null;
       bool hasLocation = downloadedResponse.Headers.Location != null && Uri.TryCreate(requestUri, downloadedResponse.Headers.Location, out location);
       downloadedResponse.Dispose();
       if (!hasLocation || redirects == 5)
        throw new InvalidOperationException(Localization.T("Скачивание остановлено: превышено допустимое число переадресаций."));
       requestUri = location;
      }
      using (HttpResponseMessage response = downloadedResponse)
      {
       response.EnsureSuccessStatusCode();
       Uri finalUri = response.RequestMessage == null ? null : response.RequestMessage.RequestUri;
       if (finalUri == null || !String.Equals(finalUri.Scheme, Uri.UriSchemeHttps, StringComparison.OrdinalIgnoreCase))
        throw new InvalidOperationException(Localization.T("Скачивание остановлено: издатель перенаправил запрос на небезопасный адрес."));
       if (!IsAllowedHost(finalUri.Host, item.AllowedHostSuffixes) && String.IsNullOrWhiteSpace(item.ExpectedSha256))
        throw new InvalidOperationException(Localization.T("Скачивание остановлено: конечный сервер не входит в список разрешённых издателей."));
       long total = response.Content.Headers.ContentLength ?? -1;
       if (total > MaxInstallerBytes) throw new InvalidOperationException(Localization.T("Размер установщика превышает ограничение 1 ГБ."));
       using (Stream input = await response.Content.ReadAsStreamAsync())
       using (FileStream output = new FileStream(temp, FileMode.CreateNew, FileAccess.Write, FileShare.None))
       {
        byte[] buffer = new byte[65536]; long done = 0; int count;
        while ((count = await input.ReadAsync(buffer, 0, buffer.Length)) > 0)
        {
         if (done + count > MaxInstallerBytes) throw new InvalidOperationException(Localization.T("Загрузка остановлена: файл превышает ограничение 1 ГБ."));
         await output.WriteAsync(buffer, 0, count); done += count;
         if (progress != null) progress(done, total);
        }
       }
      }
     }
    }
    if (!String.IsNullOrWhiteSpace(item.ExpectedSha256)) VerifySha256(temp, item.ExpectedSha256);
    else VerifyAuthenticode(temp);
    string installDir = Path.Combine(destination, item.Name.Replace(" ", ""));
    if (item.SupportsCustomPath) Directory.CreateDirectory(installDir);
    string args = (item.Arguments ?? "");
    if (item.SupportsCustomPath) args = args.Replace("{INSTALLDIR}", installDir);
    bool isMsi = String.Equals(extension, ".msi", StringComparison.OrdinalIgnoreCase);
    string msiExec = Path.Combine(Environment.SystemDirectory, "msiexec.exe");
    ProcessStartInfo psi = isMsi
     ? new ProcessStartInfo(msiExec, "/i \"" + temp + "\" " + args)
     : new ProcessStartInfo(temp, args);
    psi.UseShellExecute = !isMsi;
    using (Process p = Process.Start(psi))
    {
     await Task.Run(() => p.WaitForExit());
     if (p.ExitCode != 0 && p.ExitCode != 3010 && p.ExitCode != 1641) throw new Exception(Localization.T("Установщик завершился с кодом ") + p.ExitCode + Localization.T(". Проверьте совместимость параметров установки."));
    }
   }
   finally { try { if (File.Exists(temp)) File.Delete(temp); } catch { } }
  }

  static bool IsAllowedHost(string host, string[] suffixes)
  {
   if (suffixes == null || suffixes.Length == 0) return false;
   foreach (string value in suffixes)
   {
    string suffix = (value ?? "").Trim().TrimStart('.').ToLowerInvariant();
    string candidate = (host ?? "").ToLowerInvariant();
    if (suffix.Length > 0 && (candidate == suffix || candidate.EndsWith("." + suffix, StringComparison.Ordinal))) return true;
   }
   return false;
  }

  static void VerifySha256(string filePath, string expected)
  {
   using (SHA256 sha = SHA256.Create())
   using (FileStream stream = File.OpenRead(filePath))
   {
    byte[] bytes = sha.ComputeHash(stream);
    string actual = BitConverter.ToString(bytes).Replace("-", "").ToLowerInvariant();
    if (!String.Equals(actual, expected.Replace(" ", "").ToLowerInvariant(), StringComparison.Ordinal))
     throw new InvalidOperationException(Localization.T("Контрольная сумма установщика не совпала с опубликованной издателем. Файл не будет запущен."));
   }
  }

  static void VerifyAuthenticode(string filePath)
  {
   IntPtr pathPointer = Marshal.StringToCoTaskMemUni(filePath);
   IntPtr fileInfoPointer = IntPtr.Zero;
   try
   {
    WinTrustFileInfo fileInfo = new WinTrustFileInfo();
    fileInfo.cbStruct = (uint)Marshal.SizeOf(typeof(WinTrustFileInfo));
    fileInfo.filePath = pathPointer;
    fileInfoPointer = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(WinTrustFileInfo)));
    Marshal.StructureToPtr(fileInfo, fileInfoPointer, false);

    WinTrustData data = new WinTrustData();
    data.cbStruct = (uint)Marshal.SizeOf(typeof(WinTrustData));
    data.uiChoice = 2; // WTD_UI_NONE
    data.unionChoice = 1; // WTD_CHOICE_FILE
    data.fileInfo = fileInfoPointer;
    data.providerFlags = 0x20; // WTD_REVOCATION_CHECK_END_CERT
    data.stateAction = 1; // WTD_STATEACTION_VERIFY
    int result = WinVerifyTrust(IntPtr.Zero, GenericVerifyV2, ref data);
    data.stateAction = 2; // WTD_STATEACTION_CLOSE
    WinVerifyTrust(IntPtr.Zero, GenericVerifyV2, ref data);
    if (result != 0)
     throw new InvalidOperationException(Localization.T("Цифровая подпись установщика отсутствует или не прошла проверку доверия (0x") + result.ToString("X8") + Localization.T("). Файл не будет запущен."));
   }
   finally
   {
    if (fileInfoPointer != IntPtr.Zero) Marshal.FreeHGlobal(fileInfoPointer);
    if (pathPointer != IntPtr.Zero) Marshal.FreeCoTaskMem(pathPointer);
   }
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  struct WinTrustFileInfo
  {
   public uint cbStruct;
   public IntPtr filePath;
   public IntPtr fileHandle;
   public IntPtr knownSubject;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  struct WinTrustData
  {
   public uint cbStruct;
   public IntPtr policyCallbackData;
   public IntPtr sipClientData;
   public uint uiChoice;
   public uint revocationChecks;
   public uint unionChoice;
   public IntPtr fileInfo;
   public uint stateAction;
   public IntPtr stateData;
   public IntPtr urlReference;
   public uint providerFlags;
   public uint uiContext;
   public IntPtr signatureSettings;
  }

  [DllImport("wintrust.dll", ExactSpelling = true, PreserveSig = true)]
  static extern int WinVerifyTrust(IntPtr hwnd, [MarshalAs(UnmanagedType.LPStruct)] Guid actionId, ref WinTrustData data);
 }
}

