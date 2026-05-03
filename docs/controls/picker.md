# Picker 选择器

在预设离散选项中滚动选择一个或多个值。

## 何时使用

- 在预设离散选项中滚动选择一个或多个值。

## 当前实现方式

- 由 `PickerHost`、`PickerOptions` 和 `PickerColumn` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Picker.axaml`。

## 最小示例

```csharp
var options = new PickerOptions
{
    Title = "选择城市",
    Columns = [ new PickerColumn { Items = ["北京", "上海"] } ]
};
```

## 常用属性 / 事件 / 样式类

- `Title`：标题。
- `Columns`：列集合。
- `SheetHeight`：面板高度。
- `CloseOnOverlayClick`：点击遮罩是否关闭。
- `PickerColumn.Items`：候选项集合。
- 事件 `Confirmed`、`Closed`。

## 配套类型 / 组合方式

- 单列和多列都通过 `Columns` 描述。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/PickerDemoView.axaml`

## 已知限制 / 注意事项

- 当前 `Picker` 以宿主 + 配置方式使用。
