# Luna 控件库阶段规划

本文基于 [TDesign Mobile 组件对照分析](tdesign-mobile-component-analysis.md)，用于后续拆分 Luna 控件库建设任务。

## 阶段 0：主题基础设施

目标：先建立可持续扩展的主题与 token 结构，避免每个控件各写一套硬编码样式。

细化规划：[Luna 阶段 0：移动优先主题基础设施规划](luna-phase-0-mobile-theme-plan.md)。

阶段边界：阶段 0 只建立 token、浅色/深色主题资源结构、移动尺寸规范和示例 token preview，不实现 `Button`、`Input`、`Switch` 等具体控件模板。

建议内容：

- 建立 `Luna.Theme` 或在现有 `Luna.Mobile` / `Luna.Desktop` 中统一组织 token 资源。
- 定义 TDesign 风格 token：品牌色、功能色、文本色、背景色、边框色、圆角、间距、字号、阴影、动效时间。
- 建立浅色/深色主题资源。
- 建立控件尺寸枚举和视觉枚举，例如 `LunaSize`、`LunaTheme`、`LunaVariant`、`LunaShape`。
- 示例项目中提供 token 预览页。

验收标准：

- `Luna.Desktop.Sample` 和 `Luna.Mobile.Sample` 可同时引用主题。
- 修改品牌色 token 后，至少 Button/Input/Switch 的示例能联动变化。
- `dotnet build Luna.sln --no-restore` 通过。

## 阶段 1：Base/Form 原生控件皮肤化

目标：用 Avalonia 原生控件快速覆盖 TDesign 最常用基础输入控件。

细化规划：[Luna 阶段 1：移动 MVP Base/Form 原生控件皮肤化规划](luna-phase-1-mobile-base-form-plan.md)。

首轮范围：移动 MVP，先覆盖 `Button`、`TextBox`、`ToggleSwitch`、`CheckBox`、`RadioButton`，不新增业务型自定义控件。

移动 MVP 控件：

- `Button`
- `TextBox` as Input/Textarea
- `CheckBox`
- `RadioButton`
- `ToggleSwitch`

阶段 1 后续扩展：

- `Slider`
- `NumericUpDown` as Stepper
- `ProgressBar`

实施重点：

- 每个控件优先写 `ControlTheme` 和样式资源。
- 尽量不新增自定义控件类，除非 TDesign 语义无法由原生属性表达。
- 覆盖默认、悬停、按下、焦点、禁用、错误或选中态。
- 移动端尺寸需满足触控目标，桌面端尺寸可更紧凑。

验收标准：

- 每个控件在两个示例项目中都有最小展示。
- 浅色/深色主题可读。
- 窄屏下无明显溢出。

## 阶段 2：轻量包装控件

目标：提供 TDesign 语义明确、但可由 Avalonia 基础能力组合的 Luna 控件。

首批控件：

- `LunaBadge`
- `LunaTag`
- `LunaAvatar`
- `LunaAvatarGroup`
- `LunaCell`
- `LunaCellGroup`
- `LunaEmpty`
- `LunaGrid`
- `LunaGridItem`

实施重点：

- 使用 Avalonia 属性公开状态，不引入业务概念。
- 模板化控件优先，默认外观放在 `ControlTheme` 中。
- 内容区域使用 `ContentControl` 或 `ItemsControl`，保留自定义内容能力。
- 示例中展示默认用法、状态变化、禁用态和数据绑定。

验收标准：

- 控件可在 AXAML 中直接使用。
- 支持绑定和主题切换。
- 示例覆盖桌面和移动两端最小场景。

## 阶段 3：反馈与导航组件

目标：建立移动应用常用的反馈、弹层和导航组件。

首批控件：

- `LunaPopup`
- `LunaToast`
- `LunaDialog`
- `LunaActionSheet`
- `LunaNoticeBar`
- `LunaNavBar`
- `LunaTabBar`
- `LunaSteps`

实施重点：

- 弹层类组件统一处理遮罩、关闭触发源、动画时长和层级。
- Toast/Dialog 需要考虑服务化调用，但首版可以先提供控件形态。
- NavBar/TabBar 需要考虑移动端安全区。
- 桌面端可提供兼容样式，但不强制复刻移动交互。

验收标准：

- Popup/Dialog/ActionSheet 可以打开、关闭，并回传关闭来源。
- Toast 可在示例中触发显示和自动关闭。
- NavBar/TabBar 在 Android 示例中布局正常。

## 阶段 4：复杂移动交互控件

目标：补齐 TDesign Mobile 中无法简单皮肤化的核心移动交互。

候选控件：

- `LunaSwipeCell`
- `LunaPullDownRefresh`
- `LunaPicker`
- `LunaCascader`
- `LunaIndexes`
- `LunaSwiper`
- `LunaImageViewer`
- `LunaUpload`

实施重点：

- 先做最小可用交互，再扩展边界状态。
- 手势控件需要明确阈值、速度、取消路径和冲突处理。
- 平台服务类控件需要抽象接口，避免控件层直接依赖 Android/iOS 细节。
- 复杂控件必须配套示例和手测清单。

验收标准：

- Android 示例中触控体验可用。
- 桌面端至少能以鼠标模拟核心交互，或明确标注仅移动端支持。
- 构建通过，并记录无法自动化验证的手测项。
