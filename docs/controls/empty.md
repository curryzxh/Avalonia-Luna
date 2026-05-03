# Empty 空状态

列表无数据、加载失败或无搜索结果时给出占位说明。

## 何时使用

- 列表无数据、加载失败或无搜索结果时给出占位说明。

## 当前实现方式

- Luna 自定义控件 `Empty`，样式位于 `src/Luna.Mobile/Themes/Controls/Empty.axaml`。

## 最小示例

```xml
<luna:Empty Title="暂无数据" Description="可以稍后再试" />
```

## 常用属性 / 事件 / 样式类

- `Title`：主标题。
- `Description`：说明文案。
- `Icon`：自定义图标区域。
- `Action`：操作按钮或自定义动作内容。

## 配套类型 / 组合方式

- 不传 `Icon` 时会使用默认占位图形。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/EmptyDemoView.axaml`

## 已知限制 / 注意事项

- 当前文档以无数据、无结果、网络异常等 sample 为准。
