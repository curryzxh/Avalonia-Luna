using System.Collections.ObjectModel;
using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TableSampleViewModel : SampleDetailViewModelBase
{
    public TableSampleViewModel() : base("数据展示", "Table", "表格控件，用于展示结构化数据，支持边框、斑马纹和悬停高亮。")
    {
        Columns =
        [
            new TableColumn { Header = "编号", BindingPath = "Id", Width = 100, MinWidth = 80 },
            new TableColumn { Header = "名称", BindingPath = "Name", Width = 180 },
            new TableColumn { Header = "分类", BindingPath = "Category", Width = 120 },
            new TableColumn { Header = "价格", BindingPath = "Price", Width = 100, Alignment = TextAlignment.Right },
            new TableColumn { Header = "状态", BindingPath = "Status", Width = 80 },
        ];
    }

    public ObservableCollection<TableRow> Rows { get; } =
    [
        new("PROD-001", "智能手表", "电子设备", 1299, "在售"),
        new("PROD-002", "运动鞋", "服饰鞋帽", 599, "在售"),
        new("PROD-003", "咖啡机", "家用电器", 2499, "缺货"),
        new("PROD-004", "机械键盘", "电子设备", 899, "在售"),
        new("PROD-005", "背包", "箱包", 349, "下架"),
        new("PROD-006", "蓝牙耳机", "电子设备", 499, "在售"),
        new("PROD-007", "瑜伽垫", "运动健身", 129, "在售"),
    ];

    public ObservableCollection<TableColumn> Columns { get; }

    [ObservableProperty]
    private bool _bordered = true;

    [ObservableProperty]
    private bool _striped;

    [ObservableProperty]
    private bool _hoverable = true;
}

public class TableRow(string id, string name, string category, double price, string status)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Category { get; } = category;
    public double Price { get; } = price;
    public string Status { get; } = status;
}
