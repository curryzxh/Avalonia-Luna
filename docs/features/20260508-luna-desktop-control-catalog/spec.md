# Luna.Desktop 控件库与桌面示例工作台需求说明

- 状态：Draft
- 负责人：
- 影响项目：`src/Luna.Desktop/`、`samples/Luna.Desktop.Sample/`、`docs/`
- 更新时间：2026-05-08

## 背景

Luna 目标是建设一套面向 Avalonia 的统一控件体系。当前移动端已经具备较完整的控件、主题 token、示例页面和文档索引，桌面端还停留在最小可运行骨架。

现状如下：

- `Luna.Mobile` 已包含多个控件类、`Themes/Tokens/`、`Themes/Controls/`、`Themes/Index.axaml`、`Themes/Generic.axaml`、浅色/深色主题和完整移动示例目录。
- `Luna.Desktop` 当前只有 `DesktopBadge` 与 `Themes/Generic.axaml`，主题资源仍是单文件样式，且存在硬编码颜色。
- `Luna.Desktop.Sample` 当前只有 `MainWindow` 展示 `DesktopBadge`，尚未建立控件分类、搜索、详情页和主题切换。
- `AvaloniaSample.Desktop` 已具备较完整的桌面示例框架，包括 `SampleCatalog`、`ControlSampleCatalog`、`SampleNavigationItem`、`SampleGroup`、`MainWindowViewModel`、`MainWindow.axaml` 和 `ViewLocator`。

## 目标

- 为 `Luna.Desktop` 建立可持续扩展的控件库建设路线。
- 明确桌面端主题 token、控件样式、控件 API 和示例验证之间的关系。
- 为 `Luna.Desktop.Sample` 规划一个可扩展的桌面控件示例工作台。
- 首批覆盖桌面端最常用控件示例，支撑后续逐个控件落地。
- 让桌面端建设能复用 `Luna.Mobile` 的经验，同时保留桌面端交互和视觉密度差异。

## 非目标

- 本需求不直接实现控件代码。
- 本需求不调整 `Luna.Mobile` 已有控件行为。
- 本需求不复制 `AvaloniaSample.Desktop` 的全部官方示例。
- 本需求不在首版引入 Prism、ReactiveUI 或新的导航框架。
- 本需求不定义最终包发布策略，只规划控件与示例建设路径。

## 用户场景

- 作为控件库开发者，我需要在 `Luna.Desktop.Sample` 中快速找到某个控件示例，验证样式、状态和绑定行为。
- 作为主题维护者，我需要通过桌面示例切换浅色/深色主题，检查 token 和 ControlTheme 是否一致。
- 作为控件 API 设计者，我需要对照 `Luna.Mobile` 的控件语义，判断桌面端是复用原生控件皮肤化，还是新增 Luna 自定义控件。
- 作为评审者，我需要通过统一示例结构检查控件是否覆盖默认、悬停、按下、焦点、禁用、选中、错误和加载状态。

## 功能需求

- `Luna.Desktop` 应建立独立桌面主题入口，规划 `Themes/Index.axaml`、token 资源和控件主题资源。
- 桌面 token 应使用 `Luna.*` 命名，优先复用移动端 token 语义，但重新设计桌面尺寸、密度和键鼠交互资源。
- 原生 Avalonia 控件语义足够时，优先以 ControlTheme 和样式资源实现 Luna 桌面外观。
- TDesign 或 Luna 语义不能由原生控件表达时，再新增 Luna 桌面自定义控件。
- 控件示例应按分类组织，支持搜索、示例选择、详情展示和主题切换。
- 示例详情应展示最小用法、常用状态、绑定行为、禁用态、错误态和必要的桌面交互状态。

## 首批控件范围

首批桌面示例覆盖以下控件或能力：

- `DesktopBadge`
- Button
- TextBox
- CheckBox
- RadioButton
- ToggleSwitch
- Slider
- Progress
- Tag
- Avatar
- Empty
- Dialog 或 Notification 类反馈示例

## 桌面控件完整 backlog

本 backlog 基于 `docs/guides/tdesign-desktop-sample-authoring-guide.md`、`tdesign-mcp-server` React 组件列表和当前 `Luna.Desktop.Sample` catalog 生成，用于将后续控件实现拆分给多个 Agent 并行推进。

### P0 优先补齐

