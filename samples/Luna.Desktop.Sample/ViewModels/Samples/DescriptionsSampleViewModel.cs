using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public sealed partial class DescriptionsSampleViewModel()
    : SampleDetailViewModelBase("布局", "Descriptions", "描述列表控件，以标签-值对形式展示结构化数据，支持多列布局和标题。")
{
    [ObservableProperty]
    private ObservableCollection<DescriptionItem> _items =
    [
        new() { Label = "用户名", Value = "张三" },
        new() { Label = "邮箱", Value = "zhangsan@example.com" },
        new() { Label = "手机号", Value = "138-0000-0000" },
        new() { Label = "地址", Value = "北京市朝阳区望京街道" },
        new() { Label = "部门", Value = "前端开发组" },
        new() { Label = "职级", Value = "T6" },
        new() { Label = "入职日期", Value = "2023-01-15" },
        new() { Label = "状态", Value = "在职" },
    ];

    [ObservableProperty]
    private int _columnCount = 2;

    [ObservableProperty]
    private double _labelWidth = 100;

    [ObservableProperty]
    private string _title = "用户信息";
}
