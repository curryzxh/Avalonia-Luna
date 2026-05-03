# Radio 单选框

在同一组选项中只能选择一个值。

## 何时使用

- 在同一组选项中只能选择一个值。

## 当前实现方式

- 基于 Avalonia 原生 `RadioButton` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/RadioButton.axaml`。

## 最小示例

```xml
<RadioButton GroupName="group1" IsChecked="True" Content="单选" />
```

## 常用属性 / 事件 / 样式类

- `GroupName`：分组名，相同分组内互斥。
- `IsChecked`：当前是否选中。
- `Content`：标签或自定义内容。
- 支持 `:checked` 和 `:disabled` 样式。

## 配套类型 / 组合方式

- 可以和 `Grid`、`StackPanel` 组合形成单选列表。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/RadioDemoView.axaml`

## 已知限制 / 注意事项

- 当前没有额外包装的 `Radio` 类型。
