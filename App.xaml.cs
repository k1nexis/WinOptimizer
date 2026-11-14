using System.Windows;

namespace WindowsOptimizerForAll
{
 public partial class App : Application
 {
  protected override void OnStartup(StartupEventArgs e)
  {
   base.OnStartup(e);
   ShutdownMode = ShutdownMode.OnExplicitShutdown;
   var picker = new LanguagePickerWindow();
   if (picker.ShowDialog() != true) { Shutdown(); return; }
   Localization.SetLanguage(picker.SelectedLanguage);
   var main = new MainWindow();
   MainWindow = main;
   ShutdownMode = ShutdownMode.OnMainWindowClose;
   main.Show();
  }
 }
}
