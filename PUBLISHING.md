# Как опубликовать WinOptimizer на GitHub

## Описание репозитория

Скопируй этот текст в поле **Description** при создании репозитория:

> Windows optimizer with gaming profiles, privacy controls, app installation, driver downloads, and English/Russian UI.

Можно добавить темы: `windows`, `wpf`, `dotnet-framework`, `optimizer`, `privacy`.

## 0. Установи Git для Windows

На скриншоте PowerShell пишет, что команда `git` не найдена. Значит Git не установлен или не добавлен в PATH; команды публикации пока не выполнились.

1. Установи Git с [официальной страницы Git for Windows](https://git-scm.com/install/win).
2. Во время установки оставь включённым использование Git из командной строки (это стандартный вариант установщика).
3. Закрой все окна PowerShell и открой новое.
4. Проверь установку:

   ```powershell
   git --version
   ```

Если выводится версия Git, продолжай. GitHub Desktop тоже включает Git, но команды ниже выполняются в PowerShell после установки и перезапуска окна.

## 1. Создай пустой репозиторий

1. Войди на [github.com](https://github.com) и открой [создание нового репозитория](https://github.com/new).
2. Введи имя `WinOptimizer` и вставь описание выше.
3. Выбери **Public**, если хочешь сделать исходники общедоступными.
4. Не ставь галочки для автоматического README, `.gitignore` и лицензии: эти файлы уже есть в проекте, а повторная инициализация может создать конфликт.
5. Нажми **Create repository**. Оставь страницу открытой, она понадобится для адреса репозитория.

## 2. Отправь исходники из этой папки

Открой PowerShell и выполни команды по одной. В первой команде путь ведёт к текущей папке проекта:

```powershell
cd "C:\Users\k1nexis\OneDrive\Документы\WinOptimizer"
git init
git add .
git status --short
git commit -m "Initial WinOptimizer release"
git branch -M main
git remote add origin https://github.com/YOUR-USERNAME/WinOptimizer.git
git push -u origin main
```

Замени `YOUR-USERNAME` на свой логин GitHub. В `git status --short` проверь список перед commit: `.gitignore` исключает папки сборки `bin`, `obj` и `dist`, поэтому в репозиторий попадут исходники, README и сценарии сборки, а не временные файлы.

При первом `git push` GitHub может открыть окно входа. Если Git сообщает, что не настроены имя или почта автора commit, укажи свои данные:

```powershell
git config --global user.name "Твоё имя"
git config --global user.email "Твоя почта GitHub"
```

После этого повтори `git commit` и `git push -u origin main`.

## 3. Проверь сборку на GitHub

1. Открой репозиторий на GitHub и выбери вкладку **Actions**.
2. В списке слева нажми **Build Windows release**.
3. Нажми **Run workflow**, выбери ветку `main` и подтверди запуск.
4. Дождись зелёного статуса. Открой завершённый запуск и скачай artifact `WinOptimizer-Windows` для пробной проверки.

Ручной запуск workflow срабатывает после того, как файл `.github/workflows/release.yml` попал в ветку по умолчанию. GitHub описывает этот запуск в [документации Actions](https://docs.github.com/en/actions/how-tos/manage-workflow-runs/manually-run-a-workflow).

## 4. Создай публичный релиз с EXE

Когда проверишь артефакт, в PowerShell из папки проекта выполни:

```powershell
git tag v1.0.0
git push origin v1.0.0
```

Тег `v1.0.0` запускает workflow повторно. Он создаст GitHub Release и приложит `WinOptimizer.zip` с EXE, README и файлом SHA-256. Релиз появится во вкладке **Releases** репозитория.

Чтобы выпустить обновление, увеличь номер версии, например на `v1.0.1`, и отправь новый тег:

```powershell
git tag v1.0.1
git push origin v1.0.1
```

## Перед распространением

- В проекте нет файла лицензии. Если хочешь разрешить другим использовать или изменять исходники, выбери и добавь лицензию. Если не уверен, пока оставь без лицензии.
- EXE не подписан сертификатом разработчика. Windows может показать предупреждение SmartScreen.
- Программа требует .NET Framework 4.8 и запрашивает права администратора. Прочитай README и проверь приложение на виртуальной машине до запуска на основном ПК.
- Репозиторий содержит исходный код, а GitHub Release получает собранный архив. Release workflow собирается на Windows runner GitHub Actions.
