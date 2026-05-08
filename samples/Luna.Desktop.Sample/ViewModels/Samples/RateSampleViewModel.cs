using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class RateSampleViewModel()
    : SampleDetailViewModelBase("输入", "Rate", "评分组件，支持自定义图标、清除和只读模式。")
{
    [ObservableProperty]
    private int basicRate;

    [ObservableProperty]
    private int presetRate = 3;

    [ObservableProperty]
    private int customRate;

    [ObservableProperty]
    private int customCount = 10;

    [ObservableProperty]
    private int readOnlyRate = 4;
}
