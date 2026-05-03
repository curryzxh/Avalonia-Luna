# PullDownRefresh 下拉刷新

在可滚动内容顶部下拉触发刷新。

## 何时使用

- 在可滚动内容顶部下拉触发刷新。

## 当前实现方式

- Luna 自定义控件 `PullDownRefresh`，样式位于 `src/Luna.Mobile/Themes/Controls/PullDownRefresh.axaml`。

## 最小示例

```xml
<luna:PullDownRefresh RefreshRequested="OnRefreshRequested">...</luna:PullDownRefresh>
```

## 常用属性 / 事件 / 样式类

- `IsRefreshing`：当前是否刷新中。
- `Threshold`：触发阈值。
- `IndicatorHeight`：指示器高度。
- `MaxPullDistance`：最大下拉距离。
- `PullText`、`ReleaseText`、`RefreshingText`、`CompleteText`：各状态文案。
- 事件 `RefreshRequested`。

## 配套类型 / 组合方式

- 刷新结束后需要由调用方把 `IsRefreshing` 复位。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/PullDownRefreshDemoView.axaml`

## 已知限制 / 注意事项

- 当前 sample 主要演示顶部下拉和骨架内容刷新。
