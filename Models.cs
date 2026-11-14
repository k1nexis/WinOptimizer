using System;
namespace WinOptimizer {
 public sealed class TweakItem { public string Name {get;set;} public string Description {get;set;} public Action Apply {get;set;} public bool IsSelected {get;set;} public override string ToString(){return Localization.T(Name)+" — "+Localization.T(Description);} }
 public sealed class SoftwareItem { public string Name {get;set;} public string Description {get;set;} public string Url {get;set;} public string Arguments {get;set;} public bool IsSelected {get;set;} public string Category {get;set;} public string PackageId {get;set;} public bool SupportsCustomPath {get;set;} public bool IsSupported {get;set;} public string CompatibilityNote {get;set;} public string FileExtension {get;set;} public string ExpectedSha256 {get;set;} public string[] AllowedHostSuffixes {get;set;} public override string ToString(){return Localization.T(Name)+" ("+Localization.T(Description)+")";} }
 public sealed class DriverDownloadResult { public int AvailableCount {get;set;} public int DownloadedCount {get;set;} public string[] Titles {get;set;} public string ResultMessage {get;set;} }
}
