# ImageViewer 图片预览

点击缩略图后查看大图，或在图片集合中切换浏览。

## 何时使用

- 点击缩略图后查看大图，或在图片集合中切换浏览。

## 当前实现方式

- Luna 自定义控件 `ImageViewer`，样式位于 `src/Luna.Mobile/Themes/Controls/ImageViewer.axaml`。

## 最小示例

```xml
<luna:ImageViewer Name="PART_Viewer" />
```

## 常用属性 / 事件 / 样式类

- `Images`：图片列表。
- `CurrentIndex`：当前图片索引。
- `IsOpen`：是否打开预览。
- `ShowIndex`：是否显示索引。
- `ShowDelete`：是否显示删除操作。
- 事件 `Closed`、`DeleteRequested`。

## 配套类型 / 组合方式

- sample 中通过按钮点击在 code-behind 打开不同预览模式。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/ImageViewerDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档重点覆盖宿主控件和打开方式，不扩展未展示的手势细节。
