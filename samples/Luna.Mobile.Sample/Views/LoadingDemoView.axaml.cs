using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using Avalonia.Threading;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class LoadingDemoView : UserControl
{
    private DispatcherTimer? _refreshTimer;

    public event EventHandler? BackRequested;

    public LoadingDemoView()
    {
        InitializeComponent();
        UpdateSpeed(SpeedSlider.Value);
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
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

