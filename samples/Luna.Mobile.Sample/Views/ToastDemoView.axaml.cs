using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using Toast = Luna.Mobile.Controls.Toast;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class ToastDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public ToastDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnOnlyTextClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show("轻提示文字内容");
    }

    private void OnMultiLineClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "最多一行展示十个汉字宽度限制最多不超过三行文字行文字行文字",
        });
    }

    private void OnIconRowClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Row,
        });
    }

    private void OnIconColumnClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Column,
        });
    }

    private void OnLoadingClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Loading,
        });
    }

    private void OnSuccessClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Column,
        });
    }

    private void OnWarningClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Warning,
            Direction = ToastDirection.Column,
        });
    }

    private void OnErrorClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Error,
            Direction = ToastDirection.Column,
        });
    }

    private void OnOverlayClick(object? sender, RoutedEventArgs e)
    {
        Toast.Show(new ToastOptions
        {
            Message = "禁止滑动和点击",
            Direction = ToastDirection.Column,
            Placement = ToastPlacement.Bottom,
            Duration = TimeSpan.FromSeconds(5),
            ShowOverlay = true,
        });
    }
}
