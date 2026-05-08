using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class MenuSampleViewModel()
    : SampleDetailViewModelBase("导航", "Menu", "导航菜单，基于 Avalonia Menu 皮肤化，展示顶部菜单和子菜单。")
{
    [ObservableProperty]
    private string lastAction = "无操作";
}
