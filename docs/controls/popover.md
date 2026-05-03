# Popover 弹出气泡

围绕某个按钮或元素展示短提示、选项菜单或解释性内容。

## 何时使用

- 围绕某个按钮或元素展示短提示、选项菜单或解释性内容。

## 当前实现方式

- Luna 自定义控件 `Popover`，样式位于 `src/Luna.Mobile/Themes/Controls/Popover.axaml`。

## 最小示例

```xml
<luna:Popover Placement="Top" Theme="Dark" PopupContent="弹出气泡内容"><Button Content="带箭头" /></luna:Popover>
```

## 常用属性 / 事件 / 样式类

- `PopupContent`：弹层内容。
- `Placement`：弹出位置。
- `ShowArrow`：是否显示箭头。
- `IsOpen`：是否打开。
- `Theme`：主题色。

## 配套类型 / 组合方式

- 也可以通过 `Popover.PopupContent` 放一个完整 `StackPanel` 做自定义菜单。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/PopoverDemoView.axaml`

## 已知限制 / 注意事项

- 当前 sample 覆盖了上下左右和角落位置。
