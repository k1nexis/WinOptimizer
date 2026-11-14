using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WinOptimizer
{
 public sealed class LanguagePickerWindow : Window
 {
  readonly ComboBox languageBox;
  readonly Button continueButton;
  public string SelectedLanguage { get { return languageBox.SelectedIndex == 1 ? "ru" : "en"; } }

  public LanguagePickerWindow()
  {
   Title = "Choose language / Выберите язык";
   Width = 390;
   Height = 210;
   ResizeMode = ResizeMode.NoResize;
   WindowStartupLocation = WindowStartupLocation.CenterScreen;
   Background = new SolidColorBrush(Color.FromRgb(16, 19, 24));
   Foreground = new SolidColorBrush(Color.FromRgb(230, 234, 240));

   var layout = new StackPanel { Margin = new Thickness(24) };
   layout.Children.Add(new TextBlock { Text = "Choose language / Выберите язык", FontSize = 18, FontWeight = FontWeights.SemiBold, Margin = new Thickness(0, 0, 0, 12) });
   languageBox = new ComboBox { Margin = new Thickness(0, 0, 0, 16), Padding = new Thickness(8, 6, 8, 6) };
   languageBox.Items.Add("English");
   languageBox.Items.Add("Русский");
   languageBox.SelectedIndex = 0;
   layout.Children.Add(languageBox);
   continueButton = new Button { Content = "Continue", HorizontalAlignment = HorizontalAlignment.Right, MinWidth = 120 };
   continueButton.Click += (sender, args) => { DialogResult = true; };
   languageBox.SelectionChanged += (sender, args) => continueButton.Content = languageBox.SelectedIndex == 1 ? "Продолжить" : "Continue";
   layout.Children.Add(continueButton);
   Content = layout;
  }
 }
}
