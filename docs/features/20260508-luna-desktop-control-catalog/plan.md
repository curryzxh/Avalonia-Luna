# Luna.Desktop 控件库与桌面示例工作台实施方案

- 状态：Draft
- 负责人：
- 关联需求：[spec.md](spec.md)
- 更新时间：2026-05-08

## 总体方案

以 `Luna.Mobile` 的主题组织、控件分层和示例覆盖经验为基础，为 `Luna.Desktop` 建立独立桌面端建设路径。桌面端不复制移动交互，而是围绕键鼠效率、窗口缩放、焦点可见性、高密度信息和浅色/深色主题一致性设计。

`Luna.Desktop.Sample` 先建设示例工作台框架，再分阶段填入控件示例。工作台参考 `AvaloniaSample.Desktop` 的 catalog 模型和左右分栏布局，但只保留 Luna 控件库需要的能力。

## 改动范围

- `src/Luna.Desktop/`：规划主题 token、控件主题资源和后续控件类。
- `samples/Luna.Desktop.Sample/`：规划 catalog shell、ViewModel、Models 和示例页面组织。
- `docs/features/`：保存本次需求和技术方案。默认每个 feature 只维护 `spec.md` 与 `plan.md`，不额外生成任务、验收或过程记录文档。

## 桌面控件库分层

### 主题层

- 新增 `Themes/Index.axaml` 作为桌面主题资源入口。
- 规划 `Themes/Tokens/`，包含颜色、字号、间距、圆角、阴影、尺寸和动效。
- 规划 `Themes/Controls/`，每个控件一个主题文件，避免继续堆在 `Generic.axaml`。
- `Generic.axaml` 负责合并控件主题，不直接承载大量硬编码样式。

### 控件层

- 优先皮肤化 Avalonia 原生控件：Button、TextBox、CheckBox、RadioButton、ToggleSwitch、Slider、ProgressBar。
- 轻量语义控件再进入 Luna 自定义控件：Badge、Tag、Avatar、Empty。
- 反馈类控件后续按桌面语义设计：Dialog、Notification、Popover、Flyout 或 WindowNotificationManager 包装。
- 公共状态通过 Avalonia 属性、命令、事件、伪类和模板部件表达。

### 示例层

- 每个控件至少有一个 sample ViewModel 和一个 sample View。
- 示例覆盖默认用法、关键属性、绑定、禁用态和桌面交互状态。
- 示例数据内置，不依赖外部服务。

## Sample Catalog 设计

参考 `AvaloniaSample.Desktop`，但收敛到 Luna 需要的模型：

- `ControlCategory`：描述分类 key、显示名和说明。
- `SampleNavigationItem`：描述所属分类、标题、详情 ViewModel 和文档入口。
- `SampleGroup`：按分类聚合示例。
- `SampleCatalog`：合并所有 Luna 示例。
- `ControlSampleCatalog`：登记桌面控件示例。
- `SampleDetailViewModelBase`：提供标题、分类和说明字段。

`MainWindowViewModel` 负责：

- 保存全部示例。
- 根据搜索文本过滤示例。
- 管理当前选中示例。
- 暴露当前标题、分类、描述和内容。
- 切换 `Application.Current.RequestedThemeVariant`。

`ViewLocator` 继续使用当前项目已有模式，通过 ViewModel 命名映射到 View。

## Sample Shell 布局

`MainWindow.axaml` 建议采用桌面工作台布局：

- 顶部：产品标题、说明、浅色/深色主题切换。
- 左侧：搜索框、分类切换、分组示例列表。
- 右侧：当前示例标题、分类、描述、可选文档入口和示例内容。
- 内容区使用 `ScrollViewer`，确保窗口缩放时不裁切。

默认窗口尺寸建议不小于 `1180x760`，最小尺寸不小于 `920x620`。控件页面内部使用 `WrapPanel`、`Grid`、`StackPanel` 和响应式约束，避免宽度变化时文本溢出。

## 示例页面规范

- 示例 View 使用 `UserControl`。
- 示例 ViewModel 继承 `SampleDetailViewModelBase`。
- 交互状态放在 ViewModel 中，View 只做展示和轻量事件桥接。
- 命令使用 CommunityToolkit.Mvvm 的 `[RelayCommand]`。
- 示例不写业务流程，不访问外部服务。
- 每个示例至少包含一组默认状态、一组禁用状态和一组绑定或命令交互。

## 阶段路线

1. 文档与目录基线：完成本 feature 文档并登记索引。
2. 桌面主题基线：建立 `Themes/Index.axaml`、token 目录和控件主题目录。
3. Sample 框架：建立 catalog 模型、`MainWindowViewModel`、左右分栏 shell、搜索和主题切换。
4. 首批原生控件皮肤化：Button、TextBox、CheckBox、RadioButton、ToggleSwitch、Slider、ProgressBar。
5. 首批 Luna 语义控件：DesktopBadge、Tag、Avatar、Empty。
6. 首批反馈示例：Dialog 或 Notification。
7. 文档同步：更新 `docs/controls/` 或新增桌面控件文档入口。

## 并行 Agent 分工

