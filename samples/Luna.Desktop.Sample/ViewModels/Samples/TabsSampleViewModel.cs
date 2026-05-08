using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TabsSampleViewModel()
    : SampleDetailViewModelBase("导航", "Tabs", "标签页导航，基于 Avalonia TabControl 皮肤化，支持默认和卡片样式。")
{
    [ObservableProperty]
    private int selectedTab;
}
