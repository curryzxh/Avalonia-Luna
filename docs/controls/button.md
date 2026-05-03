# Button 按钮

触发提交、确认、删除、跳转等主次操作。

## 何时使用

- 触发提交、确认、删除、跳转等主次操作。

## 当前实现方式

- 基于 Avalonia 原生 `Button` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/Button.axaml`。

## 最小示例

```xml
<Button Classes="primary large" Content="提交" />
```

## 常用属性 / 事件 / 样式类

- `Classes`：主题/变体常用 `primary`、`light`、`danger`、`outline`、`text`、`ghost`。
- `Classes`：尺寸支持 `large`、`small`、`extra-small`。
- `Classes`：形状支持 `square`、`round`、`circle`、`rectangle`。
- `IsEnabled`：控制禁用态。

## 配套类型 / 组合方式

- 可直接把 `Path`、`TextBlock` 等组合进内容区做图标按钮。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/ButtonDemoView.axaml`

## 已知限制 / 注意事项

- 样式切换依赖 `Classes` 组合，而不是额外自定义属性。
