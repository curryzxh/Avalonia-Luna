# Badge 徽标

给按钮、图标、列表项增加数量提示或小红点提示。

## 何时使用

- 给按钮、图标、列表项增加数量提示或小红点提示。

## 当前实现方式

- Luna 自定义控件 `Badge`，样式位于 `src/Luna.Mobile/Themes/Controls/Badge.axaml`。

## 最小示例

```xml
<luna:Badge Count="8"><Button Content="按钮" /></luna:Badge>
```

## 常用属性 / 事件 / 样式类

- `Count`：数字徽标内容。
- `OverflowCount`：超过上限后的显示阈值。
- `Dot`：是否显示圆点。
- `Shape`：徽标形状。
- `Placement`、`Offset`：相对宿主的位置和偏移。

## 配套类型 / 组合方式

- 通常把任何可视元素作为 `Badge` 内容包进去。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/BadgeDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档只覆盖圆点、数字和位置偏移这些已演示能力。
