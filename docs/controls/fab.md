# Fab 悬浮按钮

承载页面中的高优先级快捷操作，例如新增、发布。

## 何时使用

- 承载页面中的高优先级快捷操作，例如新增、发布。

## 当前实现方式

- Luna 自定义控件 `Fab`，继承自 `Button`，样式位于 `src/Luna.Mobile/Themes/Controls/Fab.axaml`。

## 最小示例

```xml
<luna:Fab Text="发布" />
```

## 常用属性 / 事件 / 样式类

- `Text`：按钮文案。
- `Icon`：图标内容。
- `DragMode`：控制是否允许拖拽。
- `HasText`、`HasIcon`：运行时派生状态。

## 配套类型 / 组合方式

- 既可以只放图标，也可以图标加文字。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/FabDemoView.axaml`

## 已知限制 / 注意事项

- 拖拽和停靠行为以当前 sample 为准。
