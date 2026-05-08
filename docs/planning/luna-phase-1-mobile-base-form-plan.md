# Luna 阶段 1：移动 MVP Base/Form 原生控件皮肤化规划

本文细化 `阶段 1：Base/Form 原生控件皮肤化` 的首轮移动 MVP。阶段 1 不新增业务型自定义控件，优先用 Avalonia 原生控件和 `ControlTheme` 覆盖最常用移动表单路径。

## 目标

- 基于阶段 0 的 `Luna.*` token，建立移动优先的 Base/Form 控件皮肤化样板。
- 首轮覆盖 `Button`、`TextBox`、`ToggleSwitch`、`CheckBox`、`RadioButton`。
- 验证 Avalonia 原生控件通过样式即可承载 TDesign Mobile 的基础视觉语义。
- 为后续 `Slider`、`NumericUpDown`、`ProgressBar` 扩展提供同一套资源组织和示例验收方式。

## 范围

阶段 1 移动 MVP 包含：

- Avalonia 原生控件主题样式。
- 移动端触控尺寸、状态色、文本色、边框、焦点和禁用态。
- 移动示例项目中的 Base/Form preview 区域。
- 后续控件样式拆分和资源命名约定。

阶段 1 移动 MVP 不包含：

- `Badge`、`Cell`、`Toast` 等 Luna 自定义控件。
- `Input` 的 label、suffix、tips、clearable 等完整 TDesign 包装控件语义。
- `Slider`、`NumericUpDown`、`ProgressBar` 的首轮实现；这些作为阶段 1 后续扩展。
- 桌面端 compact 主题覆盖。

## MVP 控件映射

| TDesign Mobile | Avalonia 原生控件 | 阶段 1 MVP 策略 |
| --- | --- | --- |
| `button` | `Button` | 通过样式覆盖移动尺寸、圆角、品牌色和禁用态 |
| `input` | `TextBox` | 先覆盖单行输入框基础视觉、焦点和禁用态 |
| `textarea` | `TextBox` | 通过 `AcceptsReturn` 场景复用 TextBox 样式 |
| `switch` | `ToggleSwitch` | 覆盖开/关/禁用状态和移动触控尺寸 |
| `checkbox` | `CheckBox` | 覆盖选中、未选中、禁用和焦点状态 |
| `radio` | `RadioButton` | 覆盖选中、未选中、禁用和焦点状态 |

## 主题资源结构

当前实现使用 `src/Luna.Mobile/Themes/Generic.axaml` 作为控件样式入口，并按控件拆分：

```text
src/Luna.Mobile/Themes/
  Generic.axaml
  Controls/
    Button.axaml
    TextBox.axaml
    ToggleSwitch.axaml
    CheckBox.axaml
    RadioButton.axaml
```

`Generic.axaml` include 各控件样式文件。所有样式文件必须引用阶段 0 的 `Luna.*` token，不写非必要硬编码颜色、字号、圆角和尺寸。

## 样式覆盖目标

### Button

- 默认按钮：使用容器背景、默认边框和主文本色。
- Primary 按钮：使用 `Luna.Brush.Brand` 和 `Luna.Brush.Text.Anti`。
- Light 与 Danger 语义先规划资源键，首轮可不完整实现。
- 尺寸不低于 `Luna.Size.TouchTarget`。
- 覆盖普通、按下、禁用、焦点状态。

建议通过 style class 表达视觉语义：

- 默认：`<Button />`
- Primary：`<Button Classes="primary" />`
- Danger：`<Button Classes="danger" />`
- Light：`<Button Classes="light" />`

### TextBox

- 单行输入：默认高度不低于 `Luna.Size.TouchTarget`。
- 多行输入：复用 TextBox 样式，允许通过 `AcceptsReturn="True"` 展示 textarea 场景。
- 使用 `Luna.Brush.Background.Container`、`Luna.Brush.Border.Default`、`Luna.Brush.Text.Primary`。
- 焦点态使用品牌色边框。
- 禁用态使用禁用背景和禁用文本。
- 错误态先规划 class，例如 `Classes="error"`；完整验证逻辑不在 MVP。

### ToggleSwitch

- 使用 TDesign Mobile 的触控优先尺寸。
- 开启态使用 `Luna.Brush.Brand`。
- 关闭态使用组件背景或弱边框。
- 禁用态降低对比度但保持可辨识。
- 保持原生 `IsChecked` 绑定和键盘/辅助功能行为。

### CheckBox 与 RadioButton

- 复用 Avalonia 原生选择逻辑。
- 控件整体点击区域不低于 `Luna.Size.TouchTarget`。
- 选中态使用品牌色。
- 禁用态使用禁用文本和禁用边框。
- 焦点态保持可见，不移除可访问性反馈。

## 示例规划

阶段 1 MVP 当前在 `samples/Luna.Mobile.Sample/Views/MainView.axaml` 增加 Base/Form preview 区域。

示例应覆盖：

- Button：default、primary、disabled。
- TextBox：普通输入、placeholder、disabled、textarea。
- ToggleSwitch：off、on、disabled。
- CheckBox：unchecked、checked、disabled。
- RadioButton：同组单选、disabled。

后续示例工作台建立后，再迁移到独立 `BaseFormPreviewView` 或分类示例页面。

## 后续扩展

阶段 1 后续再补：

- `Slider`
- `NumericUpDown` as Stepper
- `ProgressBar`

扩展时继续沿用本文的资源组织、token 接入和示例验收方式。

## 验收标准

文档验收：

- `docs/README.md` 可找到本文档入口。
- `docs/planning/luna-control-roadmap.md` 阶段 1 与本文档一致。
- 本文档明确 MVP 控件、非目标、资源组织、token 接入、示例验收。

实现验收：

- `dotnet build Luna.sln --no-restore` 通过。
- `Luna.Mobile.Sample` 可展示 MVP 控件皮肤化结果。
- 浅色/深色主题下控件文本和边框可读。
- 控件最小触控高度不低于 `Luna.Size.TouchTarget`。
- 新增样式文件不应出现非必要硬编码色值；组件级资源应引用 `Luna.*` 基础 token。
