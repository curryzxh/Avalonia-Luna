# Link 链接

展示轻量跳转入口、外部链接或补充操作。

## 何时使用

- 展示轻量跳转入口、外部链接或补充操作。

## 当前实现方式

- 基于 Avalonia 原生 `HyperlinkButton` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/Link.axaml`。

## 最小示例

```xml
<HyperlinkButton Classes="primary small underline" Content="跳转链接" />
```

## 常用属性 / 事件 / 样式类

- `Classes`：主题支持 `primary`、`danger`、`success`、`warning`。
- `Classes`：尺寸支持 `small`、`medium`、`large`。
- `Classes="underline"`：开启下划线样式。
- `IsEnabled`：控制禁用态。

## 配套类型 / 组合方式

- 前后缀图标通常直接写在 `Content` 中，用 `StackPanel + TextBlock + Path` 组合。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/LinkDemoView.axaml`

## 已知限制 / 注意事项

- 当前 `Link` 是皮肤化的 `HyperlinkButton`，不是独立 Luna 控件类型。
