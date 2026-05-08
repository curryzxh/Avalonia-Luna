using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class PopconfirmSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Popconfirm", "确认弹层，气泡确认框，用于二次确认操作。")
{
    [ObservableProperty]
    private int confirmCount;

    [ObservableProperty]
    private int cancelCount;

    [RelayCommand]
    private void OnConfirmed() => ConfirmCount++;

    [RelayCommand]
    private void OnCanceled() => CancelCount++;
}
