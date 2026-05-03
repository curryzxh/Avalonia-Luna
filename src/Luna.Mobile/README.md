# Luna.Avalonia.Mobile

`Luna.Avalonia.Mobile` is a touch-first Avalonia control library for mobile-oriented UI surfaces.

It provides reusable controls, themes, and tokens for building mobile experiences with Avalonia, including components such as:

- `ActionSheetHost`
- `Button`
- `Cascader`
- `DateTimePicker`
- `DropdownMenu`
- `Fab`
- `Loading`
- `NavBar`
- `NoticeBar`
- `Picker`
- `PullDownRefresh`
- `SearchBar`
- `Segmented`
- `Skeleton`
- `Stepper`
- `Steps`
- `SwipeCell`
- `Tag`
- `Toast`
- `Watermark`

## Install

```bash
dotnet add package Luna.Avalonia.Mobile --version 1.0.1
```

## Usage

Merge the Luna theme resources in your Avalonia application:

```xml
<Application xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml">
  <Application.Styles>
    <StyleInclude Source="avares://Luna.Mobile/Themes/Index.axaml" />
    <StyleInclude Source="avares://Luna.Mobile/Themes/Generic.axaml" />
  </Application.Styles>
</Application>
```

Use Luna controls in XAML with the Luna XML namespace:

```xml
<UserControl xmlns="https://github.com/avaloniaui"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:luna="https://github.com/curryzxh/Luna">
  <luna:Button Classes="primary" Content="Confirm" />
</UserControl>
```

## Notes

- The package currently targets `net10.0` and `net10.0-android`.
- Control APIs live under the `Luna.Mobile.Controls` namespace.
