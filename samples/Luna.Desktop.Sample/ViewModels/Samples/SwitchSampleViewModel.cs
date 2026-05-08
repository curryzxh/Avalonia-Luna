using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SwitchSampleViewModel()
    : SampleDetailViewModelBase("输入", "ToggleSwitch / Switch", "复刻 TDesign Switch 的大小、禁用、加载占位和开关状态。")
{
    [ObservableProperty]
    private bool realtimePreview = true;
}
