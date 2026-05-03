# Slider 滑动选择器

通过拖拽在连续数值区间内选择值。

## 何时使用

- 通过拖拽在连续数值区间内选择值。

## 当前实现方式

- 基于 Avalonia 原生 `Slider` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/Slider.axaml`。

## 最小示例

```xml
<Slider Minimum="0" Maximum="100" Value="35" />
```

## 常用属性 / 事件 / 样式类

- `Minimum`、`Maximum`、`Value`：数值区间和当前值。
- `Orientation`：横向或纵向。
- `Classes="luna-capsule"`：胶囊样式。
- `IsEnabled`：控制禁用态。

## 配套类型 / 组合方式

- sample 中演示了基础、带标签、步进和禁用态。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SliderDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档只覆盖主题文件中已声明的 `luna-capsule` 变体。
