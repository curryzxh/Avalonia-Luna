# Watermark 水印

在容器背景中重复显示文本或图片水印，表达版权或来源。

## 何时使用

- 在容器背景中重复显示文本或图片水印，表达版权或来源。

## 当前实现方式

- Luna 自定义控件 `Watermark`，配套数据项为 `WatermarkItem`，样式位于 `src/Luna.Mobile/Themes/Controls/Watermark.axaml`。

## 最小示例

```xml
<luna:Watermark TileWidth="68" />
```

## 常用属性 / 事件 / 样式类

- `Items`：水印项集合。
- `TileWidth`、`TileHeight`：单元尺寸。
- `HorizontalSpacing`、`VerticalSpacing`：间距。
- `Angle`、`Alpha`：旋转和透明度。
- `Movable`、`AnimationDuration`：是否移动及动画时长。

## 配套类型 / 组合方式

- 既可以做纯文字水印，也可以做图片 Logo 水印。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/WatermarkDemoView.axaml`

## 已知限制 / 注意事项

- 当前 sample 中的图片水印使用外部资源链接。
