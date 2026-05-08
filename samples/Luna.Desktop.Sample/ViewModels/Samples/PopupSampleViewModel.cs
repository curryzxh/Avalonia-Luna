using Avalonia.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class PopupSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Popup", "弹出层，支持 Click / Hover / Focus / Manual 四种触发方式。")
{
    [ObservableProperty]
    private bool isClickOpen;

    [ObservableProperty]
    private bool isHoverOpen;

    [ObservableProperty]
    private bool isManualOpen;

    [ObservableProperty]
    private PlacementMode selectedPlacement = PlacementMode.Bottom;

    public PlacementMode[] Placements =>
    [
        PlacementMode.Bottom,
        PlacementMode.Top,
        PlacementMode.Left,
        PlacementMode.Right,
        PlacementMode.Center,
    ];

    [RelayCommand]
    private void ToggleManual() => IsManualOpen = !IsManualOpen;
}
