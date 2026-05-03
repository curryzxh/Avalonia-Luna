# NoticeBar 公告栏

在页面局部持续展示公告、活动提示或系统广播。

## 何时使用

- 在页面局部持续展示公告、活动提示或系统广播。

## 当前实现方式

- Luna 自定义控件 `NoticeBar`，样式位于 `src/Luna.Mobile/Themes/Controls/NoticeBar.axaml`。

## 最小示例

```xml
<luna:NoticeBar Content="这是一条普通的通知消息" />
```

## 常用属性 / 事件 / 样式类

- `Content` / `ContentList`：单条或多条内容。
- `Theme`：公告栏主题。
- `ShowIcon`、`ShowClose`：前缀图标和关闭入口。
- `OperationText`：右侧操作文案。
- `Marquee`、`Direction`、`MarqueeSpeed`：滚动行为。
- 事件 `CloseRequested`、`SuffixRequested`。

## 配套类型 / 组合方式

- 适合横向滚动和竖向轮播两类公告展示。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/NoticeBarDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档只覆盖 sample 已演示的关闭、自定义右侧操作和 cover 样式场景。
