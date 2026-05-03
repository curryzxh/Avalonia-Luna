# Tag 标签

展示状态、分类、轻量属性，或作为可选择的小型交互单元。

## 何时使用

- 展示状态、分类、轻量属性，或作为可选择的小型交互单元。

## 当前实现方式

- Luna 自定义控件 `Tag`，配套可选类型为 `CheckTag`，样式位于 `src/Luna.Mobile/Themes/Controls/Tag.axaml`。

## 最小示例

```xml
<luna:Tag Variant="Light">标签文字</luna:Tag>
```

## 常用属性 / 事件 / 样式类

- `Theme`：语义主题。
- `Variant`：如 `Light`、`Outline`。
- `Shape`：形状。
- `Size`：尺寸。
- `IsClosable`：是否显示关闭入口。
- 事件 `CloseRequested`。

## 配套类型 / 组合方式

- `CheckTag` 继承自 `ToggleButton`，适合筛选条件的选中/取消。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/TagDemoView.axaml`

## 已知限制 / 注意事项

- 文档以 sample 中的轻量、描边、圆角、Mark 等样式组合为准。
