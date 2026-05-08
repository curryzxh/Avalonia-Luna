using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SliderSampleViewModel()
    : SampleDetailViewModelBase("输入", "Slider", "复刻 TDesign Slider 的单值、步进、禁用和百分比展示。")
{
    [ObservableProperty]
    private double progress = 42;

    [ObservableProperty]
    private double threshold = 70;
}
