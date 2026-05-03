# Collapse 折叠面板

把大段内容按区块折叠，减少页面初始信息密度。

## 何时使用

- 把大段内容按区块折叠，减少页面初始信息密度。

## 当前实现方式

- 基于 Avalonia 原生 `Expander` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/Collapse.axaml`。

## 最小示例

```xml
<Expander Header="标题" IsExpanded="True"><TextBlock Text="内容" /></Expander>
```

## 常用属性 / 事件 / 样式类

- `Header`：面板标题区域。
- `IsExpanded`：是否展开。
- `Classes="card"`：卡片样式。
- `Classes="with-action"`：带操作区域样式。

## 配套类型 / 组合方式

- 适合 FAQ、设置说明、表单高级项等场景。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/CollapseDemoView.axaml`

## 已知限制 / 注意事项

- 当前样式语义依赖 `Expander` 原生结构和 Header 内容组合。
