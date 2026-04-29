# TDesign Mobile 组件对照分析

本文用于指导 Luna 移动端控件库的建设策略：先判断 TDesign Mobile 组件在 Avalonia 中是否已有可复用基础能力；已有则优先做 TDesign 风格皮肤，没有或语义不匹配时再开发 Luna 自定义控件。

## 来源与结论

- TDesign MCP 查询范围：`mobile-react` 与 `mobile-vue`。
- 两套移动端组件清单一致，可按 TDesign Mobile 的 `base`、`form`、`message`、`data`、`navigation` 五组规划。
- Luna 当前目标：统一桌面端和移动端 Avalonia 控件风格，但移动端交互应优先参考 TDesign Mobile。
- Avalonia 对照版本：当前仓库使用 Avalonia `12.0.1`。

## 分类策略

### A. Avalonia 已有控件，优先做 TDesign 皮肤

这些组件的核心交互、状态和可访问性已有 Avalonia 基础控件支撑。Luna 初期应优先提供 `ControlTheme`、样式资源和设计 token，而不是重写控件逻辑。

| TDesign Mobile | Avalonia 基础能力 | Luna 建议 |
| --- | --- | --- |
| `button` | `Button` | 皮肤化，补齐 `theme`、`variant`、`size`、`shape` 风格映射 |
| `checkbox` | `CheckBox` | 皮肤化 |
| `radio` | `RadioButton` | 皮肤化 |
| `switch` | `ToggleSwitch` | 皮肤化，按 TDesign 尺寸和颜色重做模板 |
| `input` | `TextBox` | 皮肤化，必要时提供带 label/suffix/tips 的包装控件 |
| `textarea` | `TextBox` | 皮肤化，多行模式和计数器可能需要包装 |
| `slider` | `Slider` | 皮肤化，范围滑块另行评估 |
| `stepper` | `NumericUpDown` | 皮肤化或轻包装 |
| `calendar` | `Calendar` | 皮肤化 |
| `date-time-picker` | `DatePicker` / `TimePicker` | 皮肤化，移动端底部选择器体验另行扩展 |
| `progress` | `ProgressBar` | 皮肤化 |
| `image` | `Image` | 皮肤化或包装加载失败状态 |
| `tabs` | `TabControl` | 皮肤化，移动端 swipeable 需要扩展 |
| `drawer` | `SplitView` | 皮肤化，弹层抽屉另行包装 |
| `popup` / `popover` | `Popup` / `ToolTip` | 包装 Avalonia 弹层能力 |
| `list` | `ItemsControl` / `ListBox` | 皮肤化或按场景包装 |
| `table` | `Avalonia.Controls.DataGrid` | 桌面优先；移动端不作为首批重点 |

### B. Avalonia 可组合实现，但建议提供 Luna 包装控件

这些组件可以用 Avalonia 原生布局、内容控件、弹层或 ItemsControl 组合出来，但 TDesign 的组件语义明确，直接暴露 Luna API 更利于统一使用。

| TDesign Mobile | 推荐 Luna 控件 | 原因 |
| --- | --- | --- |
| `badge` | `LunaBadge` | 数字、红点、封顶、偏移和附着内容需要统一 API |
| `tag` | `LunaTag` | 状态、主题、尺寸和可关闭交互需要统一 |
| `avatar` / `avatar-group` | `LunaAvatar` / `LunaAvatarGroup` | 图片失败、形状、层叠和折叠数量需要封装 |
| `cell` / `cell-group` | `LunaCell` / `LunaCellGroup` | 移动端列表项语义、左右图标、箭头、说明文字较固定 |
| `empty` | `LunaEmpty` | 图标、描述、操作按钮组合固定 |
| `result` | `LunaResult` | 成功、失败、警告等结果页模式固定 |
| `grid` / `grid-item` | `LunaGrid` / `LunaGridItem` | 宫格入口、列数、边框、卡片模式需要统一 |
| `navbar` | `LunaNavBar` | 标题、左右区域、安全区、固定顶部是移动端常用模式 |
| `tab-bar` | `LunaTabBar` / `LunaTabBarItem` | 底部导航、安全区、徽标和固定定位需要封装 |
| `steps` | `LunaSteps` / `LunaStepItem` | 状态、方向、线条和图标语义明确 |
| `toast` | `LunaToast` / `LunaToastHost` | 生命周期、队列、位置和图标状态需要服务化 |
| `message` | `LunaMessage` / `LunaMessageHost` | 轻量反馈，和 toast/notification 区分 |
| `notice-bar` | `LunaNoticeBar` | 顶部提示、滚动、关闭和操作入口需要统一 |
| `dialog` | `LunaDialog` / `LunaDialogService` | 遮罩、按钮布局、确认取消生命周期需要封装 |
| `action-sheet` | `LunaActionSheet` | 底部弹层、取消按钮、列表/宫格动作项需要封装 |
| `loading` | `LunaLoading` / `LunaLoadingOverlay` | 动效、遮罩和局部加载状态需要统一 |
| `collapse` | `LunaCollapse` / `LunaCollapseItem` | 展开状态、标题和内容过渡需要统一 |
| `footer` | `LunaFooter` | 移动端页脚固定模式 |
| `sticky` | `LunaSticky` | 滚动容器和吸顶偏移需要封装 |
| `back-top` | `LunaBackTop` | 绑定滚动容器、显示阈值和滚动回顶行为 |

