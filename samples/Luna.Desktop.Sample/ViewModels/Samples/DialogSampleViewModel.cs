using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class DialogSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Dialog", "对话框，支持标题、内容、确认/取消按钮和遮罩关闭。")
{
    [ObservableProperty]
    private string dialogTitle = "基本对话框";

    [ObservableProperty]
    private string dialogContent = "这是一个对话框的内容区域，用于展示需要用户确认的信息。";

    [ObservableProperty]
    private bool closeOnOverlayClick;

    [RelayCommand]
    private void ShowDialog()
    {
        LunaDialog.Show(new DialogOptions
        {
            Title = DialogTitle,
            Content = DialogContent,
            ConfirmText = "确认",
            CancelText = "取消",
            CloseOnOverlayClick = CloseOnOverlayClick,
            ShowCloseButton = true,
            Width = 480,
        });
    }

    [RelayCommand]
    private void ShowSimpleDialog()
    {
        LunaDialog.Show(new DialogOptions
        {
            Title = "提示",
            Content = "操作成功完成。",
            ConfirmText = "知道了",
            CloseOnOverlayClick = true,
            Width = 360,
        });
    }
}
