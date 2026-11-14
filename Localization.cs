using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace WinOptimizer
{
 public static class Localization
 {
  public static string Language { get; private set; } = "en";
  static readonly Dictionary<string, string> English = new Dictionary<string, string>
  {
   {"Выберите версию Windows", "Choose a Windows version"}, {"Автоопределение", "Detect automatically"}, {"Определить автоматически", "Detect Windows automatically"},
   {"Профили оптимизации", "Optimization profiles"}, {"Установка программ", "Install software"}, {"Полная приватность", "Privacy"}, {"Монтаж и медиа", "Media and editing"}, {"Утилиты", "Utilities"},
   {"Игры", "Gaming"}, {"Старый ПК и ноутбук", "Older PC and laptop"}, {"Монтаж и рендеринг", "Video editing and rendering"}, {"Работа и учёба", "Work and study"},
   {"Перед системными изменениями создайте точку восстановления.", "Create a restore point before changing system settings."}, {"Создать точку восстановления", "Create restore point"}, {"Применить отмеченное", "Apply selected changes"}, {"Выберите профиль", "Choose a profile"},
   {"Папка установки:", "Install folder:"}, {"Обзор…", "Browse…"}, {"Скачать доступные драйверы", "Download available drivers"}, {"Видеокарта: определение…", "Graphics card: detecting…"},
   {"Профиль оптимизации автоматически отмечает подходящие программы; отметки можно изменить вручную.", "The selected profile checks recommended software. You can change the selection."},
   {"Драйверы ищутся через настроенный Windows Update и скачиваются, но не устанавливаются автоматически. В VM это будут драйверы виртуальных устройств.", "Drivers are searched for through the configured Windows Update source and downloaded, but not installed. In a VM, these are drivers for virtual devices."},
   {"Установить выбранное", "Install selected"}, {"Готово", "Ready"}, {"Изменения реестра и служб требуют прав администратора. Перед применением убедитесь, что выбранные параметры подходят вашей системе.", "Registry and service changes require administrator rights. Make sure the selected settings suit your system before applying them."},
   {"Настройки уменьшают часть сбора данных и рекламы Windows. Полной анонимности они не обеспечивают; некоторые параметры зависят от редакции и версии Windows.", "These settings reduce some Windows data collection and advertising. They do not provide complete anonymity, and some options depend on your Windows edition and version."},
   {"Браузер для приватности", "Privacy browser"}, {"Установить выбранный браузер", "Install selected browser"}, {"Страница Tor Browser", "Tor Browser download page"},
   {"Mullvad Browser блокирует трекеры и помогает защищаться от цифрового отпечатка, но сам не скрывает IP. Tor Browser направляет трафик через сеть Tor. Не устанавливайте дополнительные расширения в Tor Browser и не входите в личные аккаунты, если нужна анонимность.", "Mullvad Browser blocks trackers and helps resist fingerprinting, but does not hide your IP address. Tor Browser routes traffic through the Tor network. Avoid extra extensions and personal accounts in Tor Browser when anonymity matters."},
   {"Параметры Windows", "Windows privacy settings"}, {"Открыть разрешения приложений", "Open app permissions"}, {"Создать точку и применить", "Create restore point and apply"},
   {"Отключить рекламный идентификатор", "Disable the advertising ID"}, {"для текущего пользователя; приложения могут использовать его для персонализированной рекламы", "for this user; apps may use it for personalized advertising"},
   {"Отключить персонализированные советы", "Turn off personalized tips"}, {"уменьшает рекомендации Windows на основе диагностических данных", "reduces Windows recommendations based on diagnostic data"},
   {"Отключить рекламные предложения и советы в меню Пуск", "Turn off ads and suggestions in the Start menu"}, {"текущий пользователь; часть предложений зависит от версии Windows", "current user; some suggestions depend on the Windows version"},
   {"Отключить журнал активности и синхронизацию действий", "Turn off activity history and activity sync"}, {"политика Windows; может быть недоступна или проигнорирована в отдельных редакциях", "Windows policy; may be unavailable or ignored by some editions"},
   {"Ограничить диагностические данные до обязательного минимума", "Limit diagnostic data to the required minimum"}, {"Windows продолжит отправлять диагностические данные, необходимые для работы и обновлений; доступность параметра зависит от редакции и версии", "Windows will continue sending diagnostic data needed for operation and updates. Availability depends on the edition and version."},
   {"сжимает и распаковывает архивы RAR, ZIP и других форматов; пробная версия", "creates and extracts RAR, ZIP, and other archives; trial version"},
   {"магазин и библиотека: покупка, установка, обновление и запуск игр", "store and library for buying, installing, updating, and launching games"}, {"воспроизводит видео и музыку в большинстве распространённых форматов", "plays video and music in most common formats"},
   {"перекодирует и сжимает видео, чтобы уменьшить размер или подготовить файл для публикации", "converts and compresses video to reduce file size or prepare it for publishing"}, {"записывает и редактирует звук: обрезает дорожки и помогает убрать шум", "records and edits audio, trims tracks, and helps reduce noise"},
   {"записывает экран и игры, а также ведёт прямые трансляции", "records your screen and games, and streams live"}, {"добавляет кодеки и плеер MPC-HC для воспроизведения многих видео- и аудиоформатов", "adds codecs and the MPC-HC player for many video and audio formats"},
   {"создаёт и редактирует документы, таблицы и презентации; открывает файлы Office", "creates and edits documents, spreadsheets, and presentations; opens Office files"}, {"быстро открывает PDF, EPUB, комиксы и другие документы для чтения", "quickly opens PDFs, EPUBs, comics, and other reading formats"},
   {"редактирует текст и код с вкладками, поиском и подсветкой синтаксиса", "edits text and code with tabs, search, and syntax highlighting"}, {"браузер блокирует трекеры и помогает защититься от цифрового отпечатка; сам по себе не скрывает IP", "blocks trackers and helps resist browser fingerprinting; does not hide your IP address"},
   {"сжимает и распаковывает архивы 7z, ZIP, RAR и других форматов", "creates and extracts 7z, ZIP, RAR, and other archives"}, {"находит файлы и папки по имени почти мгновенно, не перебирая их вручную", "finds files and folders by name almost instantly"},
   {"Требуется Windows 10 или новее.", "Requires Windows 10 or later."}, {"Требуется Windows 10 или новее, 64-разрядная версия.", "Requires 64-bit Windows 10 or later."},
   {"Система: ", "System: "}, {"Выбрана система: ", "Selected system: "}, {"Видеокарта: ", "Graphics card: "}, {"Не определена", "Unknown"}, {"Отметки программ обновлены для выбранного профиля; их можно изменить вручную.", "Software selections were updated for this profile. You can change them manually."},
   {"Максимальная производительность", "Maximum performance power plan"}, {"добавить схему питания через powercfg", "add a power plan with powercfg"}, {"Игровой режим без DVR", "Gaming without DVR recording"}, {"отключить запись Xbox Game Bar текущего пользователя", "turn off Xbox Game Bar recording for this user"},
   {"Снижение фоновой активности", "Reduce background activity"}, {"отключить приложения в фоне для текущего пользователя", "turn off background apps for this user"}, {"Упростить интерфейс", "Simplify the interface"}, {"отключить анимацию окон", "turn off window animations"},
   {"Отключить прозрачность", "Turn off transparency"}, {"тема текущего пользователя", "theme setting for this user"}, {"Очистить временные файлы", "Clean temporary files"}, {"временные каталоги Windows и пользователя", "Windows and user temporary folders"},
   {"Уменьшить нагрузку SysMain", "Reduce SysMain activity"}, {"перевести SysMain в ручной запуск", "set SysMain to start manually"}, {"Сбалансированная схема", "Balanced power plan"}, {"включить план питания Balanced", "activate the Balanced power plan"},
   {"Приоритет программ переднего плана", "Prioritize foreground apps"}, {"Win32PrioritySeparation = 0x26", "Win32PrioritySeparation = 0x26"}, {"освободить используемые временные каталоги", "clean temporary folders"}, {"Файл подкачки", "Paging file"}, {"оставить под управлением Windows", "leave managed by Windows"},
   {"Показывать расширения файлов", "Show file extensions"}, {"проводник текущего пользователя", "File Explorer setting for this user"}, {"Показывать скрытые файлы", "Show hidden files"}, {"Очистить кэш DNS", "Clear DNS cache"}, {"сбросить кэш системного DNS", "clear the system DNS cache"}, {"Уменьшить рекламные предложения", "Reduce advertising suggestions"}, {"отключить советы и подсказки", "turn off tips and suggestions"},
   {"Отметьте программы, доступные для этой версии Windows.", "Select software supported by this Windows version."}, {"Выберите папку установки.", "Choose an installation folder."},
   {"Точка восстановления создана.", "Restore point created."}, {"Ошибка", "Error"}, {"Создать точку восстановления и применить ", "Create a restore point and apply "}, {" операций? Некоторые изменения вступят в силу после перезагрузки.", " changes? Some changes take effect after a restart."},
   {"Подтверждение", "Confirmation"}, {"Отмеченные изменения применены.", "Selected changes have been applied."}, {"Операция остановлена: ", "Operation stopped: "}, {"Отметьте параметры Windows, которые хотите изменить.", "Select the Windows settings you want to change."},
   {" параметров приватности? Некоторые параметры зависят от версии Windows и могут изменить рекомендации или журнал активности.", " privacy settings? Some options depend on Windows and may change suggestions or activity history."}, {"Настройки приватности", "Privacy settings"}, {"Выбранные параметры применены. Для некоторых из них может потребоваться выход из системы или перезагрузка.", "Selected settings were applied. Some may require signing out or restarting."},
   {"Отметьте Mullvad Browser для установки. Для Tor Browser откройте официальную страницу загрузки.", "Select Mullvad Browser to install, or open the official Tor Browser download page."}, {"Mullvad Browser будет загружен с официального сайта, проверен по цифровой подписи и установлен в стандартную папку пользователя. Продолжить?", "Mullvad Browser will be downloaded from its official website, verified by digital signature, and installed in the user's standard folder. Continue?"}, {"Установка браузера", "Browser installation"},
   {"Загрузка и установка: ", "Downloading and installing: "}, {"Установка браузера завершена.", "Browser installation complete."}, {"Mullvad Browser установлен.", "Mullvad Browser has been installed."}, {"Приватность", "Privacy"}, {"Не удалось установить браузер.", "Could not install the browser."}, {"Ошибка установки", "Installation error"},
   {"Не удалось открыть страницу Tor Project. ", "Could not open the Tor Project page. "}, {"Не удалось открыть параметры приватности Windows. ", "Could not open Windows privacy settings. "}, {"Выберите папку установки", "Choose an installation folder"},
   {"Для каждой выбранной программы приложение скачает временный установщик, проверит его SHA-256 или цифровую подпись, запустит установку и удалит временный файл после её завершения. Установщики с собственным мастером покажут своё окно. Папка назначения передаётся только тем программам, которые её поддерживают.", "For each selected app, WinOptimizer downloads a temporary installer, checks its SHA-256 hash or digital signature, runs it, and deletes the temporary file when it exits. Installers with their own wizard show their own window. The destination folder is passed only to apps that support it."},
   {"Скачивание и установка: ", "Downloading and installing: "}, {"Установка выбранных программ завершена. Временные установщики удалены.", "Selected software has been installed. Temporary installers were deleted."}, {"Установка остановлена.", "Installation stopped."},
   {"Приложение выполнит поиск драйверов, предлагаемых настроенным Windows Update, и скачает найденные пакеты. Драйверы автоматически установлены не будут. Продолжить?", "WinOptimizer will search the configured Windows Update source and download matching driver packages. Drivers will not be installed automatically. Continue?"}, {"Драйверы Windows", "Windows drivers"}, {"Не удалось получить драйверы.", "Could not download drivers."}, {"Windows Update не смог выполнить поиск или загрузку драйверов.\n\n", "Windows Update could not search for or download drivers.\n\n"},
   {"Диск {0} (Свободно {1:0.0} ГБ из {2:0.0} ГБ)", "Drive {0} ({1:0.0} GB free of {2:0.0} GB)"},
   {"Для программы не задан безопасный HTTPS-источник.", "No secure HTTPS source is configured for this app."}, {"Разрешены только установщики EXE и MSI.", "Only EXE and MSI installers are allowed."}, {"Скачивание остановлено: издатель перенаправил запрос на небезопасный адрес.", "Download stopped: the publisher redirected to an insecure address."},
   {"Скачивание остановлено: конечный сервер не входит в список разрешённых издателей.", "Download stopped: the final server is not an approved publisher host."}, {"Скачивание остановлено: превышено допустимое число переадресаций.", "Download stopped: too many redirects."}, {"Размер установщика превышает ограничение 1 ГБ.", "The installer exceeds the 1 GB size limit."}, {"Загрузка остановлена: файл превышает ограничение 1 ГБ.", "Download stopped: the file exceeds the 1 GB size limit."},
   {"Контрольная сумма установщика не совпала с опубликованной издателем. Файл не будет запущен.", "The installer hash does not match the publisher's published hash. The file will not be run."}, {"Цифровая подпись установщика отсутствует или не прошла проверку доверия (0x", "The installer has no valid trusted digital signature (0x"}, {"). Файл не будет запущен.", "). The file will not be run."},
   {"Установщик завершился с кодом ", "The installer exited with code "}, {". Проверьте совместимость параметров установки.", ". Check whether the install arguments are supported."}, {"Компонент Windows Update Agent недоступен.", "Windows Update Agent is unavailable."}, {"Поиск доступных драйверов через Windows Update…", "Searching Windows Update for available drivers…"},
   {"Windows Update не нашёл доступных драйверов для этого устройства.", "Windows Update found no available drivers for this device."}, {"Найдено пакетов: ", "Packages found: "}, {". Начинаю загрузку…", ". Starting download…"}, {"Windows Update не завершил загрузку драйверов (код ", "Windows Update did not finish downloading drivers (code "}, {". Проверьте службу Windows Update и повторите попытку.", ". Check the Windows Update service and try again."}, {"Загрузка драйверов завершена.", "Driver download complete."}, {"Загрузка завершена частично. Некоторые пакеты могли не скачаться.", "Download partially completed. Some packages may not have downloaded."}, {"Скачано: ", "Downloaded: "}, {" из ", " of "},
   {"Запуск этой системной команды не разрешён.", "Running this system command is not allowed."}, {"Системная команда не найдена.", "System command was not found."}, {"Изменение этой службы не разрешено.", "Changing this service is not allowed."}, {" завершился с кодом ", " exited with code "}, {"Не удалось создать точку восстановления. Проверьте, включена ли защита системы. ", "Could not create a restore point. Check whether System Protection is enabled. "}, {"WMI не смог создать точку восстановления.", "WMI could not create a restore point."}, {"точка перед изменениями", "point before changes"}
  };

  public static void SetLanguage(string language) { Language = language == "ru" ? "ru" : "en"; }
  public static string T(string text)
  {
   if (text == null || Language == "ru") return text;
   string translated;
   return English.TryGetValue(text, out translated) ? translated : text;
  }
  public static void Apply(Window window)
  {
   window.Title = T(window.Title);
   ApplyElement(window);
  }
  static void ApplyElement(DependencyObject parent)
  {
   TextBlock text = parent as TextBlock;
   if (text != null && text.ReadLocalValue(TextBlock.TextProperty) is string) text.Text = T(text.Text);
   ContentControl content = parent as ContentControl;
   if (content != null && content.Content is string) content.Content = T((string)content.Content);
   HeaderedContentControl header = parent as HeaderedContentControl;
   if (header != null && header.Header is string) header.Header = T((string)header.Header);
   foreach (DependencyObject child in LogicalTreeHelper.GetChildren(parent).Cast<object>().OfType<DependencyObject>().ToArray()) ApplyElement(child);
  }
 }

 public sealed class LocalizedTextConverter : IValueConverter
 {
  public object Convert(object value, System.Type targetType, object parameter, CultureInfo culture) { return Localization.T(value == null ? null : value.ToString()); }
  public object ConvertBack(object value, System.Type targetType, object parameter, CultureInfo culture) { return Binding.DoNothing; }
 }
}

