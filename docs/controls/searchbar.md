# SearchBar 搜索框

用于搜索关键词输入，或在列表页做轻量过滤。

## 何时使用

- 用于搜索关键词输入，或在列表页做轻量过滤。

## 当前实现方式

- Luna 自定义控件 `SearchBar`，样式位于 `src/Luna.Mobile/Themes/Controls/SearchBar.axaml`。

## 最小示例

```xml
<luna:SearchBar Placeholder="搜索预设文案" />
```

## 常用属性 / 事件 / 样式类

- `Text`：当前搜索词。
- `Placeholder`：占位文案。
- `IsClearable`：是否显示清空按钮。
- `ShowCancelButton`、`ShowCancelButtonOnFocus`：取消按钮策略。
- `CancelText`：取消按钮文案。
- 事件 `SearchRequested`、`CancelRequested`、`Cleared`。

## 配套类型 / 组合方式

- 适合直接放在列表页头部，和外层筛选状态联动。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SearchDemoView.axaml`

## 已知限制 / 注意事项

- `SearchDemoViewModel` 当前为空，示例主要来自 AXAML 页面布局。
