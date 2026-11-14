# WinOptimizer

WinOptimizer is a Windows desktop utility built with WPF and .NET Framework 4.8. It provides optional optimization profiles, a curated software installer, a Windows Update driver-download helper, and privacy settings. The first screen asks for a language; English is selected by default, and Russian is also available.

## Download and run

Download `WinOptimizer.zip` from the repository Releases page, extract it, and run `WinOptimizer.exe`. Windows asks for administrator permission because some selected settings apply to the computer. Review every selection before applying it.

Requirements:

- Windows 10 or 11 recommended. Some basic functionality may run on Windows 7 SP1 or Windows 8.1 with .NET Framework 4.8 installed, but newer software in the catalog is not supported there.
- .NET Framework 4.8. It is included with current Windows 10/11 releases; older supported Windows versions may need the Microsoft installer.
- Internet access is needed to download selected applications or driver packages.

## Features

- **Optimization profiles:** gaming, older PCs/laptops, media work, and work/study. A profile only preselects proposed settings and software. No changes are made until you confirm the action.
- **Software installation:** downloads selected installers from configured HTTPS sources, enforces a host allowlist and size limit, verifies a configured SHA-256 hash or a trusted Authenticode signature, then runs the installer. Installers can show their own setup window. WinRAR is listed in each software category, but duplicate entries install only once.
- **Driver downloads:** searches the configured Windows Update source for assigned driver updates and downloads matching packages. It does not install drivers automatically. In a virtual machine, Windows Update searches for drivers for that VM's virtual hardware.
- **Privacy:** offers Mullvad Browser and links to the official Tor Browser download page. It also offers selected Windows privacy settings. The app does not remove Windows apps or components.
- **Restore points:** the app attempts to create a restore point before applying selected system settings. This can fail if System Protection is disabled or blocked by policy.

Privacy settings reduce some advertising and activity features. They do not make Windows fully anonymous, disable all data collection, or hide your IP address. The diagnostic-data policy requests the required minimum where supported; Windows can still send data needed for security, updates, and operation. Per-app location, camera, and microphone permissions remain under Windows Settings.

Mullvad Browser is intended to reduce tracking and browser fingerprinting; it does not route traffic through Tor or hide your IP. Tor Browser routes traffic through the Tor network. Follow the Tor Project's guidance and avoid extra extensions if anonymity is important.

## Build locally

On Windows, install Visual Studio Build Tools or Visual Studio with **.NET desktop development**, the .NET Framework 4.8 Developer Pack, and WPF build tools. Then run:

```powershell
./build-release.ps1
```

The script builds Release and writes `dist/WinOptimizer.exe` and `dist/WinOptimizer.zip`. The executable depends on the Windows .NET Framework 4.8 runtime; this is not a self-contained .NET deployment. No third-party NuGet packages are used.

Alternatively, open `WinOptimizer.csproj` in Visual Studio and build the **Release / Any CPU** configuration. The raw build output is `bin/Release/WinOptimizer.exe`.

## Publish a GitHub release

For the full step-by-step publishing guide in Russian, see [PUBLISHING.md](PUBLISHING.md).

1. Create a GitHub repository and push the project files.
2. Run **Actions → Build Windows release → Run workflow** to build and download a test artifact.
3. When ready to publish, create and push a version tag, for example:

   ```powershell
   git tag v1.0.0
   git push origin v1.0.0
   ```

The workflow builds the project on Windows and attaches `WinOptimizer.zip` to a GitHub Release for `v*` tags. Review the source, choose a repository license, and test the artifact on a clean Windows machine before distributing it broadly. The executable is not code-signed by this project.

## Project files

- `MainWindow.xaml` and `MainWindow.xaml.cs` — main interface, profiles, software catalog, and privacy actions.
- `LanguagePickerWindow.cs`, `Localization.cs` — startup language selection and English/Russian UI strings.
- `SystemActions.cs` — Windows and GPU detection, restore points, temporary-file cleanup, and selected system settings.
- `InstallerService.cs` — download limits, host checks, hash/signature verification, and installer execution.
- `DriverUpdateService.cs` — Windows Update Agent driver search and download.
- `app.manifest` — requests administrator privileges.
- `build-release.ps1` and `.github/workflows/release.yml` — local packaging and GitHub release build.

---

# WinOptimizer (русский)

WinOptimizer — настольная программа для Windows на WPF и .NET Framework 4.8. В ней есть профили оптимизации, каталог установщиков программ, загрузка драйверов через Windows Update и параметры приватности. При запуске появляется выбор языка: по умолчанию выбран English, также доступен русский.

## Скачать и запустить

Скачайте `WinOptimizer.zip` со страницы Releases репозитория, распакуйте архив и запустите `WinOptimizer.exe`. Windows запросит права администратора, потому что некоторые выбранные настройки затрагивают систему. Перед применением проверьте отмеченные пункты.

