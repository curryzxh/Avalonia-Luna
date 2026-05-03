# Footer 页脚

在页面底部展示版权、备案、帮助链接或品牌信息。

## 何时使用

- 在页面底部展示版权、备案、帮助链接或品牌信息。

## 当前实现方式

- Luna 自定义控件 `Footer`，配套类型为 `FooterLink`，样式位于 `src/Luna.Mobile/Themes/Controls/Footer.axaml`。

## 最小示例

```xml
<luna:Footer Text="Copyright © 2026 Luna" />
```

## 常用属性 / 事件 / 样式类

- `LogoIcon`、`LogoTitle`：品牌区域。
- `Text`：版权或说明文本。
- `Links`：链接集合。

## 配套类型 / 组合方式

- `FooterLink` 适合承载用户协议、隐私政策等轻量入口。

## 对应 Sample

- `samples/Luna.Mobile.Sample/Views/FooterDemoView.axaml`

## 已知限制 / 注意事项

- `FooterLink` 更偏数据项，不建议单独作为页面组件使用。
