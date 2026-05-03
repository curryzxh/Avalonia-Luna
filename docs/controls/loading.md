# Loading 加载

表示内容正在加载、提交中或后台任务仍未完成。

## 何时使用

- 表示内容正在加载、提交中或后台任务仍未完成。

## 当前实现方式

- Luna 自定义控件 `Loading`，样式位于 `src/Luna.Mobile/Themes/Controls/Loading.axaml`。

## 最小示例

```xml
<luna:Loading Text="加载中..." />
```

## 常用属性 / 事件 / 样式类

- `Theme`：不同加载指示器主题。
- `IsLoading`：是否处于加载态。
- `Size`：指示器尺寸。
- `Text`：加载文案。
- `Layout`：水平或垂直排布。
- `ShowOverlay`：是否显示遮罩。

## 配套类型 / 组合方式

- 既可独立显示，也可作为内容容器上的覆盖层。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/LoadingDemoView.axaml`

## 已知限制 / 注意事项

- 当前样式能力以 sample 中的基础、文案、垂直布局和遮罩展示为准。
