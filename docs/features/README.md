# 功能目录说明

`features/` 用于存放单次需求、改造或专题工作的独立文档目录。  
目录索引见：[index.md](index.md)

每个功能或改造在 `features/` 下拥有独立目录，建议结构如下：

```text
features/
└── 20260508-mobile-picker-rework/
    ├── README.md
    ├── spec.md
    ├── plan.md
    ├── tasks.md
    ├── acceptance.md
    └── notes.md
```

## 约定

- 一个目录只承载一个明确交付目标。
- 如果是跨 `Desktop`、`Mobile`、`Samples`、`Docs` 的同一需求，仍然放在同一个功能目录中。
- 若需求中途拆分为独立交付，应新开功能目录，不要无限堆叠。
- 新功能目录命名建议使用 `YYYYMMDD-主题短名`。
- 至少补齐 `README.md`、`spec.md`、`plan.md` 三个入口文档。
- 新建或调整功能目录后，同步更新 [index.md](index.md)。

## 索引说明

- `index.md` 负责汇总当前功能目录和主要文档入口。
- 标准目录进入“功能目录”表格。
- 若后续出现历史遗留单文件方案，可单独放在“历史遗留目录”区说明。
