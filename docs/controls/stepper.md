# Stepper 步进器

用加减按钮调整整数或数值范围，例如购买数量。

## 何时使用

- 用加减按钮调整整数或数值范围，例如购买数量。

## 当前实现方式

- Luna 自定义控件 `Stepper`，样式位于 `src/Luna.Mobile/Themes/Controls/Stepper.axaml`。

## 最小示例

```xml
<luna:Stepper Theme="Filled" Minimum="0" Maximum="10" Value="3" />
```

## 常用属性 / 事件 / 样式类

- `Value`：当前值。
- `Minimum`、`Maximum`、`Step`：范围和步长。
- `Theme`：如 `Filled`、`Outline`。
- `Size`：尺寸。
- `IsEditable`：是否允许直接输入。
- 事件 `ValueChanged`、`Overlimit`。

## 配套类型 / 组合方式

- 适合与商品卡片、订单行、筛选表单组合。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/StepperDemoView.axaml`

## 已知限制 / 注意事项

- 超过范围时的体验以当前 `Overlimit` 事件和主题样式为准。
