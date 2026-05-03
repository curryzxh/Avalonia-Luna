# Cascader 级联选择器

从多层级树形数据中逐级选择结果，例如地址或分类。

## 何时使用

- 从多层级树形数据中逐级选择结果，例如地址或分类。

## 当前实现方式

- 由 `CascaderHost`、`CascaderOptions` 和 `CascaderOption` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Cascader.axaml`。

## 最小示例

```csharp
var options = new CascaderOptions
{
    Title = "选择地址",
    Options = new[] { new CascaderOption { Label = "浙江省", Value = "zj" } }
};
```

## 常用属性 / 事件 / 样式类

- `Title`、`Placeholder`：标题和占位文案。
- `Options`：树形选项数据。
- `Theme`：面板主题。
- `CheckStrictly`：是否允许父节点直接选中。
- `SubTitles`：每一级辅助标题。
- 事件 `Changed`、`Picked`、`Closed`。

## 配套类型 / 组合方式

- 页面里先放 `CascaderHost`，再由 ViewModel 或 code-behind 请求打开。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/CascaderDemoView.axaml`

## 已知限制 / 注意事项

- 文档只覆盖 sample 中已演示的地址、分类和自定义高度场景。
