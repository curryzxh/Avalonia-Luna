# Indexes 索引

通过字母或分组索引定位大量列表内容，例如城市或联系人。

## 何时使用

- 通过字母或分组索引定位大量列表内容，例如城市或联系人。

## 当前实现方式

- Luna 自定义控件 `Indexes`，配套锚点为 `IndexesAnchor`，样式位于 `src/Luna.Mobile/Themes/Controls/Indexes.axaml`。

## 最小示例

```xml
<luna:Indexes Sticky="True" />
```

## 常用属性 / 事件 / 样式类

- `IndexList`：可用索引集合。
- `Sticky`：是否显示吸顶分组头。
- `StickyOffset`：吸顶偏移。
- `CurrentIndex`：当前高亮索引。
- 事件 `CurrentIndexChanged`、`IndexSelected`：响应索引切换。

## 配套类型 / 组合方式

- 通常与 `IndexesAnchor`、`CellGroup`、`Cell` 组合使用。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/IndexesDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档以 sample 中的静态城市列表为准。
