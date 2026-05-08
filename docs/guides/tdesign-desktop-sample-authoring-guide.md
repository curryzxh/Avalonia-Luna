# TDesign Desktop 示例复刻指南

本文记录在 `Luna.Desktop.Sample` 中复刻 TDesign React 桌面端示例页的实践约定。目标是让桌面端示例的信息架构、视觉状态和交互覆盖对齐 TDesign，同时允许在 Avalonia 实现层参考成熟的 `Ursa.Avalonia` 控件经验。

## 信息来源

优先通过 `tdesign-mcp-server` 查询组件列表、组件文档和 DOM 结构；需要确认示例分组、源码细节或样式变量时，再读取 TDesign React 官方源码和文档。不要只看截图或页面 DOM。

- MCP 组件列表：`get_component_list({ framework: "react" })`
- MCP 组件文档：`get_component_docs({ framework: "react", names: [...] })`
- MCP DOM 结构：`get_component_dom({ framework: "react", names: [...] })`
- TDesign React 仓库：`https://github.com/Tencent/tdesign-react`
- TDesign React 文档站：`https://tdesign.tencent.com/react`
- TDesign 总仓库与设计体系说明：`https://github.com/Tencent/tdesign`
- 本地 Ursa 参考仓库：`/Users/curryzxh/Documents/work/avalonia/Ursa.Avalonia`
- 移动端复刻指南：`docs/guides/tdesign-mobile-sample-authoring-guide.md`

TDesign React 当前是桌面端 React 组件库，仓库主体位于 `packages/`。复刻时重点查：

- `packages/components/{component}/_example/`：示例页面结构、分组和示例组合。
- `packages/components/{component}/{component}.md` 或中英文文档：组件 API、状态、说明和设计语义。
- `packages/common/style/web/components/{component}/`：跨框架共享样式，确认 token、类名和状态选择器。
- `packages/common/style/web/_variables/`：设计 token 和全局样式变量。

`tdesign-mcp-server` 的 React 组件列表可作为 Luna.Desktop catalog 分类参考：

- `base`：button、icon、image-viewer、link、typography。
- `form`：input、textarea、checkbox、radio、switch、select、date-picker、time-picker、form、slider、upload、transfer、tree-select 等。
- `data`：avatar、badge、card、collapse、comment、image、list、progress、qrcode、skeleton、statistic、table、tag、timeline、tooltip、tree、watermark 等。
- `message`：alert、dialog、drawer、loading、message、notification、popconfirm、popup、swiper。
- `navigation`：affix、anchor、breadcrumb、dropdown、menu、pagination、steps、sticky-tool、tabs。
- `layout`：descriptions、divider、empty、grid、guide、layout、space。

示例：

```bash
curl -L 'https://api.github.com/repos/Tencent/tdesign-react/contents/packages/components/button/_example?ref=develop'
curl -L 'https://raw.githubusercontent.com/Tencent/tdesign-react/develop/packages/components/button/_example/index.tsx'
curl -L 'https://raw.githubusercontent.com/Tencent/tdesign-react/develop/packages/common/style/web/components/button/_index.less'
```

## 复刻目标

- 示例的信息架构对齐 TDesign React：页面标题、分组顺序、示例名称、状态覆盖和常见组合尽量一致。
- 控件视觉对齐 TDesign：颜色、字号、圆角、边框、阴影、间距、禁用态、悬停态、焦点态和深色主题优先使用 `Luna.*` token 表达。
- Avalonia 实现尊重桌面端语义：键鼠交互、焦点可见性、窗口缩放、右键菜单、弹层定位和高密度布局要单独评估。
- Ursa 只作为实现参考：可复刻控件结构、属性建模、模板部件、服务形态和测试思路，但样式必须重新按 TDesign 调整。

## 三层参考优先级

1. TDesign React 决定示例内容和视觉目标。
2. Luna 当前工程约定决定目录、命名、token 和 Sample 接入方式。
3. Ursa.Avalonia 决定 Avalonia 控件实现是否已有成熟路径。

如果三者冲突，优先保持 TDesign 视觉和 Luna 工程约定。Ursa 的 Semi 样式、命名体系和桌面交互细节不能直接成为 Luna 的最终样式。

## 复刻流程

