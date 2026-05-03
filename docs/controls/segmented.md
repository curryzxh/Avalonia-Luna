# Segmented 分段控制器

在少量互斥选项之间快速切换视图或过滤条件。

## 何时使用

- 在少量互斥选项之间快速切换视图或过滤条件。

## 当前实现方式

- Luna 自定义控件 `Segmented`，选项模型为 `SegmentedItem`，样式位于 `src/Luna.Mobile/Themes/Controls/Segmented.axaml`。

## 最小示例

```xml
<luna:Segmented SelectedIndex="0"><luna:SegmentedItem Content="选项" Value="0" /></luna:Segmented>
```

## 常用属性 / 事件 / 样式类

- `SelectedIndex`、`SelectedValue`：当前选中项。
- `Block`：是否拉伸占满可用宽度。
- `SegmentedItem.Content`、`Value`、`Icon`、`IsEnabled`：单个分段项配置。
- 事件 `SelectionChanged`。

## 配套类型 / 组合方式

- 适合替代页签中的轻量切换，不承担复杂内容管理。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/SegmentedDemoView.axaml`

## 已知限制 / 注意事项

- 每个选项最终会渲染成内部按钮，禁用项行为以当前主题样式为准。
