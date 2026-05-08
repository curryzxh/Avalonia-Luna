using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public sealed partial class DialogSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Dialog", "对话框控件，支持模态、自定义按钮、ESC 关闭和加载状态。")
{
    [ObservableProperty]
    private string _dialogTitle = "基本对话框";

    [ObservableProperty]
    private string _dialogContent = "对话框内容区域，可放置任意控件。";

    [ObservableProperty]
    private bool _showCloseButton = true;

    [ObservableProperty]
    private bool _showCancelButton = true;

    [ObservableProperty]
    private bool _closeOnOverlayClick = true;

    [ObservableProperty]
    private bool _closeOnEsc = true;

    [ObservableProperty]
    private string _resultText = string.Empty;

    [RelayCommand]
    private void ShowBasic() => LunaDialog.Show(new DialogOptions
    {
        Title = "基本对话框",
        Content = "这是一个基本对话框示例，点击确认或取消按钮关闭。按 ESC 键也可以关闭。",
    });

    [RelayCommand]
    private void ShowSmall() => LunaDialog.Show(new DialogOptions
    {
        Title = "小型对话框",
        Content = "这是一个小型对话框，宽度 360px。",
        Width = 360,
    });

    [RelayCommand]
    private void ShowLarge() => LunaDialog.Show(new DialogOptions
    {
        Title = "大型对话框",
        Content = "这是一个大型对话框，宽度 600px，适合展示较多内容或复杂表单。",
        Width = 600,
    });

    [RelayCommand]
    private void ShowConfirm() => LunaDialog.Show(new DialogOptions
    {
        Title = "确认操作",
        Content = "确定要删除这条记录吗？此操作不可撤销。",
        ConfirmText = "删除",
        CancelText = "取消",
        CloseOnOverlayClick = false,
    });

    [RelayCommand]
    private void ShowCustom()
    {
        if (DialogHost.Current is null) return;
        DialogHost.Current.Result += OnDialogResult;
        DialogHost.Current.Show(new DialogOptions
        {
            Title = DialogTitle,
            Content = DialogContent,
            ShowCloseButton = ShowCloseButton,
            ShowCancelButton = ShowCancelButton,
            CloseOnOverlayClick = CloseOnOverlayClick,
            CloseOnEsc = CloseOnEsc,
        });
    }

    private void OnDialogResult(object? sender, DialogResult result)
    {
        if (sender is DialogHost host)
        {
            host.Result -= OnDialogResult;
            ResultText = $"对话框结果：{result}";
        }
    }
}
