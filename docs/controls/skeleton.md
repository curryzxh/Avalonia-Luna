# Skeleton 骨架屏

在数据尚未返回时展示占位结构，避免页面闪烁。

## 何时使用

- 在数据尚未返回时展示占位结构，避免页面闪烁。

## 当前实现方式

- Luna 自定义控件 `Skeleton`，样式位于 `src/Luna.Mobile/Themes/Controls/Skeleton.axaml`。

## 最小示例

```xml
<luna:Skeleton Theme="Text" Width="100" />
```

## 常用属性 / 事件 / 样式类

- `Theme`：预设占位形态。
- `Loading`：是否显示骨架。
- `Animation`：动画类型。
- `Rows`、`RowSpacing`：自定义行布局。

## 配套类型 / 组合方式

- 除了预设主题，也可以用行定义和块定义组合自定义骨架布局。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SkeletonDemoView.axaml`

## 已知限制 / 注意事项

- 复杂布局建议参考 sample 中的 cell/avatar/image 组合写法。
