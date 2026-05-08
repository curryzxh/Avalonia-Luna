using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class MessageSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Message", "全局消息提示，顶部轻量反馈，支持 info/success/warning/error。")
{
    [RelayCommand]
    private void ShowInfo() => LunaMessage.Info("这是一条信息提示消息。");

    [RelayCommand]
    private void ShowSuccess() => LunaMessage.Success("操作成功完成！");

    [RelayCommand]
    private void ShowWarning() => LunaMessage.Warning("请注意，此操作可能存在风险。");

    [RelayCommand]
    private void ShowError() => LunaMessage.Error("操作失败，请重试。");

    [RelayCommand]
    private void ShowClosable() => MessageHost.Current?.Show(new MessageOptions
    {
        Content = "这是一条可手动关闭的消息。",
        Theme = MessageTheme.Info,
        Duration = TimeSpan.Zero,
        ShowClose = true,
    });
}
