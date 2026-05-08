using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class DropdownSampleViewModel()
    : SampleDetailViewModelBase("导航", "Dropdown", "下拉菜单，基于 Avalonia ContextMenu/Flyout 机制，展示右键和按钮触发。")
{
    [ObservableProperty]
    private string lastAction = "无操作";
}
