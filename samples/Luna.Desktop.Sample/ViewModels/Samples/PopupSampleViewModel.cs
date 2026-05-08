using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class PopupSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Popup", "弹出层，基于 Avalonia Popup 的皮肤化封装。")
{
    [ObservableProperty]
    private bool isPopupOpen;

    [RelayCommand]
    private void TogglePopup() => IsPopupOpen = !IsPopupOpen;
}
