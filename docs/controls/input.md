# Input 输入框

采集短文本、编号、名称、搜索条件等单行输入内容。

## 何时使用

- 采集短文本、编号、名称、搜索条件等单行输入内容。

## 当前实现方式

- 基于 Avalonia 原生 `TextBox` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/TextBox.axaml`。

## 最小示例

```xml
<TextBox Watermark="请输入内容" />
```

## 常用属性 / 事件 / 样式类

- `Text`：输入值。
- `Watermark`：占位提示。
- `Classes`：支持 `filled`、`outlined`、`quiet`、`dark`、`error`。
- `IsReadOnly`、`MaxLength`、`PasswordChar` 等原生属性仍可用。

## 配套类型 / 组合方式

- sample 中通过外层布局组合了前后缀、说明文字和表单行。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/InputDemoView.axaml`

## 已知限制 / 注意事项

- 当前 `Input` 只是 `TextBox` 的 Luna 样式语义，不是单独控件类。
