using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Mobile.Sample.ViewModels;

public partial class OverlayDemoViewModel : DemoViewModelBase
{
    [ObservableProperty]
    private bool _isBasicOverlayVisible;

    [ObservableProperty]
    private bool _isContentOverlayVisible;

    [ObservableProperty]
    private bool _isTintOverlayVisible;

    [ObservableProperty]
    private bool _isPassThroughOverlayVisible;

    [ObservableProperty]
    private string _lifecycleText = "最近事件：未触发";

    [ObservableProperty]
    private int _passThroughCount;

    public string PassThroughCountText => $"当前点击次数：{PassThroughCount}";

    partial void OnPassThroughCountChanged(int value)
    {
        OnPropertyChanged(nameof(PassThroughCountText));
    }

    [RelayCommand]
    private void ShowBasicOverlay()
    {
        IsBasicOverlayVisible = true;
    }

    [RelayCommand]
    private void ShowContentOverlay()
    {
        IsContentOverlayVisible = true;
    }

    [RelayCommand]
    private void CloseContentOverlay()
    {
        IsContentOverlayVisible = false;
    }

    [RelayCommand]
    private void ShowTintOverlay()
    {
        IsTintOverlayVisible = true;
    }

    [RelayCommand]
    private void CloseTintOverlay()
    {
        IsTintOverlayVisible = false;
    }

    [RelayCommand]
    private void TogglePassThroughOverlay()
    {
        IsPassThroughOverlayVisible = !IsPassThroughOverlayVisible;
    }

    [RelayCommand]
    private void IncrementPassThroughCount()
    {
        PassThroughCount++;
    }

    public void HandleBasicOverlayClicked()
    {
        IsBasicOverlayVisible = false;
    }

    public void HandleBasicOverlayOpening()
    {
        LifecycleText = "最近事件：onOpen";
    }

    public void HandleBasicOverlayOpened()
    {
        LifecycleText = "最近事件：onOpened";
    }

    public void HandleBasicOverlayClosing()
    {
        LifecycleText = "最近事件：onClose";
    }

    public void HandleBasicOverlayClosed()
    {
        LifecycleText = "最近事件：onClosed";
    }

    public void HandleContentOverlayClicked()
    {
        IsContentOverlayVisible = false;
    }

    public void HandleTintOverlayClicked()
    {
        IsTintOverlayVisible = false;
    }
}
