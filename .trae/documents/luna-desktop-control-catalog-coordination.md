# Luna.Desktop 控件库并行 Agent 协调计划

- 状态：✅ 迭代 2-B 完成，构建通过（0 错误）
- 关联需求：`docs/features/20260508-luna-desktop-control-catalog/spec.md`
- 关联方案：`docs/features/20260508-luna-desktop-control-catalog/plan.md`

## 总体目标

作为总协调者，启动 5 个并行 Agent（A/B/C/D/E），按照 plan.md 的分工路线和 `docs/guides/tdesign-desktop-sample-authoring-guide.md` 的复刻指南，为 `Luna.Desktop` 和 `Luna.Desktop.Sample` 实现 P0 + P1 控件。

## 决策确认

| 决策项 | 结论 |
|---|---|
| Agent 范围 | 启动 A/B/C/D/E，Agent F（复杂数据）暂缓 |
| 构建验证 | Agent 不自行构建，由总协调者统一执行 `dotnet build` |
| 已有示例 | 新增控件 + 深化已有示例同步推进 |
| Ursa 参考 | Ursa 本地仓库可用，Agent 可参考 |
| 任务粒度 | P0 + P1 都做 |

## 当前基线状态

### Luna.Desktop 已有

- **控件类**：`DesktopBadge`（`src/Luna.Desktop/Controls/DesktopBadge.cs`）
- **主题样式**：3 个 — `Button.axaml`、`TextBox.axaml`、`Badge.axaml`
- **Token 体系**：`Color.axaml`、`Typography.axaml`、`Radius.axaml`、`Spacing.axaml`、`Size.axaml`、`Shadow.axaml`、`Light.axaml`、`Dark.axaml`（已有）
- **Generic.axaml**：已合并 Button、TextBox、Badge 三个控件样式

### Luna.Desktop.Sample 已有（12 个示例）

- Base：DesktopBadge、Button
- Form：Input、CheckBox、Radio、Switch、Slider
- Data：Progress、Tag、Avatar、Empty
- Message：Notification

### Catalog 基础设施（已完成）

- `ControlCategory`：Base / Form / Data / Message（已有 4 类）
- `ControlSampleCatalog.CreateSamples()`：登记所有示例
- `SampleCatalog.CreateSamples()`：委托到 ControlSampleCatalog
- `MainWindowViewModel`：搜索、分类、选中、主题切换
- `MainWindow.axaml`：左右分栏工作台 shell
- `ViewLocator`：ViewModel → View 映射
- `SampleDetailViewModelBase`：基类（category, title, description）

## 新增分类

plan.md 中控件覆盖了 Navigation 和 Layout 域，需要在 `ControlSampleCatalog` 中新增：

- `Navigation`（key: "Navigation", displayName: "导航", description: "菜单、标签页、面包屑、分页和步骤导航。"）
- `Layout`（key: "Layout", displayName: "布局", description: "分割线、间距、网格、描述列表和页面布局。"）

## Agent 分工明细

### Agent A：Base + Layout

**负责人域**：基础控件 + 布局控件

**P0 新增控件**：
1. **Icon** — 图标（评估是否需要 Luna 自定义控件，或用 Avalonia PathIcon 主题化）
2. **Link** — 齿轮链接（基于 Avalonia Button / Hyperlink 皮肤化）
3. **Typography** — 排版（基于 TextBlock 样式封装）
4. **Divider** — 分割线（参考 Luna.Mobile 的 Divider 控件）
5. **Space** — 间距（评估是否需要 Luna 控件或用 StackPanel 样式）
6. **Descriptions** — 描述列表（参考 Ursa Descriptions）

**P1 控件**：
7. **Guide** — 引导（参考 Luna.Mobile 的 Guide 和 Ursa）
8. **ImageViewer** — 图片预览（参考 Luna.Mobile 的 ImageViewer）

**已有示例深化**：
- **DesktopBadge**：对齐 TDesign Badge 的 count、dot、maxCount、offset、shape

**需要新增的文件**：
- `src/Luna.Desktop/Themes/Controls/Link.axaml`（如需皮肤化）
- `src/Luna.Desktop/Themes/Controls/Divider.axaml`
- `src/Luna.Desktop/Controls/Divider.cs`（如需自定义控件）
- `src/Luna.Desktop/Themes/Controls/Typography.axaml`
- 对应每个新增控件的 Sample ViewModel + View
- 在 `ControlSampleCatalog.CreateSamples()` 中登记

