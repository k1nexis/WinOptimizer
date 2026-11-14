# Windows Optimizer for All — публикация

## Имя и описание

- Название репозитория: `Windows-Optimizer-for-All`
- Отображаемое название: **Windows Optimizer for All**
- Описание GitHub: `Windows optimizer for all: gaming profiles, privacy controls, app installation, hardware detection, and restore protection.`

Локальная папка на этом компьютере пока называется `WinOptimizer`; это не влияет на имя репозитория или приложения.

## Переименование репозитория на GitHub

Открой настройки репозитория → **General** → **Repository name**, укажи `Windows-Optimizer-for-All` и подтверди **Rename**. Затем обнови адрес origin в PowerShell:

```powershell
cd "C:\Users\k1nexis\OneDrive\Документы\WinOptimizer"
git remote set-url origin https://github.com/k1nexis/Windows-Optimizer-for-All.git
git remote -v
```

GitHub перенаправляет большинство старых ссылок и Git-операций, но используй новый адрес для будущей работы.

## Сборка и публикация

1. Отправь изменения в основную ветку:

   ```powershell
   git add .
   git status --short
   git commit -m "Update Windows Optimizer for All branding"
   git push origin main
   ```

2. На GitHub открой **Actions → Build Windows release → Run workflow** и дождись зелёной галочки. Артефакт сборки называется `Windows-Optimizer-for-All-Windows`.
3. Для публичного GitHub Release создай уникальный тег и отправь его. Например, для этой версии:

   ```powershell
   git tag v1.4.1
   git push origin v1.4.1
   ```

   Workflow прикрепит `Windows-Optimizer-for-All.zip` к релизу. ZIP содержит `WindowsOptimizerForAll.exe`, README, эту инструкцию и SHA-256.

## Примечания

- Для локальной сборки нужны Visual Studio Build Tools, WPF build tools и .NET Framework 4.8 Developer Pack. Команда: `./build-release.ps1`.
- EXE не подписан сертификатом разработчика; SmartScreen может показать предупреждение.
- Проверь приложение на виртуальной машине перед запуском на основном ПК. Не устанавливай неподтверждённые драйверы и внимательно проверяй выбранные системные изменения.
