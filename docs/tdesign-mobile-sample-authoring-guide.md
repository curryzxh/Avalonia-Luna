# TDesign Mobile 示例复刻指南

本文记录在 `Luna.Mobile.Sample` 中复刻 TDesign Mobile React 示例页的实践约定，供后续 Agent 继续补充组件示例时参考。

## 信息来源

优先读取 TDesign Mobile React 官方示例源码，而不是只看截图或页面 DOM。

- 示例站根路径：`https://static.tdesign.tencent.com/mobile-react/mobile.html`
- 组件示例路径：`https://static.tdesign.tencent.com/mobile-react/mobile.html#/{path}`
- 官方源码路径：`https://github.com/Tencent/tdesign-mobile-react/tree/develop/src/{component}/_example`

推荐读取：

- `src/{component}/_example/index.tsx`：确认页面标题、说明、DemoBlock 顺序。
- `src/{component}/_example/*.tsx`：确认每个分组中的具体示例。
- `src/{component}/_example/style/index.less`：确认行间距、横向排列、背景色和特殊区域。

示例：

```bash
curl -L 'https://api.github.com/repos/Tencent/tdesign-mobile-react/contents/src/button/_example?ref=develop'
curl -L https://raw.githubusercontent.com/Tencent/tdesign-mobile-react/develop/src/button/_example/index.tsx
```

## 复刻流程

1. 从目录数据中找到组件 `path`，例如 `Button 按钮` 对应 `/button`。
2. 读取官方 `_example/index.tsx`，整理出页面结构：
   - `TDemoHeader title/summary`
   - `TDemoBlock title/summary`
   - 每个 block 引用的子示例组件
3. 读取子示例文件，转换为 Avalonia 原生控件或 Luna 控件。
4. 在 `samples/Luna.Mobile.Sample/Views` 下新增 `{Component}DemoView.axaml` 和 code-behind。
5. 在 `MainView.axaml.cs` 的 path 分发中接入新页面。
6. 如原生控件缺少必要 style class，在 `src/Luna.Mobile/Themes/Controls/{Control}.axaml` 中补样式。
7. 运行 `dotnet build Luna.sln --no-restore` 验证。

## 映射原则

- 保持 TDesign 示例的信息架构：标题、说明、分组顺序、示例文案尽量一致。
- 保持 Avalonia 语义：用 `Button`、`ToggleSwitch` 等原生控件表达，不为了示例强行实现完整 TDesign API。
- 样式优先用 `Luna.*` token；只有 TDesign 示例本身需要特殊背景时才允许少量硬编码色值，例如幽灵按钮黑底区域。
- 示例页应可从目录进入，并提供返回目录的轻量交互。
- 目录项的 `path` 先作为页面分发键；不要在未建立导航框架前引入复杂路由。

## 不支持能力的处理

如果 Avalonia 原生控件暂不支持 TDesign 示例能力，先占位，不在示例任务中扩大为自定义控件开发。

已采用的占位策略：

- `Button loading`：用“加载中”文本占位，不新增 loading 状态控件。
- `Switch loading`：用“加载中”文本 + disabled `ToggleSwitch` 占位。
- `Switch label`：用相邻文本或图标占位，不改造 `ToggleSwitch` 内部模板。
- `Switch custom color`：用相邻“占位”文本表达语义；真实 checked color 后续进入模板或控件级 token 任务。

占位必须保留官方示例标题和位置，便于后续替换为真实实现。

## 示例页结构约定

新增示例页建议保持以下结构：

- 顶部 56px 返回栏。
- `ScrollViewer` 包裹整页。
- Header 区展示组件中文标题和用途说明。
- DemoBlock 使用 `StackPanel` 分组：
  - 一级标题如 `01 组件类型`、`02 状态`、`03 组件样式`。
  - 二级说明如 `基础按钮`、`加载状态`。
  - 示例主体尽量复刻 TDesign 行排列、间距和背景。
- code-behind 只保留 `BackRequested` 事件，不承载业务逻辑。

## 已完成组件经验

### Button

官方分组：

- `01 组件类型`：基础按钮、图标按钮、幽灵按钮、组合按钮、通栏按钮。
- `02 组件状态`：按钮禁用态。
- `03 组件样式`：按钮尺寸、按钮形状、按钮主题。

Avalonia 映射：

- `theme` 映射到 class：`primary`、`danger`、`light`、默认。
- `variant` 映射到 class：`outline`、`text`、默认填充。
- `size` 映射到 class：`large`、默认、`small`、`extra-small`。
- `shape` 映射到 class：`square`、`round`、`circle`、`rectangle`。
- `block` 使用 `HorizontalAlignment="Stretch"`。

### Switch

官方分组：

- `01 组件类型`：基础开关、带描述开关、自定义颜色开关。
- `02 状态`：加载状态、禁用状态。
- `03 组件样式`：开关尺寸。

Avalonia 映射：

- 基础能力使用 `ToggleSwitch`。
- 大小示例用 `large` / 默认 / `small` class 占位。
- loading、label、custom color 暂不改造模板，按“不支持能力的处理”占位。

## 验收清单

- 组件路径从目录可进入，例如 `/button`、`/switch`。
- 示例页标题、说明、分组顺序与 TDesign `_example/index.tsx` 一致。
- 不支持能力有明确占位，不静默删除示例。
- 新增样式优先引用 `Luna.*` token。
- `dotnet build samples/Luna.Mobile.Sample/Luna.Mobile.Sample.csproj --no-restore` 通过。
- `dotnet build Luna.sln --no-restore` 通过。
