# Rate 评分

收集星级评分、满意度评价或展示已有评分结果。

## 何时使用

- 收集星级评分、满意度评价或展示已有评分结果。

## 当前实现方式

- Luna 自定义控件 `Rate`，样式位于 `src/Luna.Mobile/Themes/Controls/Rate.axaml`。

## 最小示例

```xml
<luna:Rate Value="3" />
```

## 常用属性 / 事件 / 样式类

- `Count`：图标个数。
- `Value`：当前分值。
- `AllowHalf`：是否允许半选。
- `AllowClear`：是否允许点当前值清空。
- `ReadOnly`：只展示。
- 事件 `ValueChanged`。

## 配套类型 / 组合方式

- 也可以自定义图标几何和前后景画刷。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/RateDemoView.axaml`

## 已知限制 / 注意事项

- 评分文案展示能力以 sample 中的 `Texts` 数组映射为准。
