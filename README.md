# Poor Man's T-SQL Formatter for SSMS 22

A Visual Studio extension (VSIX) that integrates the [Poor Man's T-SQL Formatter](https://github.com/TaoK/PoorMansTSqlFormatter) into **SQL Server Management Studio 22** (VS 2026 shell).

It reformats T-SQL code cleanly and **preserves comments** (`--` and `/* */`).

## Features

- **Docked toolbar** "Poor Man's T-SQL Formatter" below the menu bar (between "Standard" and "SQL-Editor")
  - `Format Selection`
  - `Format Whole Document`
  - `Formatting Settings…`
- Custom toolbar icons
- Keyboard shortcuts
- Comment-preserving, configurable formatting (indentation, keyword casing, etc.)
- **Localized UI** — German texts on German systems (`de-DE`, `de-AT`, `de-CH`, …), English everywhere else (toolbar/menu captions, message boxes, settings dialog)
- Per-user install — no admin rights required

## System requirements

- SQL Server Management Studio 22 (SSMS 22)
- Windows 10 / 11

## Installation

Download the latest `.vsix` from the [Releases](https://github.com/dislhaas/dislPoorMansTSqlFormatter-SSMS22/releases) page.

1. Close SSMS.
2. Double-click the `.vsix` file, then click **Install**.
3. Start SSMS.

The toolbar "Poor Man's T-SQL Formatter" appears docked below the menu bar. It can be toggled via **View > Toolbars**.

> **Important:** On the very first start, click one menu item under **Tools > Poor Man's T-SQL Formatter** (e.g. "Formatting Settings…") once. This activates the keyboard shortcuts permanently.

## Usage

| Command | Description |
|---|---|
| Format Selection | formats only the selected text |
| Format Whole Document | formats the whole active script |
| Formatting Settings… | opens the options (indentation, casing, …) |

| Shortcut | Action |
|---|---|
| `Ctrl+Alt+F` | Format Selection |
| `Ctrl+Alt+D` | Format Whole Document |
| `Ctrl+Alt+K` | Formatting Settings |

## Build from source

Required: **Visual Studio 2026** (v18.0 — the same shell SSMS 22 is built on) with the **Visual Studio extension development** workload. The project uses `Microsoft.VisualStudio.SDK` and `Microsoft.VSSDK.BuildTools` via NuGet.

Command line:

```
msbuild PoorMansTSqlFormatter.SSMS21.VSIX\PoorMansTSqlFormatter.SSMS21.VSIX.csproj /t:Restore /p:Configuration=Release /p:VisualStudioVersion=18.0
msbuild PoorMansTSqlFormatter.SSMS21.VSIX\PoorMansTSqlFormatter.SSMS21.VSIX.csproj /t:Build   /p:Configuration=Release /p:VisualStudioVersion=18.0
```

(`VisualStudioVersion=18.0` is required so the VSSDK BuildTools resolve the VS 18 shell.)

The resulting `PoorMansTSqlFormatter.SSMS21.VSIX.vsix` is in `PoorMansTSqlFormatter.SSMS21.VSIX\bin\Release\`.

> **Build gotchas:**
> - **Restore and Build separately.** The combined `/t:Restore;Build` target list puts MSBuild into restore mode, which skips the NuGet `VSToolsPath` property — the build then falls back to the VS-installed VSSDK targets and `CreatePkgDef` fails with a `Microsoft.VisualStudio.Shell.Framework 18.0` resolution error. Run the two commands above in sequence.
> - **Stale manifest.** When bumping the version number, delete `obj\Release\extension.vsixmanifest`, `bin\Release\extension.vsixmanifest` and the old `.vsix` before rebuilding — otherwise the old version number lands in the VSIX and the installer refuses the update.

## Uninstall

- **Option A:** Open **Extensions > Manage Extensions**, select the extension, click **Uninstall**, restart SSMS.
- **Option B:** Close SSMS and run the VSIX uninstaller:
  ```
  "%ProgramFiles%\Microsoft SQL Server Management Studio 22\Release\Common7\IDE\VSIXInstaller.exe" /q /u:PoorMansTSqlFormatter.SSMS21.VSIX.93aae78f-16c6-49f1-92ff-d53a474d56b9
  ```

## License

**AGPL-3.0** — see [LICENSE.txt](LICENSE.txt).

This project is a modified, SSMS-22-specific build of the [Poor Man's T-SQL Formatter](https://github.com/TaoK/PoorMansTSqlFormatter) by [Tao Klerks](https://github.com/TaoK) (originally authored by Joseph Shakely). The formatter engine (`PoorMansTSqlFormatterLib`, `PoorMansTSqlFormatterLibShared`) is unmodified upstream code; the SSMS 22 VSIX integration is the contribution of this repository.

*LinqBridge.dll* is included under its [BSD-style license](PoorMansTSqlFormatterLib/References/LinqBridge/COPYING.txt).
