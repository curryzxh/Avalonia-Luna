# Switch 开关

用于开启或关闭某项即时生效的状态。

## 何时使用

- 用于开启或关闭某项即时生效的状态。

## 当前实现方式

- 基于 Avalonia 原生 `ToggleSwitch` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/ToggleSwitch.axaml`。

## 最小示例

```xml
<ToggleSwitch IsChecked="True" />
```

## 常用属性 / 事件 / 样式类

- `IsChecked`：开关状态。
- `IsEnabled`：禁用态。
- `Classes`：支持 `large`、`small`。

## 配套类型 / 组合方式

- 可单独使用，也可塞进 `Cell.RightContent` 形成设置项。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SwitchDemoView.axaml`

## 已知限制 / 注意事项

- 当前是原生开关皮肤化，不额外包装为 `Switch` 类型。