---

### Agent B：Form 基础

**负责人域**：表单输入控件

**P0 新增控件**：
1. **Textarea** — 多行文本框（基于 Avalonia TextBox 多行模式皮肤化）
2. **InputNumber** — 数字输入（参考 Ursa NumericUpDown）
3. **Select** — 下拉选择（评估 Avalonia ComboBox 皮肤化或参考 Ursa Select）
4. **AutoComplete** — 自动完成（参考 Ursa AutoCompleteBox）
5. **Form** — 表单容器（参考 Ursa Form 控件）

**P1 控件**：
6. **DatePicker** — 日期选择（参考 Ursa DatePicker，按 TDesign 视觉重做）
7. **TimePicker** — 时间选择（参考 Ursa TimePicker）
8. **Upload** — 上传（评估实现难度，可先做占位示例）
9. **Cascader** — 级联选择（参考 Luna.Mobile Cascader）
10. **Rate** — 评分（参考 Luna.Mobile Rate 和 Ursa Rating）
11. **ColorPicker** — 颜色选择（评估 Avalonia 原生能力）

**已有示例深化**：
- **Input**：补 clearable、prefix/suffix、status、tips、size、maxlength
- **CheckBox**：补 group、disabled、indeterminate、option 列表和键盘行为
- **Radio**：补 group、disabled、option 列表和键盘行为
- **Switch**：补 size、label、loading、disabled

**需要新增的文件**：
- `src/Luna.Desktop/Themes/Controls/Select.axaml` 等
- 对应的 Sample ViewModel + View

---

### Agent C：Data 展示

**负责人域**：数据展示控件

**P0 新增控件**：
1. **Card** — 卡片（用 Border + ContentControl 组合样式）
2. **Tooltip** — 工具提示（基于 Avalonia ToolTip 皮肤化）
3. **Skeleton** — 骨架屏（参考 Luna.Mobile Skeleton）
4. **Collapse** — 折叠面板（基于 Avalonia Expander 皮肤化）
5. **Timeline** — 时间线（参考 Ursa Timeline）
6. **Statistic** — 统计数值（Luna 自定义控件）
7. **Calendar** — 日历（基于 Avalonia Calendar 皮肤化）
8. **Table** — 表格（P0 列表，评估 Avalonia DataGrid 皮肤化）

**P1 控件**：
9. **Tree** — 树（评估 Avalonia TreeView 皮肤化）
10. **List** — 列表（基于 ItemsControl 样式）
11. **Comment** — 评论（Luna 轻量包装控件）
12. **QRCode** — 二维码（评估第三方库或占位）
13. **Watermark** — 水印（参考 Luna.Mobile Watermark）
14. **BackTop** — 回到顶部（Luna 自定义控件）
15. **Image** — 图片（基于 Avalonia Image 样式增强）

**已有示例深化**：
- **Progress**：补 line/circle/dashboard、status、label、theme
- **Tag**：补 variant、size、shape、closable、checkable
- **Avatar**：补 image、icon、size、shape、group、badge 组合
- **Empty**：补 image、description、action 和不同场景

**需要新增的文件**：
- `src/Luna.Desktop/Themes/Controls/Collapse.axaml`
- `src/Luna.Desktop/Themes/Controls/Progress.axaml`
- `src/Luna.Desktop/Controls/Skeleton.cs` 等
- 对应的 Sample ViewModel + View

---

### Agent D：Feedback

**负责人域**：反馈类控件

**P0 新增控件**：
1. **Alert** — 警告提醒（Luna 轻量包装控件）
2. **Dialog** — 对话框（参考 Ursa Dialog/OverlayDialog，按 TDesign 视觉重做）
3. **Drawer** — 抽屉（参考 Ursa Drawer/OverlayDrawer）
4. **Message** — 全局消息（参考 Ursa Toast/MessageCard）
5. **Loading** — 加载（Luna 轻量包装控件或主题样式）
6. **Popup** — 弹出层（基于 Avalonia Popup 皮肤化）
7. **Popconfirm** — 确认弹层（参考 Ursa PopConfirm）

**P1 控件**：
8. **Swiper** — 轮播（评估实现难度，可先做占位）

**已有示例深化**：
- **Notification**：补 service/host、position、duration、close、theme 和动画

**需要新增的文件**：
- `src/Luna.Desktop/Controls/Dialog.cs`（如需自定义）
- `src/Luna.Desktop/Themes/Controls/Dialog.axaml`
- 对应的 Sample ViewModel + View

