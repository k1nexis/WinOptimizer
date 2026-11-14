using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Win32;

namespace WindowsOptimizerForAll {
 public static class InstalledSoftwareService {
  static readonly string[] UninstallKeys={@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall",@"SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall"};
  static string Normalize(string value){return new string((value??"").Where(Char.IsLetterOrDigit).Select(Char.ToLowerInvariant).ToArray());}
  static string[] Aliases(SoftwareItem item){string id=(item.PackageId??"").ToLowerInvariant();if(id=="winrar")return new[]{"WinRAR"};if(id=="kingsoft.wpsoffice"||item.Name=="WPS Office")return new[]{"WPS Office","Kingsoft Office","Kingsoft WPS"};switch(item.Name){case "Steam":return new[]{"Steam"};case "VLC":return new[]{"VLC media player","VideoLAN VLC"};case "HandBrake":return new[]{"HandBrake"};case "Audacity":return new[]{"Audacity"};case "OBS Studio":return new[]{"OBS Studio"};case "K-Lite Codec Pack Standard":return new[]{"K-Lite Codec Pack"};case "LibreOffice":return new[]{"LibreOffice"};case "SumatraPDF":return new[]{"SumatraPDF"};case "Notepad++":return new[]{"Notepad++"};case "Mullvad Browser":return new[]{"Mullvad Browser"};case "7-Zip":return new[]{"7-Zip"};case "Everything":return new[]{"Everything"};default:return new[]{item.Name};}}
  public static HashSet<string> GetInstalledNames(){var names=new HashSet<string>(StringComparer.OrdinalIgnoreCase);var views=Environment.Is64BitOperatingSystem?new[]{RegistryView.Registry64,RegistryView.Registry32}:new[]{RegistryView.Registry32};foreach(RegistryHive hive in new[]{RegistryHive.LocalMachine,RegistryHive.CurrentUser})foreach(RegistryView view in views){try{using(var root=RegistryKey.OpenBaseKey(hive,view)){foreach(string path in UninstallKeys){try{using(var uninstall=root.OpenSubKey(path,false)){if(uninstall==null)continue;foreach(string subName in uninstall.GetSubKeyNames()){try{using(var app=uninstall.OpenSubKey(subName,false)){string display=app==null?null:Convert.ToString(app.GetValue("DisplayName",null));if(!String.IsNullOrWhiteSpace(display))names.Add(display.Trim());}}catch{}}}}catch{}}}}catch{}}return names;}
  public static bool IsInstalled(SoftwareItem item,IEnumerable<string> displayNames){string[] aliases=Aliases(item).Select(Normalize).Where(x=>x.Length>0).ToArray();return displayNames.Any(display=>{string candidate=Normalize(display);return aliases.Any(alias=>candidate.Contains(alias));});}
 }
}
