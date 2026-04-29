# Luna

Luna is an Avalonia control library workspace for building a unified visual style across desktop and mobile surfaces.

## Projects

- `src/Luna.Desktop/`：桌面端自定义控件库，当前包含 `DesktopBadge` 和 `Themes/Generic.axaml` 作为模板化控件骨架。
- `src/Luna.Mobile/`：移动端自定义控件库，当前包含 `MobileActionChip` 和 `Themes/Generic.axaml` 作为触控优先控件骨架。
- `samples/Luna.Desktop.Sample/`：桌面端示例应用，用于调试和展示 `Luna.Desktop` 控件。
- `samples/Luna.Mobile.Sample/`：Android 移动端示例应用，用于调试和展示 `Luna.Mobile` 控件。

## Build

```bash
dotnet restore Luna.sln
dotnet build Luna.sln --no-restore
```

Desktop sample:

```bash
dotnet run --project samples/Luna.Desktop.Sample/Luna.Desktop.Sample.csproj
```

Mobile sample:

```bash
dotnet build samples/Luna.Mobile.Sample/Luna.Mobile.Sample.csproj --no-restore
```
