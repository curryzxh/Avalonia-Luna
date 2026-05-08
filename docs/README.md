# Luna Docs

本目录统一保存 Luna 控件库的规划、分析、指南、控件使用文档和功能工作档案。

## 目录结构

- `controls/`：已实现控件的使用文档。
- `analysis/`：组件对照、能力缺口和设计判断。
- `planning/`：阶段规划、实施拆分和验收标准。
- `guides/`：偏流程和落地方法的操作指南。
- `features/`：单次需求、改造或专题工作的独立文档目录。

## 使用文档

- [Luna.Mobile 控件使用文档](controls/README.md)：面向控件使用者的组件手册，覆盖最小用法、常用属性、对应 sample 和当前限制。

## 分析与规划

- [TDesign Mobile 组件对照分析](analysis/tdesign-mobile-component-analysis.md)：对照 TDesign Mobile 与 Avalonia 12，判断哪些组件应优先皮肤化，哪些需要 Luna 自定义控件。
- [Luna 控件库阶段规划](planning/luna-control-roadmap.md)：按阶段拆分 token、基础控件、数据展示、反馈导航和高交互移动控件建设顺序。
- [Luna 阶段 0：移动优先主题基础设施规划](planning/luna-phase-0-mobile-theme-plan.md)：细化移动优先 token、主题资源结构和示例验证要求。
- [Luna 阶段 1：移动 MVP Base/Form 原生控件皮肤化规划](planning/luna-phase-1-mobile-base-form-plan.md)：细化 Button、TextBox、ToggleSwitch、CheckBox、RadioButton 的移动优先皮肤化路径。

## 指南与过程文档

- [TDesign Mobile 示例复刻指南](guides/tdesign-mobile-sample-authoring-guide.md)：沉淀从 TDesign Mobile React 官方示例源码复刻 Luna 移动示例页的流程、映射原则和占位策略。
- [Features 目录说明](features/README.md)：说明单次需求或改造文档如何在 `docs/features/` 中组织。
- [Features 索引](features/index.md)：汇总当前功能目录与专题工作入口。

## 维护约定

- 新增文档时优先判断所属类别，避免把分析、规划和指南继续平铺在 `docs/` 根目录。
- 分析类文档优先记录决策依据、组件映射和风险，不写成教程。
- 任务拆分类文档需要写清阶段目标、验收标准和推荐验证命令。
- 如果后续实现与文档判断不一致，应同步更新对应文档。
