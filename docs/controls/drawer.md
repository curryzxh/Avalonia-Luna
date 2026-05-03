# Drawer 抽屉

从页面左右边缘滑出补充信息、筛选面板或二级导航。

## 何时使用

- 从页面左右边缘滑出补充信息、筛选面板或二级导航。

## 当前实现方式

- 由静态入口 `Drawer`、宿主 `DrawerHost` 和配置对象 `DrawerOptions` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Drawer.axaml`。

## 最小示例

```csharp
Drawer.Show(new DrawerOptions
{
    Title = "筛选",
    Placement = DrawerPlacement.Right
});
```

## 常用属性 / 事件 / 样式类

- `DrawerOptions.Title`：标题。
- `DrawerOptions.Content`：抽屉主体内容。
- `DrawerOptions.Placement`：`Left` / `Right`。
- `ShowOverlay`、`CloseOnOverlayClick`、`ShowCloseButton`：控制遮罩与关闭行为。
- `Width`：抽屉宽度。

## 配套类型 / 组合方式

- 页面里先放一个 `DrawerHost`，静态 `Drawer.Show()` 才能找到当前宿主。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/DrawerDemoView.axaml`

## 已知限制 / 注意事项

- 当前实现只支持左右两侧滑出。
