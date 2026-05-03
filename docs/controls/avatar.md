# Avatar 头像

展示用户头像、名称首字母或占位图标。

## 何时使用

- 展示用户头像、名称首字母或占位图标。

## 当前实现方式

- Luna 自定义控件 `Avatar`，样式位于 `src/Luna.Mobile/Themes/Controls/Avatar.axaml`。

## 最小示例

```xml
<luna:Avatar Shape="Round">A</luna:Avatar>
```

## 常用属性 / 事件 / 样式类

- `Source`：头像图片地址。
- `Icon`：图标占位。
- `Shape`：头像形状。
- `Size`：头像尺寸。

## 配套类型 / 组合方式

- 没有图片时，可退化为文字头像或图标头像。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/AvatarDemoView.axaml`

## 已知限制 / 注意事项

- 文档以 sample 中的图片、字母和图标三类展示为准。
