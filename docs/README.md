# Luna Docs

本目录统一保存 Luna 控件库的规划、设计分析和实施拆分文档。

## 文档索引

- [TDesign Mobile 组件对照分析](tdesign-mobile-component-analysis.md)：对照 TDesign Mobile 与 Avalonia 12，判断哪些组件应优先皮肤化，哪些需要 Luna 自定义控件。
- [Luna 控件库阶段规划](luna-control-roadmap.md)：按阶段拆分 token、基础控件、数据展示、反馈导航和高交互移动控件建设顺序。
- [Luna 阶段 0：移动优先主题基础设施规划](luna-phase-0-mobile-theme-plan.md)：细化移动优先 token、主题资源结构和示例验证要求。
- [Luna 阶段 1：移动 MVP Base/Form 原生控件皮肤化规划](luna-phase-1-mobile-base-form-plan.md)：细化 Button、TextBox、ToggleSwitch、CheckBox、RadioButton 的移动优先皮肤化路径。

## 维护约定

- 新增规划文档统一放在 `docs/`，避免散落到仓库根目录。
- 分析类文档优先记录决策依据、组件映射和风险，不写成教程。
- 任务拆分类文档需要写清阶段目标、验收标准和推荐验证命令。
- 如果后续实现与文档判断不一致，应同步更新对应文档。
