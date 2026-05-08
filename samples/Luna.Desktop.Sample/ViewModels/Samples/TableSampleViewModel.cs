using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TableSampleViewModel : SampleDetailViewModelBase
{
    public TableSampleViewModel() : base("数据展示", "Table", "表格控件，用于展示结构化数据。")
    {
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
