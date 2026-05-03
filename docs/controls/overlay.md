# Overlay 遮罩层

强调当前界面状态、承载简单弹层内容，或作为其他浮层的基础遮罩能力。

## 何时使用

- 强调当前界面状态、承载简单弹层内容，或作为其他浮层的基础遮罩能力。

## 当前实现方式

- Luna 自定义控件 `Overlay`，样式位于 `src/Luna.Mobile/Themes/Controls/Overlay.axaml`。

## 最小示例

```xml
<luna:Overlay Visible="True" OverlayBrush="#804084FF" />
```

## 常用属性 / 事件 / 样式类

- `Visible`：是否显示遮罩。
- `OverlayBrush`：遮罩颜色。
- `Duration`：开关动画时长。
- `PreventScrollThrough`：是否阻止穿透点击和滚动。
- 事件 `Clicked`、`Opening`、`Opened`、`Closing`、`Closed`。

## 配套类型 / 组合方式

- 可以把任意内容直接作为 `Overlay` 的子元素放在遮罩中央。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/OverlayDemoView.axaml`

## 已知限制 / 注意事项

- 如果需要弱提示穿透效果，可关闭 `PreventScrollThrough`。
