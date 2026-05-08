using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class AlertSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Alert", "警告提醒，支持 info/success/warning/error 四种类型，可关闭。")
{
    [ObservableProperty]
    private bool showInfoAlert = true;

    [ObservableProperty]
    private bool showSuccessAlert = true;

    [ObservableProperty]
    private bool showWarningAlert = true;

    [ObservableProperty]
    private bool showErrorAlert = true;

    [RelayCommand]
    private void ResetAlerts()
    {
        ShowInfoAlert = true;
        ShowSuccessAlert = true;
        ShowWarningAlert = true;
        ShowErrorAlert = true;
    }
}
