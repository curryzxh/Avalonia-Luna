# Dialog 对话框

展示重要提示、确认操作或输入中断式任务。

## 何时使用

- 展示重要提示、确认操作或输入中断式任务。

## 当前实现方式

- 由静态入口 `Dialog`、宿主 `DialogHost`、配置 `DialogOptions` 和按钮配置 `DialogButtonOptions` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Dialog.axaml`。

## 最小示例

```csharp
Dialog.Show(new DialogOptions
{
    Title = "对话框标题",
    Content = "告知当前状态"
});
```

## 常用属性 / 事件 / 样式类

- `Title`、`Content`：标题和正文。
- `Top`、`Middle`：自定义顶部/中部内容。
- `ConfirmButton`、`CancelButton`：按钮配置。
- `ButtonLayout`：横向或纵向按钮布局。
- `ShowCloseButton`、`CloseOnOverlayClick`：关闭策略。
- 事件 `Confirmed`、`Canceled`、`Closed`。

## 配套类型 / 组合方式

- 页面里先放 `DialogHost`，静态 `Dialog.Show()` 会使用当前宿主。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/DialogDemoView.axaml`

## 已知限制 / 注意事项

- sample 中的图片型和输入型对话框通过自定义内容实现。
