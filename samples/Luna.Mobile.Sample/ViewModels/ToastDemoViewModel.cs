using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using Toast = Luna.Mobile.Controls.Toast;
using System;

namespace Luna.Mobile.Sample.ViewModels;

public partial class ToastDemoViewModel : DemoViewModelBase
{
    [RelayCommand]
    private void OnlyText()
    {
        Toast.Show("轻提示文字内容");
    }

    [RelayCommand]
    private void MultiLine()
    {
        Toast.Show(new ToastOptions
        {
            Message = "最多一行展示十个汉字宽度限制最多不超过三行文字行文字行文字",
        });
    }

    [RelayCommand]
    private void IconRow()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Row,
        });
    }

    [RelayCommand]
    private void IconColumn()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Column,
        });
    }

    [RelayCommand]
    private void Loading()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Loading,
        });
    }

    [RelayCommand]
    private void Success()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Success,
            Direction = ToastDirection.Column,
        });
    }

    [RelayCommand]
    private void Warning()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Warning,
            Direction = ToastDirection.Column,
        });
    }

    [RelayCommand]
    private void Error()
    {
        Toast.Show(new ToastOptions
        {
            Message = "轻提示文字内容",
            Theme = ToastTheme.Error,
            Direction = ToastDirection.Column,
        });
    }

    [RelayCommand]
    private void Overlay()
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
