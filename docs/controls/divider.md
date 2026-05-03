# Divider 分割线

分隔同一区域内的内容块、列表节或操作区。

## 何时使用

- 分隔同一区域内的内容块、列表节或操作区。

## 当前实现方式

- Luna 自定义控件，类型为 `Luna.Mobile.Controls.Divider`，样式位于 `src/Luna.Mobile/Themes/Controls/Divider.axaml`。

## 最小示例

```xml
<luna:Divider Content="文字信息" Align="Left" />
```

## 常用属性 / 事件 / 样式类

- `Layout`：`Horizontal` / `Vertical`。
- `Align`：带文案时控制文本对齐。
- `Dashed`：切换实线和虚线。
- `LineThickness`、`LineLength`：控制线条粗细和长度。

## 配套类型 / 组合方式

- 既可以做纯分割线，也可以带中间文案。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/DividerDemoView.axaml`

## 已知限制 / 注意事项

- 竖向分割线通常要显式设置 `LineLength`。
