# Progress 进度条

展示任务完成进度、配额使用情况或加载阶段。

## 何时使用

- 展示任务完成进度、配额使用情况或加载阶段。

## 当前实现方式

- 基于 Avalonia 原生 `ProgressBar` 的 Luna 皮肤化组件，样式入口在 `src/Luna.Mobile/Themes/Controls/Progress.axaml`。

## 最小示例

```xml
<ProgressBar Minimum="0" Maximum="100" Value="35" />
```

## 常用属性 / 事件 / 样式类

- `Minimum`、`Maximum`、`Value`：进度范围和值。
- `Classes="plump"`：粗条样式。
- `Classes`：主题支持 `warning`、`error`、`success`、`active`。
- `IsEnabled`：禁用态。

## 配套类型 / 组合方式

- sample 中同时演示了细线和饱满进度条两种外观。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/ProgressDemoView.axaml`

## 已知限制 / 注意事项

- 当前 `Progress` 是 `ProgressBar` 的皮肤语义。