### C. 需求不匹配，需要重点自定义控件开发

这些组件的关键价值在移动交互、状态机、平台能力或复杂布局，不能只靠 Avalonia 原生控件皮肤化解决。

| TDesign Mobile | 推荐 Luna 控件 | 关键工作 |
| --- | --- | --- |
| `swipe-cell` | `LunaSwipeCell` | 左右滑手势、阈值、打开态、操作区宽度、互斥关闭 |
| `pull-down-refresh` | `LunaPullDownRefresh` | 下拉距离、状态文案、刷新完成/超时、滚动到底部事件 |
| `picker` | `LunaPicker` | 底部弹层、滚轮选择、确认取消、级联数据适配 |
| `cascader` | `LunaCascader` | 多级选择、路径展示、异步节点加载预留 |
| `tree-select` | `LunaTreeSelect` | 左右分栏、多级选择、选中态同步 |
| `indexes` | `LunaIndexes` / `LunaIndexesAnchor` | 右侧索引条、触控定位、吸顶 anchor、浮层提示 |
| `swiper` | `LunaSwiper` | 自动播放、循环、指示器、触控切换、露出边距 |
| `image-viewer` | `LunaImageViewer` | 预览弹层、缩放、拖拽、手势关闭、图片加载状态 |
| `upload` | `LunaUpload` | 文件/相册/拍照平台服务、状态列表、失败重试 |
| `qrcode` | `LunaQrCode` | 编码依赖、自绘或位图生成、颜色和 Logo 配置 |
| `watermark` | `LunaWatermark` | 装饰层或自绘、重复平铺、旋转和透明度 |
| `skeleton` | `LunaSkeleton` | 骨架占位、行列配置、shimmer 动效 |
| `guide` | `LunaGuide` | 遮罩、高亮目标、步骤状态、定位和滚动跟随 |
| `dropdown-menu` | `LunaDropdownMenu` | 顶部筛选菜单、面板展开、选项状态和遮罩 |
| `rate` | `LunaRate` | 星级绘制、半选、触控拖动、只读状态 |
| `count-down` | `LunaCountDown` | 计时器、格式化、完成事件、生命周期释放 |

## 初步建设顺序

1. 建立 TDesign token 体系：颜色、字体、圆角、间距、阴影、尺寸、状态色。
2. 先完成可皮肤化的 Base/Form 控件：`Button`、`TextBox`、`CheckBox`、`RadioButton`、`ToggleSwitch`、`Slider`、`NumericUpDown`。
3. 再做轻量包装控件：`Badge`、`Tag`、`Cell`、`Empty`、`Grid`。
4. 然后做反馈和导航：`Popup`、`Toast`、`Dialog`、`ActionSheet`、`NavBar`、`TabBar`。
5. 最后进入复杂移动交互控件：`SwipeCell`、`PullDownRefresh`、`Picker`、`Swiper`、`Indexes`。

## 风险与注意事项

- TDesign 是 Web/小程序组件语义，不能直接照搬 DOM API；Luna API 应使用 Avalonia 属性、命令、事件和模板部件表达。
- 移动端触控控件要优先处理安全区、虚拟键盘、屏幕密度和手势冲突。
- 桌面端和移动端视觉可以共享 token，但交互不应强行统一。
- 如果 Avalonia 原生控件已覆盖交互，不要重写控件逻辑；否则后续维护成本会过高。
