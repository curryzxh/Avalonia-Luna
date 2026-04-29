# Luna 阶段 0：移动优先主题基础设施规划

本文细化 `阶段 0：主题基础设施` 的目标、边界、资源结构和验收标准。阶段 0 只建立移动优先的 TDesign 风格 token 与主题基础，不实现具体控件模板。

## 目标

- 建立一套移动优先、桌面可复用的 Luna 设计 token。
- 建立浅色/深色主题资源结构，为后续控件皮肤化提供统一输入。
- 在移动示例项目中提供 token preview，用于验证资源引用、主题切换和视觉一致性。
- 明确后续控件接入 token 的命名和使用约定，避免控件内硬编码颜色、字号、圆角和尺寸。

## 范围

阶段 0 包含：

- TDesign 风格 token 体系。
- 移动端默认尺寸规范。
- 浅色/深色主题资源。
- 主题资源组织方式。
- 示例项目中的 token preview 页面或区域。
- 后续控件皮肤化的接入约定。

阶段 0 不包含：

- `Button`、`Input`、`Switch` 等具体控件模板实现。
- `Badge`、`Cell`、`Toast` 等 Luna 自定义控件开发。
- 平台服务、手势控件或复杂弹层状态机。
- 桌面端 compact 主题覆盖；桌面端可复用 token，但不作为阶段 0 的主要验收对象。

## Token 分组

### Color

颜色 token 以移动端 TDesign 语义为基础，但使用 Avalonia 资源命名，不直接复用 CSS 变量名。

- Brand：品牌色阶，例如 `Luna.Color.Brand.1` 到 `Luna.Color.Brand.9`，其中 `Luna.Color.Brand.6` 作为默认主色。
- Success：成功状态色阶。
- Warning：警告状态色阶。
- Error：错误与危险操作色阶。
- Text：主文本、次级文本、占位文本、禁用文本、反色文本。
- Background：页面背景、容器背景、组件背景、激活背景、禁用背景。
- Border：普通边框、弱边框、强边框、分割线。
- Mask：普通遮罩、强遮罩、轻遮罩。

### Typography

字体 token 优先覆盖移动端常见文本层级：

- Title：页面标题、模块标题、弹层标题。
- Body：正文大、中、小。
- Mark：徽标、辅助说明、计数器。
- FontWeight：regular、medium、semibold。
- LineHeight：移动端文本行高应高于桌面紧凑布局，优先保证可读性。

### Radius

圆角 token：

- `Luna.Radius.Small`
- `Luna.Radius.Default`
- `Luna.Radius.Large`
- `Luna.Radius.ExtraLarge`
- `Luna.Radius.Circle`

### Spacing

间距 token 使用 4px 基础步进：

- `Luna.Spacing.1` = 4
- `Luna.Spacing.2` = 8
- `Luna.Spacing.3` = 12
- `Luna.Spacing.4` = 16
- `Luna.Spacing.5` = 20
- `Luna.Spacing.6` = 24
- `Luna.Spacing.8` = 32

### Size

尺寸 token 以移动触控为默认：

- `Luna.Size.TouchTarget`：不低于 44px。
- `Luna.Size.Control.Small`
- `Luna.Size.Control.Medium`
- `Luna.Size.Control.Large`
- `Luna.Size.Icon.Small`
- `Luna.Size.Icon.Medium`
- `Luna.Size.Icon.Large`

### Shadow

阴影 token 覆盖移动端常见浮层和卡片：

- `Luna.Shadow.Card`
- `Luna.Shadow.Popup`
- `Luna.Shadow.Floating`

### Motion

动效 token：

- `Luna.Motion.Fast`
- `Luna.Motion.Base`
- `Luna.Motion.Slow`

阶段 0 只定义时长和缓动命名，不实现具体控件动画。

## 资源结构

当前实现结构：

```text
src/Luna.Mobile/Themes/
  Index.axaml
  Light.axaml
  Dark.axaml
  Tokens/
    Color.axaml
    Typography.axaml
    Radius.axaml
    Spacing.axaml
    Size.axaml
    Shadow.axaml
    Motion.axaml
```

职责划分：

- `Tokens/*.axaml` 定义跨主题通用 token、语义 brush 和 token key 的默认值。
- `Light.axaml` 定义浅色主题颜色资源和浅色专属覆盖，资源放在 `ThemeDictionaries` 的 `Light` 字典中。
- `Dark.axaml` 定义深色主题颜色资源和深色专属覆盖，资源放在 `ThemeDictionaries` 的 `Dark` 字典中。
- `Index.axaml` 作为移动主题入口，统一 include token 与主题资源。
- `Generic.axaml` 只保留控件默认样式入口，并通过 `DynamicResource` 引用 token。

示例项目当前实现：

```text
samples/Luna.Mobile.Sample/
  Views/
    MainView.axaml
    MainView.axaml.cs
```

当前先把 token preview 放进 `MainView.axaml` 的独立区域；后续示例工作台建立后再迁移到独立 `ThemePreviewView`。

## 命名约定

- 所有 Luna 资源键统一使用 `Luna.` 前缀。
- 不直接使用 TDesign CSS 变量名作为 Avalonia 资源键。
- TDesign 来源和映射关系写入文档，代码资源使用 Avalonia 风格命名。
- 颜色资源建议使用语义 + 色阶，例如 `Luna.Color.Brand.6`。
- 组件样式后续引用语义 token，不直接引用固定色值。

示例：

```xml
<Color x:Key="Luna.Color.Brand.6">#0052D9</Color>
<SolidColorBrush x:Key="Luna.Brush.Brand" Color="{DynamicResource Luna.Color.Brand.6}" />
<x:Double x:Key="Luna.Size.TouchTarget">44</x:Double>
<CornerRadius x:Key="Luna.Radius.Default">6</CornerRadius>
```

## 设计原则

- 移动端优先：token 默认值优先满足触控、窄屏、安全区和高密度屏幕。
- 桌面端复用：桌面端可以引用相同 token，但后续允许建立 compact 覆盖。
- 资源驱动：控件模板不得散落硬编码颜色、圆角、字号和尺寸。
- 主题可切换：浅色与深色主题必须使用同一资源键，不在控件模板中写主题分支。
- 渐进扩展：阶段 0 只定义首批通用 token，后续控件确实需要时再补充组件级 token。

## 后续控件接入约定

- 阶段 1 的 `Button`、`TextBox`、`CheckBox`、`RadioButton`、`ToggleSwitch`、`Slider`、`NumericUpDown`、`ProgressBar` 必须优先使用阶段 0 token。
- 控件级资源键命名建议为 `Luna.ControlName.Property.State`，例如 `Luna.Button.Background.Primary`.
- 组件级 token 只能引用基础 token，不应直接写死色值。
- 如果 TDesign 组件语义和 Avalonia 原生控件不匹配，应先更新 `tdesign-mobile-component-analysis.md`，再进入控件设计。

## 验收标准

文档验收：

- `docs/README.md` 能找到本文档入口。
- `docs/luna-control-roadmap.md` 阶段 0 与本文档不冲突。
- 本文档明确阶段 0 产出、非目标、资源结构、token 命名和验收标准。

实现验收：

- `dotnet build Luna.sln --no-restore` 通过。
- `Luna.Mobile.Sample` 可展示移动 token preview。
- 浅色/深色主题资源均可被示例引用。
- 修改品牌色 token 后，示例预览能集中变化。