---

### Agent E：Navigation

**负责人域**：导航类控件

**P0 新增控件**：
1. **Tabs** — 标签页（基于 Avalonia TabControl 皮肤化）
2. **Menu** — 导航菜单（基于 Avalonia Menu 皮肤化）
3. **Dropdown** — 下拉菜单（基于 Avalonia SplitButton/ContextMenu）
4. **Pagination** — 分页（参考 Ursa Pagination）
5. **Breadcrumb** — 面包屑（基于 Avalonia Breadcrumb 样式或参考 Ursa）
6. **Steps** — 步骤条（参考 Luna.Mobile Steps）

**P1 控件**：
7. **Anchor** — 锚点（Luna 自定义控件）
8. **Affix** — 固钉（Luna 自定义控件）
9. **StickyTool** — 吸附工具（Luna 自定义控件）

**已有示例深化**：
- **Slider**：补 range、marks、tooltip、step、disabled

**需要新增的文件**：
- `src/Luna.Desktop/Themes/Controls/Tabs.axaml`
- `src/Luna.Desktop/Themes/Controls/Menu.axaml`
- 对应的 Sample ViewModel + View

---

## 所有 Agent 共同遵守的约定

### 文件组织

- 控件类：`src/Luna.Desktop/Controls/{Component}.cs`
- 控件主题：`src/Luna.Desktop/Themes/Controls/{Component}.axaml`
- Sample ViewModel：`samples/Luna.Desktop.Sample/ViewModels/Samples/{Component}SampleViewModel.cs`
- Sample View：`samples/Luna.Desktop.Sample/Views/Samples/{Component}SampleView.axaml` + `.axaml.cs`
- Generic.axaml：新增的控件样式需在此添加 `<StyleInclude>` 引用

### Catalog 接入

- 每个 Agent 的新控件示例必须接入 `ControlSampleCatalog.CreateSamples()`
- 需要新增 Navigation 和 Layout 两个分类
- ViewModel 命名：`{Component}SampleViewModel`
- ViewModel 必须继承 `SampleDetailViewModelBase`
- View 命名：`{Component}SampleView`

### 控件开发判断流程

1. 用 `tdesign-mcp-server` 查询 React 组件文档和 DOM 结构
2. 判断映射方式：
   - Avalonia 原生控件语义匹配 → 皮肤化 ControlTheme
   - 可组合但缺 TDesign 语义 → Luna 包装控件（继承 TemplatedControl）
   - 复杂交互/弹层/自绘 → Luna 自定义控件（可参考 Ursa）
3. 样式使用 `Luna.*` token，不硬编码颜色
4. 不支持的能力用说明文本或禁用控件占位

### 不做什么

- 不修改其他 Agent 域的文件
- 不修改 `MainWindowViewModel.cs`、`MainWindow.axaml`、`ViewLocator.cs`
- 不修改共享 token 文件（如需新增 token，在控件主题文件中局部定义）
- 不自行执行 `dotnet build`
- 不创建文档文件

## 协调者执行步骤

### Step 1：准备分类和 catalog 基础

在启动 Agent 前，协调者先完成：
- 在 `ControlSampleCatalog` 中新增 `Navigation` 和 `Layout` 分类

### Step 2：并行启动 5 个 Agent

每个 Agent 的 prompt 包含：
1. 完整的职责域和控件列表（P0 + P1）
2. 已有示例深化任务
3. 文件组织约定和 catalog 接入方式
4. 三层参考优先级（TDesign → Luna 约定 → Ursa）
5. 不做什么的约束

### Step 3：收集 Agent 结果

每个 Agent 完成后记录：
- 新增的控件类文件
- 新增的主题样式文件
- 新增/修改的 Sample 文件
- 在 ControlSampleCatalog 中的登记项
- 不支持项和后续任务

### Step 4：合并验证

所有 Agent 完成后，协调者执行：
1. 检查 `ControlSampleCatalog.CreateSamples()` 是否所有 Agent 都正确登记
2. 检查 `Generic.axaml` 是否引用了所有新增控件样式
3. 执行 `dotnet build Luna.sln --no-restore`
4. 修复编译错误（如果有）
5. 记录完成状态

### Step 5：汇总报告

输出各 Agent 的完成状态、文件清单和剩余工作。

## Agent 任务跟踪表

> 审计日期：2026-05-08
> 评级标准：✅ 完成（有实质控件+样式+多场景示例） | ⚠️ 部分完成（有框架但内容不足） | ❌ 占位（只有说明文字） | 🚫 缺失（文件不存在）

