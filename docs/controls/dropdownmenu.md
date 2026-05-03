# DropdownMenu 下拉菜单

把页面级筛选条件并列展示在顶部，通过点击展开面板选择值。

## 何时使用

- 把页面级筛选条件并列展示在顶部，通过点击展开面板选择值。

## 当前实现方式

- Luna 自定义控件 `DropdownMenu`，配套类型包括 `DropdownMenuItem` 和 `DropdownMenuOption`，样式位于 `src/Luna.Mobile/Themes/Controls/DropdownMenu.axaml`。

## 最小示例

```xml
<ContentControl Name="SingleDemoHost" />
```

## 常用属性 / 事件 / 样式类

- `Items`：菜单项集合。
- `DropdownMenuItem.Options`：候选值列表。
- `Value` / `SelectedValues`：单选或多选结果。
- `Multiple`：是否多选。
- `Direction`：展开方向。
- 事件 `ValueChanged`。

## 配套类型 / 组合方式

- sample 通过 code-behind 在多个 `ContentControl` 宿主里动态构造示例。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/DropdownMenuDemoView.axaml`

## 已知限制 / 注意事项

- 当前示例以动态创建控件为主，不把内部宿主构造过程写成固定 API。
