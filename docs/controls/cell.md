# Cell 单元格

以列表行形式展示标题、描述、辅助信息和右侧操作。

## 何时使用

- 以列表行形式展示标题、描述、辅助信息和右侧操作。

## 当前实现方式

- 由 `Cell` 与 `CellGroup` 组合而成，样式位于 `src/Luna.Mobile/Themes/Controls/Cell.axaml`。

## 最小示例

```xml
<luna:CellGroup>
  <luna:Cell Title="单行标题" RightText="辅助信息" />
</luna:CellGroup>
```

## 常用属性 / 事件 / 样式类

- `Cell.Title`：主标题。
- `Cell.Description`：描述文案。
- `Cell.RightText`：右侧辅助文案。
- `Cell.LeftContent`、`RightContent`：左右自定义内容。
- `Cell.ShowArrow`：是否显示右侧箭头。
- `CellGroup.Title`、`Summary`：分组标题和说明。

## 配套类型 / 组合方式

- 可把 `ToggleSwitch`、徽标、自定义图标和图片塞进左右插槽。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/CellDemoView.axaml`

## 已知限制 / 注意事项

- `Cell` 继承自 `Button`，因此也可以直接响应点击。