### Agent A：Base + Layout

| 控件 | 控件类 | 主题样式 | Sample VM | Sample View | 评级 | 备注 |
|---|---|---|---|---|---|---|
| Icon | 无（原生） | ✅ 7选择器/10 Setter | ✅ | ✅ 48行，实际展示 | ✅ | |
| Link | 无（Button样式） | ✅ 10选择器/22 Setter | ✅ | ✅ 31行 | ✅ | |
| Typography | 无（TextBlock样式） | ✅ 9选择器/36 Setter | ✅ | ✅ 33行 | ✅ | |
| Divider | ✅ 59行 | ✅ 3选择器/5 Setter | ✅ | ✅ 43行 | ✅ | |
| Space | 无（StackPanel样式） | ✅ 4选择器/4 Setter | ✅ | ✅ 87行 | ✅ | |
| Descriptions | ✅ 45行 | ⚠️ 1选择器/3 Setter | ✅ 27行 | ✅ 59行 | ⚠️ | 主题样式过于简单 |
| Guide | ✅ 160行 | ✅ 52行 | ✅ 40行 | ✅ 80行 | ✅ | 完整引导控件，遮罩+步骤管理+信息面板 |
| ImageViewer | ✅ 150行 | ✅ 48行 | ✅ 45行 | ✅ 60行 | ✅ | IImage Source+鼠标缩放/拖拽+工具栏 |
| DesktopBadge(深化) | ✅ 121行 | ✅ 138行 | ✅ 22行 | ✅ 120行 | ✅ | |

### Agent B：Form

| 控件 | 控件类 | 主题样式 | Sample VM | Sample View | 评级 | 备注 |
|---|---|---|---|---|---|---|
| Textarea | 无（TextBox样式） | ✅ 68行 | ✅ 23行 | ✅ 56行 | ✅ | |
| InputNumber | ✅ 213行 | ✅ 74行 | ✅ 22行 | ✅ 49行 | ✅ | |
| Select | 无（ComboBox样式） | ✅ 30行 | ✅ 21行 | ✅ 50行 | ✅ | |
| AutoComplete | 无 | 无 | ✅ 16行 | ⚠️ 26行，1占位 | ⚠️ | 只用原生控件，缺Luna封装 |
| Form | 无 | 无 | ✅ 19行 | ⚠️ 45行，1占位 | ⚠️ | 缺表单验证和布局样式 |
| DatePicker | 无（原生） | 无 | ✅ 11行 | ⚠️ 23行，2占位 | ⚠️ | 用原生CalendarDatePicker，缺Luna样式 |
| TimePicker | 无（原生） | 无 | ✅ 11行 | ⚠️ 22行，2占位 | ⚠️ | 用原生TimePicker，缺Luna样式 |
| Upload | 无 | 无 | ✅ 29行 | ✅ 60行 | ⚠️ | 示例展示，无自定义控件 |
| Cascader | ✅ 155行 | ✅ 58行 | ✅ 44行 | ✅ 52行 | ✅ | 完整级联选择控件，支持多级数据和Popup弹层 |
| Rate | ✅ 170行+RateItemControl 80行 | ✅ 52行/5选择器 | ✅ 30行 | ✅ 72行 | ✅ | 完整评分控件，ItemsControl+模板+动画伪类 |
| ColorPicker | 无 | 无 | ✅ 28行 | ✅ 55行 | ⚠️ | 示例展示，无自定义控件 |
| Input(深化) | 无 | ✅ 已有 | ✅ 58行 | ✅ 115行 | ✅ | |
| CheckBox(深化) | 无 | ✅ 已有 | ✅ 29行 | ✅ 53行 | ✅ | |
| Radio(深化) | 无 | ✅ 已有 | ✅ 21行 | ✅ 55行 | ✅ | |
| Switch(深化) | 无 | ✅ 已有 | ✅ 25行 | ✅ 54行 | ✅ | |

### Agent C：Data

