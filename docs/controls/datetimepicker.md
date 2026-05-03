# DateTimePicker 时间选择器

选择日期、时间或日期时间，且需要范围限制、步进和格式化输出。

## 何时使用

- 选择日期、时间或日期时间，且需要范围限制、步进和格式化输出。

## 当前实现方式

- 由 `DateTimePickerHost`、`DateTimePickerOptions` 和步进配置组成，样式位于 `src/Luna.Mobile/Themes/Controls/DateTimePicker.axaml`。

## 最小示例

```csharp
var options = new DateTimePickerOptions
{
    Title = "选择日期",
    Mode = DateTimePickerMode.Date,
    Value = DateTime.Today
};
```

## 常用属性 / 事件 / 样式类

- `Mode`：如 `Date`、`Minute`、`Second`。
- `Value`：当前值。
- `Start`、`End`：可选范围。
- `Format`：输出格式字符串。
- `Steps`：分钟等列的步进配置。
- 事件 `Picked`、`Confirmed`、`Closed`。

## 配套类型 / 组合方式

- 页面里先放置 `DateTimePickerHost`，确认后再处理结果。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/DateTimePickerDemoView.axaml`

## 已知限制 / 注意事项

- 文档以 sample 已演示的日期、分钟、范围和步进场景为准。
