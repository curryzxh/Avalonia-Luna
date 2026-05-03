# Guide 引导

按步骤高亮页面重点区域，解释新功能、关键流程或复杂表单。

## 何时使用

- 按步骤高亮页面重点区域，解释新功能、关键流程或复杂表单。

## 当前实现方式

- Luna 自定义控件 `GuideHost`，配套步骤模型为 `GuideStep`，样式位于 `src/Luna.Mobile/Themes/Controls/Guide.axaml`。

## 最小示例

```xml
<luna:GuideHost Name="GuideHost" />
```

## 常用属性 / 事件 / 样式类

- `Steps`：引导步骤集合。
- `Mode`：引导模式。
- `ShowOverlay`：是否遮罩。
- `HideSkip`、`HideCounter`：是否隐藏跳过和计数。
- `BackText`、`SkipText`、`NextText`、`FinishText`：按钮文案。
- 事件 `CurrentChanged`、`SkipRequested`、`Finished`。

## 配套类型 / 组合方式

- 当前 Guide 需要在 code-behind 中提供目标控件引用和步骤集合。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/GuideDemoView.axaml`

## 已知限制 / 注意事项

- sample 演示了基础引导、无遮罩、弹窗、混合模式和自定义气泡。
