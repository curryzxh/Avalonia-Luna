using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class ProgressSampleViewModel()
    : SampleDetailViewModelBase("数据展示", "Progress", "复刻 TDesign Progress 的线性进度、状态进度和不确定进度。")
{
    [ObservableProperty]
    private double percentage = 64;

    [ObservableProperty]
    private double successPercentage = 100;

    [ObservableProperty]
    private double warningPercentage = 56;

    [ObservableProperty]
    private double errorPercentage = 88;
}
