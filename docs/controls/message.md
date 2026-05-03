# Message 消息通知

在页面顶部展示不打断流程的轻量消息、状态提示或可关闭通知。

## 何时使用

- 在页面顶部展示不打断流程的轻量消息、状态提示或可关闭通知。

## 当前实现方式

- 由静态入口 `Message`、宿主 `MessageHost`、消息卡片 `MessageCard` 和 `MessageOptions` 组成，样式位于 `src/Luna.Mobile/Themes/Controls/Message.axaml`。

## 最小示例

```csharp
Message.Info(new MessageOptions
{
    Content = "这是一条普通消息通知",
    ShowIcon = true
});
```

## 常用属性 / 事件 / 样式类

- `Content`：通知文本。
- `Theme`：`Info`、`Success`、`Warning`、`Error`。
- `Duration`：持续时间。
- `ShowIcon`、`CloseBtn`：图标和关闭按钮。
- `Link`：右侧链接文案。
- 静态入口：`Info`、`Success`、`Warning`、`Error`、`CloseAll()`。

## 配套类型 / 组合方式

- `MessageCard` 适合直接作为单条通知卡片使用，`MessageHost` 负责全局堆叠展示。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/MessageDemoView.axaml`

## 已知限制 / 注意事项

- 页面里必须放置 `MessageHost`，静态入口才会生效。
