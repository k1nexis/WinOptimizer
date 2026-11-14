using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace WindowsOptimizerForAll
{
 public static class OptionalAppRemovalService
 {
  static readonly HashSet<string> AllowedPackageIds=new HashSet<string>(new[]{"Microsoft.BingNews","Microsoft.BingWeather","Microsoft.MicrosoftSolitaireCollection","Microsoft.GetHelp","Microsoft.Getstarted","Microsoft.WindowsFeedbackHub"},StringComparer.OrdinalIgnoreCase);
  static string RunPowerShell(string script)
  {
   string encoded=Convert.ToBase64String(Encoding.Unicode.GetBytes(script));
   string exe=Path.Combine(Environment.SystemDirectory,"WindowsPowerShell\\v1.0\\powershell.exe");
   if(!File.Exists(exe))throw new FileNotFoundException(Localization.T("Не найдена Windows PowerShell."),exe);
   var start=new ProcessStartInfo(exe,"-NoLogo -NoProfile -NonInteractive -ExecutionPolicy RemoteSigned -EncodedCommand "+encoded){UseShellExecute=false,CreateNoWindow=true,RedirectStandardOutput=true,RedirectStandardError=true,StandardOutputEncoding=Encoding.UTF8,StandardErrorEncoding=Encoding.UTF8};
   using(Process p=Process.Start(start)){string output=p.StandardOutput.ReadToEnd();string error=p.StandardError.ReadToEnd();p.WaitForExit();if(p.ExitCode!=0)throw new InvalidOperationException(String.IsNullOrWhiteSpace(error)?Localization.T("Windows не смогла удалить это приложение."):error.Trim());return output;}
  }

  static string ValidatePackageId(string packageId)
  {
   if(String.IsNullOrWhiteSpace(packageId)||!System.Text.RegularExpressions.Regex.IsMatch(packageId,"^[A-Za-z0-9.]+$")||!AllowedPackageIds.Contains(packageId))throw new ArgumentException(Localization.T("Недопустимое имя пакета приложения."));
   return packageId;
  }

  public static HashSet<string> GetInstalledPackageNames(IEnumerable<string> allowedPackageIds)
  {
   string[] ids=allowedPackageIds.Select(ValidatePackageId).Distinct(StringComparer.OrdinalIgnoreCase).ToArray();
   string list=String.Join(",",ids.Select(x=>"'"+x+"'"));
   string output=RunPowerShell("$ErrorActionPreference='Stop'; $names=@("+list+"); Get-AppxPackage | Where-Object { $names -contains $_.Name } | ForEach-Object { $_.Name }");
   return new HashSet<string>(output.Split(new[]{'\r','\n'},StringSplitOptions.RemoveEmptyEntries).Select(x=>x.Trim()),StringComparer.OrdinalIgnoreCase);
  }

  public static void RemoveForCurrentUser(string packageId)
  {
   string id=ValidatePackageId(packageId);
   string script="$ErrorActionPreference='Stop'; try { $pkg=Get-AppxPackage -Name '"+id+"' | Select-Object -First 1; if ($null -eq $pkg) { throw 'Package is not installed for the current user.' }; Remove-AppxPackage -Package $pkg.PackageFullName -ErrorAction Stop } catch { [Console]::Error.WriteLine($_.Exception.Message); exit 1 }";
   RunPowerShell(script);
  }
 }
}
