# SwipeCell 滑动操作

在列表项上左滑或右滑露出快捷操作，例如删除或置顶。

## 何时使用

- 在列表项上左滑或右滑露出快捷操作，例如删除或置顶。

## 当前实现方式

- Luna 自定义控件 `SwipeCell`，样式位于 `src/Luna.Mobile/Themes/Controls/SwipeCell.axaml`。

## 最小示例

```xml
<luna:SwipeCell OpenMode="Right"><luna:SwipeCell.Right><Button Content="删除" /></luna:SwipeCell.Right><luna:Cell Title="左滑单操作" /></luna:SwipeCell>
```

## 常用属性 / 事件 / 样式类

- `Left`、`Right`：左右操作区内容。
- `OpenMode`：开启方向。
- `OpenThresholdRatio`：展开阈值比例。
- `HasLeft`、`HasRight`、`IsOpen`：运行时状态。
- 事件 `OpenModeChanged`。

## 配套类型 / 组合方式

- 通常把 `Cell` 或其他列表行控件作为主体内容放在中间。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SwipeCellDemoView.axaml`

## 已知限制 / 注意事项

- 操作按钮的颜色和尺寸主要由你放入的 `Button` 自己决定。