1. 从 `tdesign-react` 找到目标组件目录，例如 `packages/components/button/`。
2. 读取 `_example/index.tsx`，整理页面标题、分组顺序和引用的子示例。
3. 读取每个子示例 `.tsx`，提取控件状态、属性组合、事件行为和示例文案。
4. 使用 `tdesign-mcp-server` 读取组件文档和 DOM 结构，确认 TDesign 的核心 API、类名、状态、尺寸、主题和交互事件。
5. 读取组件文档和 Less 样式，补充确认 token、选择器、间距、弹层层级和深色主题表现。
6. 对照 Avalonia 原生控件判断实现类型：
   - 原生控件语义匹配时，优先做 Luna 桌面 ControlTheme 和资源 token。
   - 原生控件可组合但缺少 TDesign 语义时，提供 Luna 包装控件。
   - 涉及弹层、服务化调用、复杂选择器、虚拟化、自绘或多模板状态时，再参考 Ursa 自定义控件实现。
7. 在 Ursa 中查找相邻控件：
   - 控件类：`src/Ursa/Controls/{Control}.cs`
   - 主题模板：`src/Ursa.Themes.Semi/Controls/{Control}.axaml`
   - Demo 页面：`demo/Ursa.Demo/Pages/{Control}Demo.axaml`
   - 测试：`tests/Test.Ursa/` 或 `tests/HeadlessTest.Ursa/`
8. 在 `Luna.Desktop.Sample` 中新增或更新示例页面，接入 catalog。
9. 同步补齐 `Luna.Desktop` 的主题资源或控件实现。
10. 执行构建和手工验证。

## Ursa 参考方式

Ursa 中有大量桌面控件实现，适合作为 Avalonia 技术参考：

- 数据展示：`Avatar`、`Badge`、`Descriptions`、`Timeline`、`QrCode`、`Skeleton`。
- 输入：`AutoCompleteBox`、`MultiComboBox`、`DatePicker`、`DateTimePicker`、`NumericUpDown`、`RangeSlider`、`Rating`、`PinCode`。
- 反馈：`Dialog`、`MessageBox`、`Notification`、`Toast`、`Drawer`、`PopConfirm`、`Loading`。
- 导航：`Breadcrumb`、`NavMenu`、`Pagination`、`Anchor`。
- 工具型控件：`KeyGestureInput`、`PathPicker`、`ThemeSelector`、`TitleBar`、`ToolBar`。

允许复刻的内容：

- 控件分层：`TemplatedControl`、服务类、Host、Manager、Overlay 的拆分方式。
- 属性建模：`StyledProperty<T>`、命令、事件、枚举、结果类型和关闭原因。
- 模板契约：模板部件命名、`OnApplyTemplate` 绑定和解绑方式。
- 测试思路：状态计算、选择器、日期范围、弹层 host、路径选择等逻辑测试。
- Demo 组织：控件用法覆盖、状态分组和交互触发方式。

禁止直接照搬的内容：

- `Ursa.Themes.Semi` 的视觉风格。
- Semi 主题资源键作为 Luna 对外 token。
- 与 TDesign 不一致的圆角、间距、色彩、阴影和动效。
- 与 Luna 命名约定冲突的 API。
- 未经裁剪的复杂服务或框架扩展。

## TDesign 到 Avalonia 映射原则

- `theme` 映射到 Luna 样式类或资源变体，例如 `primary`、`default`、`danger`、`warning`、`success`。
- `variant` 映射到外观类型，例如填充、描边、文本、虚线、浅色背景。
- `size` 映射到桌面尺寸 token，例如 `small`、默认、`large`；桌面默认尺寸应比移动端更紧凑。
- `shape` 映射到矩形、正方形、圆角矩形、圆形等形状 token。
- `block` 映射到 Avalonia 的横向拉伸布局。
- `status` 映射到 Avalonia 伪类、样式类或校验状态，例如 error、warning、success。
- `disabled` 必须使用 Avalonia `IsEnabled`，不要只做视觉灰化。
- `loading` 先判断是否已有 Luna/Ursa 可复用实现；没有时在示例中保留占位，避免为单个示例临时扩展公共 API。
- 弹层类组件优先明确 Host、遮罩、关闭来源、焦点归还和 Esc 行为。
- 表格、树、选择器和日期控件必须先确认数据结构和键盘操作，再做视觉复刻。

代表组件的 MCP 检查重点：

- Button：`theme`、`variant`、`size`、`shape`、`loading`、`block`、`icon`、`suffix`、`disabled`。
- Dialog：`visible`、`mode`、`placement`、`showOverlay`、`closeOnEscKeydown`、`closeOnOverlayClick`、`confirmLoading`、`onClose` 的关闭来源。
- DatePicker：`mode`、`format`、`valueType`、`presets`、`clearable`、`status`、`tips`、日期范围与时间选择。
- Table：列定义、分页、hover、stripe、bordered、resizable、空内容和行点击。
- Form：布局、标签宽度、必填标记、校验规则、字段状态、提交、重置和统一禁用。

