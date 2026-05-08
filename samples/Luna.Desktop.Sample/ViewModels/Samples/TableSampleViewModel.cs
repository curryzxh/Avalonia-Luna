using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TableSampleViewModel()
    : SampleDetailViewModelBase("数据展示", "Table", "表格，基于 Avalonia DataGrid 皮肤化，用于展示结构化数据。")
{
    public ObservableCollection<TableRow> Rows { get; } =
    [
        new("PROD-001", "智能手表", "电子设备", 1299, "在售"),
        new("PROD-002", "运动鞋", "服饰鞋帽", 599, "在售"),
        new("PROD-003", "咖啡机", "家用电器", 2499, "缺货"),
        new("PROD-004", "机械键盘", "电子设备", 899, "在售"),
        new("PROD-005", "背包", "箱包", 349, "下架"),
    ];
}

public class TableRow(string id, string name, string category, double price, string status)
{
    public string Id { get; } = id;
    public string Name { get; } = name;
    public string Category { get; } = category;
    public double Price { get; } = price;
    public string Status { get; } = status;
}
