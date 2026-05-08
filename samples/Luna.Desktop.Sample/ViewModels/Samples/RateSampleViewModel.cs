using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class RateSampleViewModel()
    : SampleDetailViewModelBase("输入", "Rate", "评分组件，支持半星、自定义图标和清除。")
{
    [ObservableProperty]
    private int basicRate;

    [ObservableProperty]
    private int presetRate = 3;

    [ObservableProperty]
    private int customRate;
}
