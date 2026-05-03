# ActionSheet 动作面板

从底部弹出一组和当前场景相关的操作项。

## 何时使用

- 从底部弹出一组和当前场景相关的操作项。

## 当前实现方式

- 由 `ActionSheetHost`、`ActionSheetOptions`、`ActionSheetItem` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/ActionSheet.axaml`。

## 最小示例

```csharp
var options = new ActionSheetOptions
{
    CancelText = "cancel",
    Items = [ new ActionSheetItem { Label = "Move" } ]
};
```

## 常用属性 / 事件 / 样式类

- `Items`：选项列表。
- `Description`：面板描述。
- `CancelText`：取消文案。
- `Theme`：`List` / `Grid`。
- `Align`：列表对齐方式。
- 事件 `Selected`、`CancelRequested`、`Closed`。

## 配套类型 / 组合方式

- 列表和宫格都由同一个宿主渲染。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/ActionSheetDemoView.axaml`

## 已知限制 / 注意事项

- sample 同时演示了宿主调用和静态调用两种打开方式。
