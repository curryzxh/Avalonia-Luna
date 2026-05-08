using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class PopupSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Popup", "弹出层，基于 Avalonia Popup 的皮肤化封装。")
{
    [ObservableProperty]
    private bool isPopupOpen;

    [ObservableProperty]
    private bool isTopOpen;

    [ObservableProperty]
    private bool isBottomOpen;

    [ObservableProperty]
    private bool isLeftOpen;

    [ObservableProperty]
    private bool isRightOpen;

    [ObservableProperty]
    private bool isCenterOpen;

    [ObservableProperty]
    private PlacementMode selectedPlacement = PlacementMode.Bottom;

    public PlacementMode[] Placements { get; } =
    {
        PlacementMode.Bottom,
        PlacementMode.Top,
        PlacementMode.Left,
        PlacementMode.Right,
        PlacementMode.Center,
    };

    [RelayCommand]
    private void TogglePopup() => IsPopupOpen = !IsPopupOpen;

    [RelayCommand]
    private void ToggleTop() => IsTopOpen = !IsTopOpen;

    [RelayCommand]
    private void ToggleBottom() => IsBottomOpen = !IsBottomOpen;

    [RelayCommand]
    private void ToggleLeft() => IsLeftOpen = !IsLeftOpen;

    [RelayCommand]
    private void ToggleRight() => IsRightOpen = !IsRightOpen;

    [RelayCommand]
    private void ToggleCenter() => IsCenterOpen = !IsCenterOpen;
}
