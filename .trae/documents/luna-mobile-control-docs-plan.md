# Luna.Mobile 控件使用文档计划

## Summary

- 目标：为 `Luna.Mobile` 当前已经实现且可被使用者直接消费的控件补齐“使用优先”的 Markdown 文档。
- 输出：在 `docs/controls/` 下建立 1 份控件索引和 44 份单控件文档，并在 `docs/README.md` 增加入口。
- 范围：同时覆盖两类组件：
  - `src/Luna.Mobile/Controls/` 中的 Luna 自定义控件、宿主控件与静态入口相关组件。
  - `src/Luna.Mobile/Themes/Controls/` 中已皮肤化并被当作组件使用的 Avalonia 原生控件。
- 非范围：内部辅助类型不单独成文，例如 `OverlayHostAnimationHelper`、`RateGlyphs`、`LoadingIndicator`、`WatermarkSurface`；它们只在所属主控件文档中按需提及。

## Current State Analysis

### 现有文档状态

- `docs/` 目前只有规划、分析、复刻指南，没有任何面向最终使用者的控件使用文档。
- [docs/README.md](file:///Users/curryzxh/Documents/work/avalonia/luna/docs/README.md) 当前只索引规划类文档，需要新增“使用文档”分区。
- 根文档 [README.md](file:///Users/curryzxh/Documents/work/avalonia/luna/README.md) 只覆盖仓库结构和构建命令，不适合作为控件手册入口。

### 事实来源

- 控件实现主来源：`src/Luna.Mobile/Controls/*.cs`
- 原生控件皮肤化主来源：`src/Luna.Mobile/Themes/Controls/*.axaml`
- 皮肤入口总表：`src/Luna.Mobile/Themes/Generic.axaml`
- 当前能力示例主来源：`samples/Luna.Mobile.Sample/Views/*DemoView.axaml`
- 当前面向使用者暴露的组件目录主来源：`samples/Luna.Mobile.Sample/ViewModels/MainViewModel.cs`

### 当前仓库中适合成文的组件对象

- 以“使用者视角”而不是“public 类型逐个罗列”组织文档。
- 仅为仓库中已有实现、且能从源码或 sample 中提炼出稳定用法的组件建文档。
- 不为 `MainViewModel` 中尚未实现的占位项建文档，例如 `Icon`、`Layout`、`BackTop`、`Tabs`、`TabBar`、`Calendar`、`Form`、`Textarea`、`TreeSelect`、`Upload`、`Grid`、`Image`、`Popup` 等。

### 本次计划覆盖的文档清单

- **基础**
  - `button.md`
  - `divider.md`
  - `fab.md`
  - `link.md`
- **导航**
  - `drawer.md`
  - `indexes.md`
  - `navbar.md`
  - `steps.md`
- **输入**
  - `cascader.md`
  - `checkbox.md`
  - `datetimepicker.md`
  - `input.md`
  - `picker.md`
  - `radio.md`
  - `rate.md`
  - `searchbar.md`
  - `slider.md`
  - `stepper.md`
  - `switch.md`
- **数据展示**
  - `avatar.md`
  - `badge.md`
  - `cell.md`
  - `collapse.md`
  - `empty.md`
  - `footer.md`
  - `imageviewer.md`
  - `progress.md`
  - `segmented.md`
  - `skeleton.md`
  - `tag.md`
  - `watermark.md`
- **反馈**
  - `actionsheet.md`
  - `dialog.md`
  - `dropdownmenu.md`
  - `guide.md`
  - `loading.md`
  - `message.md`
  - `noticebar.md`
  - `overlay.md`
  - `popover.md`
  - `pulldownrefresh.md`
  - `swipecell.md`
  - `toast.md`

### 需要在主文档内合并说明的配套类型

- `cell.md`：`Cell` + `CellGroup`
- `steps.md`：`Steps` + `StepItem` + `StepsLayout` + `StepsTheme` + `StepStatus`
- `tag.md`：`Tag` + `CheckTag` + `TagTheme` + `TagVariant` + `TagShape` + `TagSize`
- `segmented.md`：`Segmented` + `SegmentedItem`
- `footer.md`：`Footer` + `FooterLink`
- `guide.md`：`GuideHost` + `GuideStep` + `GuideMode` + `GuidePlacement`
- `dropdownmenu.md`：`DropdownMenu` + `DropdownMenuItem` + `DropdownMenuOption`
- `actionsheet.md`：`ActionSheetHost` + `ActionSheetOptions` + `ActionSheetItem` + `ActionSheetCloseReason`
- `dialog.md`：`Dialog` + `DialogHost` + `DialogOptions` + `DialogButtonOptions`
- `drawer.md`：`Drawer` + `DrawerHost` + `DrawerOptions`
- `message.md`：`Message` + `MessageHost` + `MessageCard` + `MessageOptions`
- `toast.md`：`Toast` + `ToastHost` + `ToastOptions`
- `picker.md`：`Picker` + `PickerHost` + `PickerOptions` + `PickerColumn`
- `datetimepicker.md`：`DateTimePicker` + `DateTimePickerHost` + `DateTimePickerOptions`
- `cascader.md`：`Cascader` + `CascaderHost` + `CascaderOptions` + `CascaderOption`

## Assumptions & Decisions

- 决定 1：延续上一轮已确认偏好，文档采用“每控件单文件”形式，且以“使用优先”而非完整 API 参考为主。
- 决定 2：样式化原生控件必须纳入范围，并按组件语义命名，而不是按底层 Avalonia 类型命名。
- 决定 3：文档目录固定为 `docs/controls/`，文件名使用小写 kebab/单词形式，与组件语义对应，例如 `navbar.md`、`searchbar.md`、`pulldownrefresh.md`。
- 决定 4：每篇文档只描述当前仓库已经落地的能力，不把 TDesign 对标目标或未来规划写成“已支持”。
- 决定 5：对于“静态入口 + 宿主控件”模式的组件，文档以用户实际调用入口为主，同时明确宿主依赖与页面放置要求。
- 决定 6：对 sample 名称与类型名不完全一致的组件，文档标题以组件名为准，示例章节中再注明 sample 文件名：
  - `NavBar` -> `navbar.md`
  - `SearchBar` -> `searchbar.md`，对应 `SearchDemoView`
  - `CheckBox` -> `checkbox.md`
  - `ToggleSwitch` -> `switch.md`
  - `TextBox` -> `input.md`
- 决定 7：内部辅助类型不单独成文，但会在“实现方式”或“注意事项”中说明它们的角色。

## Proposed Changes

### 1. 建立控件文档入口

- 新增目录：`docs/controls/`
- 新增文件：`docs/controls/README.md`
- 修改文件：`docs/README.md`

### 2. `docs/controls/README.md` 的内容结构

- 说明该目录面向谁、阅读顺序是什么、文档如何取材。
- 按 `基础 / 导航 / 输入 / 数据展示 / 反馈` 五组列出全部 44 篇控件文档。
- 对以下文档增加“基于 Avalonia 原生控件”的说明：
  - `button.md`
  - `link.md`
  - `input.md`
  - `switch.md`
  - `checkbox.md`
  - `radio.md`
  - `slider.md`
  - `progress.md`
  - `collapse.md`
- 在索引顶部说明：未出现在本目录中的 `MainViewModel` 菜单项，表示仓库当前尚无对应实现或未形成稳定用法。

### 3. `docs/README.md` 的调整方式

- 保留当前规划类文档索引和维护约定。
- 在“文档索引”前部新增“使用文档”分区，链接到 `docs/controls/README.md`。
- 不改写现有规划文档介绍，只补一层新的入口，避免影响既有文档定位。

### 4. 每篇控件文档统一使用的模板

每篇文档按以下顺序编写：

1. 标题 + 一句话简介
2. 何时使用
3. 当前实现方式
   - 说明是 Luna 自定义控件还是原生控件皮肤化
   - 指向真实实现文件
4. 最小示例
   - 以 AXAML 为主
   - 仅在组件依赖静态入口或 code-behind 时补 C# 示例
5. 常用属性 / 事件 / 样式类
   - 仅记录高频、对使用者有决策价值的成员
6. 配套类型 / 组合方式
7. 对应 sample
8. 已知限制 / 注意事项

### 5. 取材与写作规则

- 取材优先级固定为：
  1. `src/Luna.Mobile/Controls/*.cs`
  2. `src/Luna.Mobile/Themes/Controls/*.axaml`
  3. `samples/Luna.Mobile.Sample/Views/*DemoView.axaml`
  4. `docs/tdesign-mobile-sample-authoring-guide.md`
- 样式化原生控件必须明确写出底层控件及可用 `Classes`。
- 宿主型组件必须写清楚页面上需要放置什么控件，以及静态入口如何找到 `Current`。
- 如果 sample 只是演示占位数据或局部视觉效果，文档要标明它是示例，不把其误写成通用 API。
- 如果实现与 sample 展示存在边界差异，以当前源码为准，并在“已知限制”中点明。

### 6. 按组件类型的写法要求

- `button.md`
  - 以 `Button.axaml` 的 `Classes` 组合为主线，覆盖主题、变体、尺寸、形状和禁用态。
- `link.md`
  - 明确它基于 `HyperlinkButton`，覆盖 `primary / danger / success / warning / underline / small / medium / large`。
- `input.md`
  - 明确它基于 `TextBox` 的 Luna 皮肤，重点写输入态、占位、前后缀及 sample 中实际展示的常用写法。
- `collapse.md`
  - 明确它基于 `Expander`，重点写基础展开、卡片态和带操作区的变体。
- `cell.md`
  - 合并写 `Cell` 与 `CellGroup`，说明 `LeftContent`、`RightContent`、`RightText`、`ShowArrow`。
- `steps.md`
  - 主文档写 `Steps`，并说明 `Current`、`IsReadOnly`、`StepItem.Value` 的匹配逻辑。
- `tag.md`
  - 同时覆盖 `Tag` 与 `CheckTag`，并按主题、变体、形状、尺寸组织。
- `segmented.md`
  - 写清 `SegmentedItem` 的内容模型和禁用项行为。
- `actionsheet.md`
  - 以 `ActionSheetHost`/静态入口的打开方式为主，补充 `List/Grid` 主题、关闭原因和选项模型。
- `dialog.md`
  - 以静态 `Dialog.Show()` 的调用姿势为主，补充 `DialogHost` 放置要求和按钮布局。
- `drawer.md`
  - 以静态 `Drawer.Show()` 为主，补充 `DrawerHost` 依赖和 `Placement`/遮罩行为。
- `message.md`
  - 以静态 `Message.Info/Success/Warning/Error()` 为主，补充 `MessageHost` 和 `MessageCard` 的关系。
- `toast.md`
  - 以静态 `Toast.Show()` 为主，补充 `ToastHost` 的页面依赖、主题、方向和位置。
- `picker.md`
  - 写清 `PickerOptions` 与 `PickerColumn` 的最小数据结构，以及 `PickerHost` 的职责。
- `datetimepicker.md`
  - 写清模式、步进配置、确认/关闭事件模型，以及与 `DateTimePickerHost` 的关系。
- `cascader.md`
  - 写清层级选项模型、关闭原因、选中事件和宿主关系。

### 7. 实施批次

#### 第 1 批：入口与模板校准

- 建立 `docs/controls/README.md`
- 更新 `docs/README.md`
- 先完成 5 篇代表性文档：
  - `button.md`
  - `cell.md`
  - `link.md`
  - `steps.md`
  - `toast.md`

#### 第 2 批：样式化原生控件

- `checkbox.md`
- `radio.md`
- `switch.md`
- `input.md`
- `slider.md`
- `progress.md`
- `collapse.md`

#### 第 3 批：高频展示控件

- `avatar.md`
- `badge.md`
- `divider.md`
- `fab.md`
- `footer.md`
- `empty.md`
- `tag.md`
- `watermark.md`
- `loading.md`
- `noticebar.md`
- `overlay.md`

#### 第 4 批：导航与反馈容器

- `drawer.md`
- `navbar.md`
- `indexes.md`
- `actionsheet.md`
- `dialog.md`
- `dropdownmenu.md`
- `guide.md`
- `message.md`
- `popover.md`

#### 第 5 批：复杂交互与数据录入

- `cascader.md`
- `datetimepicker.md`
- `picker.md`
- `rate.md`
- `searchbar.md`
- `stepper.md`
- `segmented.md`
- `skeleton.md`
- `swipecell.md`
- `imageviewer.md`
- `pulldownrefresh.md`

### 8. 执行时的具体文件动作

- 新建 `docs/controls/README.md`
- 新建上述 44 个 `docs/controls/*.md`
- 修改 `docs/README.md`
- 不修改控件源码、样式源码和 sample 代码；本次任务只产出文档

## Verification Steps

### 文档结构验证

- 确认 `docs/controls/README.md` 列出的每个文件都实际存在。
- 抽查所有文档都包含以下核心章节：
  - 简介
  - 何时使用
  - 最小示例
  - 常用属性 / 事件 / 样式类
  - 对应 sample
  - 已知限制 / 注意事项

### 事实一致性验证

- 每篇文档至少能回溯到 1 个真实源码文件和 1 个真实 sample 文件。
- 样式化原生控件文档必须明确底层类型：
  - `Button`
  - `HyperlinkButton`
  - `TextBox`
  - `ToggleSwitch`
  - `CheckBox`
  - `RadioButton`
  - `Slider`
  - `ProgressBar`
  - `Expander`
- 宿主型组件文档必须明确写出宿主控件名和静态入口名，不得只写其中一侧。

### 链接与命名验证

- 检查 `docs/README.md` 到 `docs/controls/README.md` 的相对链接正确。
- 检查 `docs/controls/README.md` 中全部相对链接、文件名大小写与仓库实际一致。
- 检查文档标题、文件名、组件名三者一致，允许“标题是 Search，文件名是 `searchbar.md`”这类经过计划明确的命名映射。

### 额外说明

- 本次为纯文档任务，不需要运行构建；验证重点是链接正确、内容事实准确、模板一致。

## Executor Notes

- 执行时先读本计划文件，再按批次推进，避免边写边重新定义范围。
- 如果某个控件在实现文件和 sample 中表现不一致，以实现文件为准，在文档中显式标注差异。
- 如果发现某个计划内控件实际没有稳定实现，应停止新增该控件文档，并在索引或相邻文档中说明当前状态，而不是猜测写法。