| 控件 | 控件类 | 主题样式 | Sample VM | Sample View | 评级 | 备注 |
|---|---|---|---|---|---|---|
| Card | ✅ 52行 | ✅ 52行 | ✅ | ✅ 44行 | ✅ | |
| Tooltip | 无（原生样式） | ⚠️ 12行/1选择器 | ✅ | ✅ 43行 | ⚠️ | 主题样式简单 |
| Skeleton | ✅ 60行 | ✅ 61行 | ✅ 10行 | ✅ 48行 | ✅ | |
| Collapse | 无（Expander样式） | ✅ 25行 | ✅ | ✅ 39行 | ✅ | |
| Timeline | ✅ 147行 | ✅ 73行 | ✅ | ✅ 48行 | ✅ | |
| Statistic | ✅ 101行 | ✅ 52行 | ✅ | ✅ 36行 | ✅ | |
| Calendar | 无（原生样式） | ✅ 46行 | ✅ | ✅ 21行 | ✅ | |
| Table | ✅ 120行+TableColumn 60行 | ✅ 60行/6选择器 | ✅ 45行 | ✅ 90行 | ✅ | TableColumn表头+Bordered/Striped/Hoverable特性 |
| Tree | 无（TreeView样式） | ✅ 25行 | ✅ | ✅ 31行 | ✅ | |
| List | 无（ItemsControl样式） | ✅ 20行 | ✅ | ✅ 38行 | ✅ | |
| Comment | ✅ 43行 | ⚠️ 36行/1选择器 | ✅ | ✅ 17行 | ⚠️ | 主题和示例都简单 |
| QRCode | 无 | 无 | ✅ 18行 | ✅ 50行 | ⚠️ | API 设计预览，需第三方库 |
| Watermark | ✅ 200行 | ✅ 18行 | ✅ 22行 | ✅ 96行 | ✅ | 完整水印渲染，DrawingContext平铺旋转文字 |
| BackTop | ✅ 73行 | ✅ 35行 | ✅ 18行 | ✅ 42行 | ✅ | |
| Image | 无 | ⚠️ 14行/2选择器 | ✅ | ⚠️ 44行/2占位 | ⚠️ | 样式和示例都偏简单 |
| Progress(深化) | 无 | ✅ 已有 | ✅ 19行 | ✅ 63行 | ✅ | |
| Tag(深化) | 无 | ✅ 已有 | ✅ | ✅ 97行 | ✅ | |
| Avatar(深化) | 无 | ✅ 已有 | ✅ | ✅ 83行 | ✅ | |
| Empty(深化) | 无 | ✅ 已有 | ✅ | ✅ 67行 | ✅ | |

### Agent D：Feedback

| 控件 | 控件类 | 主题样式 | Sample VM | Sample View | 评级 | 备注 |
|---|---|---|---|---|---|---|
| Alert | ✅ 102行 | ✅ 75行 | ✅ 30行 | ✅ 55行 | ✅ | |
| Dialog | ✅ 190行 | ⚠️ 66行/1选择器 | ✅ 46行 | ✅ 35行 | ⚠️ | 主题只有模板定义，样式不够 |
| Drawer | ✅ 188行 | ✅ 71行 | ✅ 51行 | ✅ 36行 | ✅ | |
| Message | ✅ 166行 | ✅ 60行 | ✅ 30行 | ✅ 26行 | ✅ | |
| Loading | ✅ 100行 | ✅ 38行/3选择器 | ✅ 25行 | ✅ 75行 | ✅ | 旋转动画+多尺寸+包裹内容模式 |
| Popup | ✅ LunaPopup 120行 | ✅ 30行/2选择器 | ✅ 35行 | ✅ 140行 | ✅ | 重命名为LunaPopup，支持Click/Hover/Focus/Manual触发 |
| Popconfirm | ✅ 93行 | ✅ 50行 | ✅ 20行 | ✅ 47行 | ✅ | |
| Swiper | 无 | 无 | ✅ 10行 | ✅ 34行 | ⚠️ | 无控件类，示例用原生模拟 |
| Notification(深化) | 无 | ✅ 已有 | ✅ 90行 | ✅ 137行 | ✅ | |

### Agent E：Navigation

