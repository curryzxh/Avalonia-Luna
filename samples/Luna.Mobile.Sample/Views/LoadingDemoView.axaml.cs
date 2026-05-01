using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class LoadingDemoView : UserControl
{
    private DispatcherTimer? _refreshTimer;

        private LoadingDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public LoadingDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        UpdateSpeed(SpeedSlider.Value);
    }

    private void OnRefreshClick(object? sender, RoutedEventArgs e)
    {
        InlineLoading.IsLoading = true;

        _refreshTimer?.Stop();
        _refreshTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromSeconds(1),
        };
        _refreshTimer.Tick += (_, _) =>
        {
            _refreshTimer?.Stop();
            InlineLoading.IsLoading = false;
        };
        _refreshTimer.Start();
    }

    private void OnSpeedChanged(object? sender, RangeBaseValueChangedEventArgs e)
    {
        UpdateSpeed(e.NewValue);
    }

    private void UpdateSpeed(double speed)
    {
        if (speed <= 0)
        {
            SpeedLoading.Duration = TimeSpan.FromMilliseconds(3000);
            return;
        }

        SpeedLoading.Duration = TimeSpan.FromMilliseconds((1.0 / speed) * 3000);
    }
}