P0 控件优先用于建立 Luna.Desktop 的基础控件面和示例覆盖，适合直接分派实现。

- Base：Icon、Link、Typography、ImageViewer。
- Form：Textarea、InputNumber、Select、AutoComplete、DatePicker、TimePicker、Upload、Form。
- Data：Card、Table、Tooltip、Skeleton、Collapse、Timeline、Statistic、Calendar。
- Message：Alert、Dialog、Drawer、Message、Loading、Popup、Popconfirm。
- Navigation：Tabs、Menu、Dropdown、Pagination、Breadcrumb、Steps。
- Layout：Divider、Descriptions、Grid、Space、Layout。

### P1 复杂控件

P1 控件实现前应先查 TDesign React 示例源码和 Ursa 相邻实现，明确控件 API、弹层、键盘行为、数据结构和验收场景。

- Form：Cascader、TreeSelect、Transfer、ColorPicker、Rate、InputAdornment。
- Data：Tree、List、Comment、QRCode、Watermark、BackTop、Image。
- Message：Swiper。
- Navigation：Anchor、Affix、StickyTool。
- Layout：Guide。

### P2 组合型能力

P2 控件更适合在 Input、Tag、Select、Popup 等基础能力稳定后再统一实现。

- Data：RangeInput、SelectInput、TagInput。

## 已有示例深化需求

当前已登记示例包括 Button、DesktopBadge、Input、CheckBox、Radio、Switch、Slider、Progress、Tag、Avatar、Empty、Notification。后续需要逐个对齐 TDesign React 示例分组和状态覆盖。

- Button：补真实 loading spinner、统一 icon 体系、dashed 边框模板和焦点态。
- DesktopBadge：对齐 TDesign Badge 的 count、dot、maxCount、offset、shape。
- Input：补 clearable、prefix/suffix、status、tips、size、maxlength。
- CheckBox / Radio：补 group、disabled、indeterminate、option 列表和键盘行为。
- Switch：补 size、label、loading、disabled。
- Slider：补 range、marks、tooltip、step、disabled。
- Progress：补 line/circle/dashboard、status、label、theme。
- Tag：补 variant、size、shape、closable、checkable。
- Avatar：补 image、icon、size、shape、group、badge 组合。
- Empty：补 image、description、action 和不同业务场景。
- Notification：补 service/host、position、duration、close、theme 和动画。

## 示例工作台需求

- 工作台采用单窗口桌面布局，顶部为标题和主题切换，左侧为分类、搜索和示例列表，右侧为示例详情。
- 工作台 ViewModel 负责搜索、分类过滤、选中示例和主题切换。
- 示例内容以 ViewModel 驱动，通过 `ViewLocator` 或 DataTemplate 映射到对应 View。
- 示例目录建议采用 `Models/`、`ViewModels/Samples/`、`Views/Samples/` 分层。
- 每个示例页面只负责展示控件行为，不承载控件库公共逻辑。

## 验收标准

- [ ] `docs/features/20260508-luna-desktop-control-catalog/` 下包含 `spec.md` 和 `plan.md`。
- [ ] `docs/features/index.md` 登记该 feature。
- [ ] 后续实现 `Luna.Desktop.Sample` 后，桌面示例工作台可启动并展示 catalog shell。
- [ ] 后续实现后，搜索控件名或分类名能过滤示例列表。
- [ ] 后续实现后，浅色/深色主题切换能作用于窗口与当前示例。
- [ ] 后续实现后，首批控件示例至少覆盖默认、禁用和关键交互状态。
- [ ] 每个新增控件示例均接入 `ControlSampleCatalog.CreateSamples()`，并可从左侧 catalog 进入。
- [ ] 每个新增控件示例记录 TDesign React 来源、MCP 查询项、Avalonia 映射方式、Ursa 参考和不支持项。
- [ ] 后续实现后，`dotnet build Luna.sln --no-restore` 通过。

## 待确认问题

- 桌面端是否需要与移动端共享一个基础 token 项目，还是继续保留 `Luna.Desktop` 与 `Luna.Mobile` 各自内置 token。
- 桌面控件命名是否统一使用 `Luna` 前缀，还是对原生控件皮肤化场景只提供样式类和 ControlTheme。
- 示例工作台是否需要展示代码片段，首版默认只展示可交互控件和说明。