后续控件可按组件域拆分给多个 Agent 并行推进。每个 Agent 必须遵循 `docs/guides/tdesign-desktop-sample-authoring-guide.md`，以 TDesign React 决定示例内容和视觉目标，以 Luna 工程约定决定目录、命名、token 和 catalog 接入方式，Ursa 只作为 Avalonia 实现参考。

- Agent A：Base + Layout，优先实现 Icon、Link、Typography、Divider、Space、Descriptions。
- Agent B：Form 基础，优先实现 Textarea、InputNumber、Select、AutoComplete、Form。
- Agent C：Data 展示，优先实现 Card、Tooltip、Skeleton、Collapse、Timeline、Statistic。
- Agent D：Feedback，优先实现 Alert、Dialog、Drawer、Message、Loading、Popup、Popconfirm。
- Agent E：Navigation，优先实现 Tabs、Menu、Dropdown、Pagination、Breadcrumb、Steps。
- Agent F：复杂数据，独立推进 Table、Tree、DatePicker、TimePicker、Upload，动手前必须参考 Ursa 相邻实现。

## 单控件交付流程

每个控件都按同一条流水线交付，确保不同 Agent 的产物能合并到同一个 catalog。

1. 使用 `tdesign-mcp-server` 查询 React 组件文档和 DOM 结构。
2. 对照 `tdesign-react/packages/components/{component}/_example/` 复刻示例分组、状态组合和示例文案。
3. 判断 Avalonia 映射方式：原生控件皮肤化、Luna 轻量包装控件，或参考 Ursa 的自定义控件结构。
4. 在 `src/Luna.Desktop/` 中补齐必要主题资源、ControlTheme 或控件类，不直接沿用 Ursa Semi 样式。
5. 在 `Luna.Desktop.Sample` 中新增 `{Component}SampleViewModel` 与 `{Component}SampleView`。
6. 接入 `ControlSampleCatalog.CreateSamples()`，文档链接临时指向 TDesign React 对应组件页。
7. 对暂不支持能力保留示例位置，用说明文本或禁用控件占位，不为单个示例临时发明不稳定公共 API。
8. 在本 feature 的 `spec.md` 或 `plan.md` 记录 TDesign 来源、MCP 查询项、Avalonia 映射方式、Ursa 参考和不支持项。

## 控件实现优先队列

### P0：直接分派

- Base：Icon、Link、Typography、ImageViewer。
- Form：Textarea、InputNumber、Select、AutoComplete、DatePicker、TimePicker、Upload、Form。
- Data：Card、Table、Tooltip、Skeleton、Collapse、Timeline、Statistic、Calendar。
- Message：Alert、Dialog、Drawer、Message、Loading、Popup、Popconfirm。
- Navigation：Tabs、Menu、Dropdown、Pagination、Breadcrumb、Steps。
- Layout：Divider、Descriptions、Grid、Space、Layout。

### P1：先设计再实现

- Form：Cascader、TreeSelect、Transfer、ColorPicker、Rate、InputAdornment。
- Data：Tree、List、Comment、QRCode、Watermark、BackTop、Image。
- Message：Swiper。
- Navigation：Anchor、Affix、StickyTool。
- Layout：Guide。

### P2：基础控件稳定后处理

- Data：RangeInput、SelectInput、TagInput。

## 已有示例深化路线

- Button：继续补真实 loading spinner、统一 icon 体系、dashed 边框模板和焦点态。
- DesktopBadge：对齐 count、dot、maxCount、offset、shape。
- Input：补 clearable、prefix/suffix、status、tips、size、maxlength。
- CheckBox / Radio：补 group、disabled、indeterminate、option 列表和键盘行为。
- Switch：补 size、label、loading、disabled。
- Slider：补 range、marks、tooltip、step、disabled。
- Progress：补 line/circle/dashboard、status、label、theme。
- Tag：补 variant、size、shape、closable、checkable。
- Avatar：补 image、icon、size、shape、group、badge 组合。
- Empty：补 image、description、action 和不同场景。
- Notification：补 service/host、position、duration、close、theme 和动画。

## 风险与取舍

- 如果桌面 token 直接复用移动尺寸，控件会显得过大，影响桌面信息密度。因此只复用命名和语义，尺寸默认单独定义。
- 如果先写大量控件再补 Sample 框架，后续示例组织会反复返工。因此先建设 catalog shell。
- 如果完全复制 `AvaloniaSample.Desktop`，会带入官方示例库的复杂度。Luna 只保留 catalog、导航、搜索、主题切换和 ViewLocator 这些必要能力。
- 如果自定义控件过早扩张，会增加 API 兼容成本。首批优先使用原生控件皮肤化。

## 测试策略

- 文档阶段执行静态检查，确认文档齐全、索引登记、无错误外链。
- 实现阶段执行 `dotnet build Luna.sln --no-restore`。
- 桌面 Sample 实现后执行 `dotnet run --project samples/Luna.Desktop.Sample/Luna.Desktop.Sample.csproj`。
- 手工验证启动、搜索、示例切换、主题切换、窗口缩放、键盘 Tab 焦点和禁用态。
- 单控件合并前至少执行 `dotnet build samples/Luna.Desktop.Sample/Luna.Desktop.Sample.csproj --no-restore`。

## 回滚方案

本阶段仅生成文档。若方案方向需要调整，可直接修改本 feature 目录内文档和 `docs/features/index.md` 索引，不影响控件库运行。