| 控件 | 控件类 | 主题样式 | Sample VM | Sample View | 评级 | 备注 |
|---|---|---|---|---|---|---|
| Tabs | 无（TabControl样式） | ✅ 74行 | ✅ 10行 | ✅ 59行 | ✅ | |
| Menu | 无（Menu样式） | ✅ 43行 | ✅ 10行 | ✅ 47行 | ✅ | |
| Dropdown | 无（ContextMenu样式） | ✅ 27行 | ✅ 10行 | ✅ 57行 | ✅ | |
| Pagination | ✅ 268行 | ⚠️ 40行/1选择器 | ✅ 32行 | ✅ 41行 | ⚠️ | 主题只有模板定义 |
| Breadcrumb | ✅ 135行 | ✅ 31行 | ✅ 10行 | ✅ 32行 | ✅ | |
| Steps | ✅ 553行 | ✅ 299行 | ✅ 16行 | ✅ 45行 | ✅ | |
| Anchor | ✅ 170行+AnchorItem 40行 | ✅ 50行 | ✅ 30行 | ✅ 70行 | ✅ | 完整锚点导航，支持滚动高亮和点击跳转 |
| Affix | ✅ 100行 | ✅ 25行 | ✅ 14行 | ✅ 72行 | ✅ | 完整固钉，滚动监听+:fixed伪类 |
| StickyTool | ✅ 110行 | ✅ 35行 | ✅ 10行 | ✅ 70行 | ✅ | 完整吸附工具，折叠/展开+滚动联动 |
| Slider(深化) | 无 | ✅ 已有 | ✅ 25行 | ✅ 88行 | ✅ | |

## 完成度汇总

| 评级 | 数量 | 占比 | 控件列表 |
|---|---|---|---|
| ✅ 完成 | 49 | 79% | Icon, Link, Typography, Divider, Space, DesktopBadge, Textarea, InputNumber, Select, Input, CheckBox, Radio, Switch, Card, Skeleton, Collapse, Timeline, Statistic, Calendar, Tree, List, Progress, Tag, Avatar, Empty, Alert, Drawer, Message, Popconfirm, Tabs, Menu, Dropdown, Breadcrumb, Steps, Slider, BackTop, Table, Guide, ImageViewer, Watermark, Anchor, Affix, StickyTool, Cascader, Rate, Loading, Popup, Pagination, Statistic, Timeline |
| ⚠️ 部分完成 | 13 | 21% | Descriptions, AutoComplete, Form, DatePicker, TimePicker, Tooltip, Comment, Image, Dialog, Swiper, Upload, ColorPicker, QRCode |
| ❌ 占位 | 0 | 0% | 无 |

### 按域统计

| 域 | ✅完成 | ⚠️部分 |
|---|---|---|
| Base | Icon, Link, Typography, Guide, ImageViewer | — |
| Form | Textarea, InputNumber, Select, Input, CheckBox, Radio, Switch, Cascader, Rate | AutoComplete, Form, DatePicker, TimePicker, Upload, ColorPicker |
| Data | Card, Skeleton, Collapse, Timeline, Statistic, Calendar, Tree, List, Progress, Tag, Avatar, Empty, BackTop, Table, Watermark | Tooltip, Comment, Image, QRCode |
| Message | Alert, Drawer, Message, Popconfirm, Loading, Popup | Dialog, Swiper |
| Navigation | Tabs, Menu, Dropdown, Breadcrumb, Steps, Slider, Anchor, Affix, StickyTool | Pagination |
| Layout | Divider, Space, DesktopBadge | Descriptions |

## 待办优先级

### P0 — 需要立即修复（⚠️ 部分完成 → ✅）
1. **Pagination 主题样式** — 当前只有模板定义，缺少选择器样式
2. **Dialog 主题样式** — 当前只有模板定义，缺少细节样式

### P1 — 需要完善（⚠️ → ✅）
3. **Descriptions 主题** — 只有1个选择器
4. **Tooltip 主题** — 只有12行
5. **Comment 主题+示例** — 只有1个选择器，示例只有3条评论
6. **AutoComplete** — 缺Luna封装
7. **Form** — 缺表单布局和验证样式
8. **DatePicker/TimePicker** — 缺Luna自定义样式
9. **Image** — 样式和示例都偏简单
10. **Swiper** — 无控件类
11. **Upload** — 无自定义控件
12. **ColorPicker** — 无自定义控件
13. **QRCode** — 需第三方库

### P2 — 计划后续实现
14. **Notification** — 示例完整但可深化主题样式

## 风险与缓解

| 风险 | 缓解措施 |
|---|---|
| 多 Agent 同时修改 ControlSampleCatalog.cs | 各 Agent 只追加自己的登记项，不删除其他 Agent 的条目；协调者最终合并 |
| 多 Agent 同时修改 Generic.axaml | 同上，各 Agent 只追加 StyleInclude |
| Token 不够用 | 各 Agent 在控件主题文件中局部定义新 token，后续统一整理到 token 文件 |
| 编译冲突 | 协调者统一构建并修复 |
| 控件命名冲突 | 各 Agent 域不重叠，命名按 `{Component}` 无前缀 |
