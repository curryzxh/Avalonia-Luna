using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SliderSampleViewModel()
    : SampleDetailViewModelBase("输入", "Slider", "复刻 TDesign Slider 的单值、步进、禁用、范围和刻度展示。")
{
    [ObservableProperty]
    private double progress = 42;

    [ObservableProperty]
    private double threshold = 70;

    [ObservableProperty]
    private double rangeStart = 20;

    [ObservableProperty]
    private double rangeEnd = 80;

    [ObservableProperty]
    private double stepValue = 30;

    [ObservableProperty]
    private double markedValue = 50;
}
