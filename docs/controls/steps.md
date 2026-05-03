# Steps 步骤条

展示流程进度、任务分步状态或向导步骤。

## 何时使用

- 展示流程进度、任务分步状态或向导步骤。

## 当前实现方式

- Luna 自定义控件 `Steps`，步骤项为 `StepItem`，样式位于 `src/Luna.Mobile/Themes/Controls/Steps.axaml`。

## 最小示例

```xml
<luna:Steps Current="1">
  <luna:StepItem Title="步骤一" />
  <luna:StepItem Title="步骤二" />
</luna:Steps>
```

## 常用属性 / 事件 / 样式类

- `Current`：当前步骤值，可按索引或 `StepItem.Value` 匹配。
- `CurrentStatus`：当前步骤状态。
- `Layout`：`Horizontal` / `Vertical`。
- `StepTheme`：`Default` / `Dot`。
- `IsReadOnly`：只展示不允许点击切换。
- 事件 `CurrentChanged`：当前步骤变化时触发。

## 配套类型 / 组合方式

- `StepItem` 常用属性有 `Title`、`Content`、`Value`、`TitleRight`、`Extra`、`Icon`、`Status`。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/StepsDemoView.axaml`

## 已知限制 / 注意事项

- `StepItem.Status` 可以显式覆盖自动推导状态。