## 示例页结构约定

桌面示例页不复用移动端的返回栏结构，应接入桌面 catalog shell。

推荐结构：

- 顶部由 Sample shell 展示组件名、分类和说明。
- 示例 View 内使用 `StackPanel` 或 `Grid` 分组。
- 每个分组包含简短标题、必要说明和可交互示例。
- 同一组中多个状态用 `WrapPanel` 或固定列宽 `Grid` 展示。
- 复杂控件可拆成多个 `Border` 区块，但不要在卡片里嵌套卡片。
- 示例控件必须支持窗口缩放，窄宽度下不应文本溢出或控件重叠。

示例命名建议：

- ViewModel：`{Component}SampleViewModel`
- View：`{Component}SampleView.axaml`
- 分类登记：`ControlSampleCatalog.CreateSamples()`
- 文档入口：优先指向 Luna 文档；尚未有 Luna 文档时可临时记录 TDesign React 文档链接。

## 不支持能力的处理

如果 Avalonia 或 Luna 暂不支持 TDesign 示例能力，先保留示例位置并明确占位。

占位规则：

- 不删除官方示例分组。
- 用禁用控件、说明文本或简化交互表示能力缺口。
- 在 `plan.md` 或对应 feature 文档记录后续实现任务。
- 不为单个示例临时增加不稳定的公共 API。

常见占位：

- `loading`：用禁用态和加载文本占位，后续补 Loading token 或控件。
- `icon`：先用 `PathIcon` 或现有图标资源占位，后续统一图标体系。
- `popupProps` / 高级弹层配置：先覆盖基础打开、关闭、定位和焦点。
- `virtual scroll`：先展示小数据量静态示例，再规划虚拟化能力。
- `drag sort`：先展示静态排序和命令入口，再进入拖拽任务。

## 可直接优先评估 Ursa 的组件

以下 TDesign 桌面组件在 Ursa 中有相邻成熟实现，适合优先评估复刻：

- Avatar：参考 `Ursa.Controls.Avatar`，样式按 TDesign avatar 重做。
- Badge：参考 `Badge`、`DualBadge`，注意 TDesign 徽标位置、形状和数字封顶。
- Dialog：参考 `Dialog`、`MessageBox`、`OverlayDialog`，按 TDesign Modal/Dialog 视觉重做。
- Drawer：参考 `Drawer`、`OverlayDrawer`，按 TDesign Drawer 宽度、遮罩和关闭行为调整。
- Notification：参考 `Notification`、`NotificationCard`，按 TDesign 通知位置和类型重做。
- Toast / Message：参考 `Toast`、`MessageCard`，按 TDesign Message/Toast 视觉重做。
- DatePicker / TimePicker：参考 Ursa 日期时间选择器族，按 TDesign 输入框、面板和快捷项重做。
- AutoComplete / Select：参考 `AutoCompleteBox`、`MultiComboBox`、`TreeComboBox`。
- Pagination：参考 `Pagination`，对齐 TDesign 页码、跳转和 page size 选择。
- Rate / Slider：参考 `Rating`、`RangeSlider`。
- Form / Descriptions：参考 `Form`、`Descriptions`，对齐 TDesign 表单布局、校验和描述列表。

## 验收清单

- 示例分组顺序与 TDesign React `_example/index.tsx` 一致。
- 示例状态覆盖 TDesign 官方示例中的主要状态，不支持能力有明确占位。
- Avalonia 实现优先使用 `Luna.*` token，不直接沿用 Semi 样式。
- Ursa 参考来源在 feature 或代码评审说明中写清楚。
- 示例可从 `Luna.Desktop.Sample` catalog 进入。
- 浅色/深色主题下文字、边框、背景和焦点态可读。
- 窗口缩放到最小尺寸时不重叠、不裁切关键操作。
- 键盘 Tab 焦点、Esc 关闭、Enter 默认操作和指针悬停态符合桌面习惯。
- `dotnet build samples/Luna.Desktop.Sample/Luna.Desktop.Sample.csproj --no-restore` 通过。
- `dotnet build Luna.sln --no-restore` 通过。

## 记录要求

每次新增桌面示例时，在对应 feature 的 `spec.md` 或 `plan.md` 中记录：

- TDesign React 源码路径。
- 使用过的 `tdesign-mcp-server` 查询项。
- 使用的 TDesign 示例分组。
- Avalonia 映射方式。
- 是否参考 Ursa，参考了哪些控件类、主题或 Demo。
- 不支持能力和后续任务。
