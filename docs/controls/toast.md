# Toast 轻提示

做短暂、轻量、不打断操作的反馈提示。

## 何时使用

- 做短暂、轻量、不打断操作的反馈提示。

## 当前实现方式

- 由静态入口 `Toast`、宿主 `ToastHost` 和 `ToastOptions` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Toast.axaml`。

## 最小示例

```csharp
Toast.Show(new ToastOptions
{
    Message = "轻提示文字内容",
    Theme = ToastTheme.Success
});
```

## 常用属性 / 事件 / 样式类

- `Message`：提示文案。
- `Theme`：`Default`、`Success`、`Warning`、`Error`、`Loading`。
- `Direction`：`Row` / `Column`。
- `Placement`：`Top` / `Middle` / `Bottom`。
- `Duration`：持续时间。
- 静态入口：`Show(string)`、`Show(ToastOptions)`、`Clear()`。

## 配套类型 / 组合方式

- 页面里需要先放一个 `ToastHost`，sample 中的主题和方向都通过 `ToastOptions` 配置。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/ToastDemoView.axaml`

## 已知限制 / 注意事项

- 当前图标由 `ToastTheme` 自动映射，不支持在 `ToastOptions` 中直接传自定义图标。
