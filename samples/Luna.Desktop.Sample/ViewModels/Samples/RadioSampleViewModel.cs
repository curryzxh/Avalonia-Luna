using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class RadioSampleViewModel()
    : SampleDetailViewModelBase("输入", "RadioButton", "复刻 TDesign Radio 的互斥选择、禁用项和按钮式分组语义。")
{
    [ObservableProperty]
    private string density = "Medium";

    [ObservableProperty]
    private string color = "Red";

    [ObservableProperty]
    private string size = "M";

    public ObservableCollection<string> Colors { get; } = ["Red", "Green", "Blue", "Yellow"];

    public ObservableCollection<string> Sizes { get; } = ["XS", "S", "M", "L", "XL"];
}
