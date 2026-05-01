using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class OverlayDemoView : UserControl
{
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

    private void OnBasicOverlayClicked(object? sender, EventArgs e)
    {
        ViewModel.HandleBasicOverlayClicked();
    }

    private void OnBasicOverlayOpening(object? sender, EventArgs e)
    {
        ViewModel.HandleBasicOverlayOpening();
    }

    private void OnBasicOverlayOpened(object? sender, EventArgs e)
    {
        ViewModel.HandleBasicOverlayOpened();
    }

    private void OnBasicOverlayClosing(object? sender, EventArgs e)
    {
        ViewModel.HandleBasicOverlayClosing();
    }

    private void OnBasicOverlayClosed(object? sender, EventArgs e)
    {
        ViewModel.HandleBasicOverlayClosed();
    }

    private void OnContentOverlayClicked(object? sender, EventArgs e)
    {
        ViewModel.HandleContentOverlayClicked();
    }

    private void OnTintOverlayClicked(object? sender, EventArgs e)
    {
        ViewModel.HandleTintOverlayClicked();
    }
}
