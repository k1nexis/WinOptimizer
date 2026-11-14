using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WindowsOptimizerForAll {
 public static class WingetService {
  static string FindWinget(){string local=Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);string alias=Path.Combine(local,"Microsoft","WindowsApps","winget.exe");if(File.Exists(alias))return alias;foreach(string dir in (Environment.GetEnvironmentVariable("PATH")??"").Split(Path.PathSeparator)){try{string candidate=Path.Combine(dir.Trim('"'),"winget.exe");if(File.Exists(candidate))return candidate;}catch{}}return null;}
  public static async Task InstallAsync(string packageId){if(String.IsNullOrWhiteSpace(packageId)||!Regex.IsMatch(packageId,"^[A-Za-z0-9][A-Za-z0-9._-]{0,100}$"))throw new ArgumentException("Invalid WinGet package identifier.");string exe=FindWinget();if(exe==null)throw new InvalidOperationException(Localization.T("Windows Package Manager (WinGet) не найден. Установите приложение App Installer из Microsoft Store и повторите попытку."));var start=new ProcessStartInfo(exe,"install --exact --id \""+packageId+"\" --silent --accept-package-agreements --accept-source-agreements --disable-interactivity"){UseShellExecute=false,CreateNoWindow=true,RedirectStandardOutput=true,RedirectStandardError=true};using(Process process=Process.Start(start)){Task<string> stdout=process.StandardOutput.ReadToEndAsync();Task<string> stderr=process.StandardError.ReadToEndAsync();await Task.Run(()=>process.WaitForExit());string output=(await stdout)+" "+(await stderr);if(process.ExitCode!=0)throw new InvalidOperationException(Localization.T("WinGet не смог установить программу (код ")+process.ExitCode+"). "+output.Trim());}}
 }
}
