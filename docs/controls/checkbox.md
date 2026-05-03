# CheckBox 多选框

在多个候选项中允许选中任意数量的值。

## 何时使用

- 在多个候选项中允许选中任意数量的值。

## 当前实现方式

- 基于 Avalonia 原生 `CheckBox` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/CheckBox.axaml`。

## 最小示例

```xml
<CheckBox IsChecked="True" Content="多选" />
```

## 常用属性 / 事件 / 样式类

- `IsChecked`：勾选状态。
- `Content`：标签文案或自定义内容。
- 支持 `:checked`、`:indeterminate`、`:disabled` 样式。

## 配套类型 / 组合方式

- 既可以只显示勾选框，也可以搭配文本说明。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/CheckBoxDemoView.axaml`

## 已知限制 / 注意事项

- 当前是原生控件皮肤化，分组布局由调用方决定。
