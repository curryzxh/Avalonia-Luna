# Luna.Desktop 控件库并行 Agent 协调计划

- 状态：✅ 全部完成，构建通过
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

| Agent | 域 | P0 控件 | P1 控件 | 深化已有 | 状态 |
|---|---|---|---|---|---|
| A | Base + Layout | Icon, Link, Typography, Divider, Space, Descriptions | Guide, ImageViewer | DesktopBadge | ✅ 已完成 |
| B | Form | Textarea, InputNumber, Select, AutoComplete, Form | DatePicker, TimePicker, Upload, Cascader, Rate, ColorPicker | Input, CheckBox, Radio, Switch | ✅ 已完成 |
| C | Data | Card, Tooltip, Skeleton, Collapse, Timeline, Statistic, Calendar, Table | Tree, List, Comment, QRCode, Watermark, BackTop, Image | Progress, Tag, Avatar, Empty | ✅ 已完成 |
| D | Feedback | Alert, Dialog, Drawer, Message, Loading, Popup, Popconfirm | Swiper | Notification | ✅ 已完成 |
| E | Navigation | Tabs, Menu, Dropdown, Pagination, Breadcrumb, Steps | Anchor, Affix, StickyTool | Slider | ✅ 已完成 |

## 风险与缓解

| 风险 | 缓解措施 |
|---|---|
| 多 Agent 同时修改 ControlSampleCatalog.cs | 各 Agent 只追加自己的登记项，不删除其他 Agent 的条目；协调者最终合并 |
| 多 Agent 同时修改 Generic.axaml | 同上，各 Agent 只追加 StyleInclude |
| Token 不够用 | 各 Agent 在控件主题文件中局部定义新 token，后续统一整理到 token 文件 |
| 编译冲突 | 协调者统一构建并修复 |
| 控件命名冲突 | 各 Agent 域不重叠，命名按 `{Component}` 无前缀 |
