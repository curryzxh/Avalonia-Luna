using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SelectSampleViewModel()
    : SampleDetailViewModelBase("输入", "Select", "下拉选择器，支持单选、分组、禁用和自定义选项。")
{
    [ObservableProperty]
    private string? selectedCity;

    [ObservableProperty]
    private string? selectedSize = "M";

    [ObservableProperty]
    private string? disabledSelected = "option1";

    public ObservableCollection<string> Cities { get; } = ["北京", "上海", "广州", "深圳", "杭州", "成都"];

    public ObservableCollection<string> Sizes { get; } = ["XS", "S", "M", "L", "XL", "XXL"];
}
