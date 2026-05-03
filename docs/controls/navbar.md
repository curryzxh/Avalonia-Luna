# NavBar 导航栏

作为页面顶部导航容器，承载标题、返回和左右操作区。

## 何时使用

- 作为页面顶部导航容器，承载标题、返回和左右操作区。

## 当前实现方式

- Luna 自定义控件 `NavBar`，样式位于 `src/Luna.Mobile/Themes/Controls/NavBar.axaml`。

## 最小示例

```xml
<luna:NavBar Title="标题文字" ShowBackButton="True" />
```

## 常用属性 / 事件 / 样式类

- `Title`：中间标题。
- `ShowBackButton`：是否显示默认返回按钮。
- `LeftContent`、`RightContent`：自定义左右区域。
- `ShowDivider`：是否显示底部分隔线。
- 事件 `BackRequested`：点击返回时触发。

## 配套类型 / 组合方式

- 当需要自定义按钮时，可在左右插槽中放普通 `Button`。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/NavBarDemoView.axaml`

## 已知限制 / 注意事项

- 控件只发出返回事件，真正的导航逻辑由调用方处理。