Требования:

- Рекомендуются Windows 10 или 11. Базовые функции могут работать на Windows 7 SP1 или Windows 8.1 с установленным .NET Framework 4.8, но новые программы из каталога там не поддерживаются.
- .NET Framework 4.8. Он входит в современные выпуски Windows 10/11; на старых поддерживаемых версиях Windows может понадобиться установщик Microsoft.
- Для загрузки программ и пакетов драйверов требуется интернет.

## Возможности

- **Профили оптимизации:** игры, старый ПК/ноутбук, монтаж и рендеринг, работа и учёба. Профиль только отмечает предлагаемые настройки и программы. Изменения вступают в силу только после подтверждения.
- **Установка программ:** загружает выбранные установщики с заданных HTTPS-источников, проверяет разрешённый домен, ограничивает размер файла и проверяет заданную SHA-256-сумму либо доверенную цифровую подпись Authenticode, после чего запускает установщик. Установщик может показать собственное окно. WinRAR отображается в каждой категории, но повторно не запускается.
- **Драйверы:** ищет назначенные драйверы через выбранный источник Windows Update и скачивает найденные пакеты. Автоматической установки драйверов нет. В виртуальной машине поиск относится к виртуальному оборудованию гостевой Windows.
- **Приватность:** предлагает Mullvad Browser и открывает официальную страницу загрузки Tor Browser. Также доступны отдельные параметры приватности Windows. Программа не удаляет приложения или компоненты Windows.
- **Точки восстановления:** перед изменением выбранных системных настроек приложение пытается создать точку восстановления. Это может не получиться, если защита системы отключена или запрещена политикой.

Параметры приватности уменьшают часть рекламы и функций истории активности. Они не делают Windows полностью анонимной, не отключают весь сбор данных и не скрывают IP-адрес. Политика диагностики запрашивает обязательный минимум там, где Windows это поддерживает; система всё равно может отправлять данные, необходимые для безопасности, обновлений и работы. Разрешения геолокации, камеры и микрофона для отдельных приложений настраиваются в параметрах Windows.

Mullvad Browser предназначен для снижения трекинга и защиты от цифрового отпечатка браузера; он не направляет трафик через Tor и не скрывает IP. Tor Browser направляет трафик через сеть Tor. Если важна анонимность, следуйте рекомендациям Tor Project и не устанавливайте дополнительные расширения.

## Локальная сборка

В Windows установите Visual Studio Build Tools или Visual Studio с компонентом **Разработка классических приложений .NET**, Developer Pack для .NET Framework 4.8 и средствами сборки WPF. Затем запустите:

```powershell
./build-release.ps1
```

Скрипт соберёт Release и создаст `dist/WinOptimizer.exe` и `dist/WinOptimizer.zip`. EXE требует среду выполнения .NET Framework 4.8 в Windows — это не автономная поставка со встроенной средой. Сторонние NuGet-пакеты не используются.

Также можно открыть `WinOptimizer.csproj` в Visual Studio и собрать конфигурацию **Release / Any CPU**. Результат сборки появится в `bin/Release/WinOptimizer.exe`.

## Публикация релиза на GitHub

Подробная пошаговая инструкция и готовое описание репозитория: [PUBLISHING.md](PUBLISHING.md).

1. Создайте репозиторий GitHub и отправьте в него файлы проекта.
2. Запустите **Actions → Build Windows release → Run workflow**, чтобы собрать и скачать тестовый артефакт.
3. Когда версия будет готова, создайте и отправьте тег, например:

   ```powershell
   git tag v1.0.0
   git push origin v1.0.0
   ```

Workflow собирает приложение на Windows и прикрепляет `WinOptimizer.zip` к GitHub Release для тегов `v*`. Перед распространением проверьте исходники, выберите лицензию репозитория и испытайте архив на чистой Windows. EXE не подписан цифровой подписью разработчика этого проекта.

## Файлы проекта

- `MainWindow.xaml` и `MainWindow.xaml.cs` — интерфейс, профили, каталог программ и настройки приватности.
- `LanguagePickerWindow.cs`, `Localization.cs` — выбор языка при запуске и английский/русский интерфейс.
- `SystemActions.cs` — определение Windows и видеокарты, точки восстановления, очистка временных файлов и выбранные системные настройки.
- `InstallerService.cs` — лимит размера, проверка домена и подписи/хеша, запуск установщика.
- `DriverUpdateService.cs` — поиск и загрузка драйверов через Windows Update Agent.
- `app.manifest` — запрос прав администратора.
- `build-release.ps1` и `.github/workflows/release.yml` — локальная упаковка и сборка релиза GitHub.

