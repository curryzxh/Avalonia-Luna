using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class RadioSampleViewModel()
    : SampleDetailViewModelBase("输入", "RadioButton", "复刻 TDesign Radio 的互斥选择、禁用项和按钮式分组语义。")
{
    [ObservableProperty]
    private string density = "Medium";
}
