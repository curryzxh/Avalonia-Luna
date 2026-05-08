using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class NotificationSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Notification", "复刻 TDesign Notification 的 info、success、warning、error 和关闭操作。")
{
    [ObservableProperty]
    private bool showInlineNotification = true;

    [RelayCommand]
    private void ToggleInlineNotification()
    {
        ShowInlineNotification = !ShowInlineNotification;
    }
}
