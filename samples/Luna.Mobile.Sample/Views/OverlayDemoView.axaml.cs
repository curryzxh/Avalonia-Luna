using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class OverlayDemoView : UserControl
{
    private int _passThroughCount;

        private OverlayDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public OverlayDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnShowBasicOverlayClick(object? sender, RoutedEventArgs e)
    {
        BasicOverlay.Visible = true;
    }

    private void OnBasicOverlayClicked(object? sender, EventArgs e)
    {
        BasicOverlay.Visible = false;
    }

    private void OnBasicOverlayOpening(object? sender, EventArgs e)
    {
        LifecycleText.Text = "最近事件：onOpen";
    }

    private void OnBasicOverlayOpened(object? sender, EventArgs e)
    {
        LifecycleText.Text = "最近事件：onOpened";
    }

    private void OnBasicOverlayClosing(object? sender, EventArgs e)
    {
        LifecycleText.Text = "最近事件：onClose";
    }

    private void OnBasicOverlayClosed(object? sender, EventArgs e)
    {
        LifecycleText.Text = "最近事件：onClosed";
    }

    private void OnShowContentOverlayClick(object? sender, RoutedEventArgs e)
    {
        ContentOverlay.Visible = true;
    }

    private void OnContentOverlayClicked(object? sender, EventArgs e)
    {
        ContentOverlay.Visible = false;
    }

    private void OnCloseContentOverlayClick(object? sender, RoutedEventArgs e)
    {
        ContentOverlay.Visible = false;
    }

    private void OnShowTintOverlayClick(object? sender, RoutedEventArgs e)
    {
        TintOverlay.Visible = true;
    }

    private void OnTintOverlayClicked(object? sender, EventArgs e)
    {
        TintOverlay.Visible = false;
    }

    private void OnCloseTintOverlayClick(object? sender, RoutedEventArgs e)
    {
        TintOverlay.Visible = false;
    }

    private void OnTogglePassThroughOverlayClick(object? sender, RoutedEventArgs e)
    {
        PassThroughOverlay.Visible = !PassThroughOverlay.Visible;
    }

    private void OnPassThroughTargetClick(object? sender, RoutedEventArgs e)
    {
        _passThroughCount++;
        PassThroughCountText.Text = $"当前点击次数：{_passThroughCount}";
    }
}
