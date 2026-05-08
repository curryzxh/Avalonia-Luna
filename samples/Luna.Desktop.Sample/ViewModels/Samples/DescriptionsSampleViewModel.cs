using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public sealed partial class DescriptionsSampleViewModel()
    : SampleDetailViewModelBase("布局", "Descriptions", "描述列表控件，以标签-值对形式展示结构化数据，支持多列布局和标题。")
{
    [ObservableProperty]
    private ObservableCollection<DescriptionItem> _items =
    [
        new("用户名", "张三"),
        new("邮箱", "zhangsan@example.com"),
        new("手机号", "138-0000-0000"),
        new("地址", "北京市朝阳区望京街道"),
        new("部门", "前端开发组"),
        new("职级", "T6"),
        new("入职日期", "2023-01-15"),
        new("状态", "在职"),
    ];
}

public sealed class DescriptionItem(string label, string value)
{
    public string Label { get; } = label;
    public string Value { get; } = value;
}
