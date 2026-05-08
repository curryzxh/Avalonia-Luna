using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class DrawerSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Drawer", "抽屉面板，支持上下左右四个方向弹出。")
{
    [ObservableProperty]
    private string drawerTitle = "抽屉标题";

    [ObservableProperty]
    private string drawerContent = "这是抽屉面板的内容区域，可以放置表单、详情等复杂内容。";

    [RelayCommand]
    private void ShowRightDrawer() => LunaDrawer.Show(new DrawerOptions
    {
        Title = DrawerTitle,
        Content = DrawerContent,
        Placement = DrawerPlacement.Right,
        Size = 400,
    });

    [RelayCommand]
    private void ShowLeftDrawer() => LunaDrawer.Show(new DrawerOptions
    {
        Title = DrawerTitle,
        Content = DrawerContent,
        Placement = DrawerPlacement.Left,
        Size = 350,
    });

    [RelayCommand]
    private void ShowTopDrawer() => LunaDrawer.Show(new DrawerOptions
    {
        Title = DrawerTitle,
        Content = DrawerContent,
        Placement = DrawerPlacement.Top,
        Size = 300,
    });

    [RelayCommand]
    private void ShowBottomDrawer() => LunaDrawer.Show(new DrawerOptions
    {
        Title = DrawerTitle,
        Content = DrawerContent,
        Placement = DrawerPlacement.Bottom,
        Size = 300,
    });
}
