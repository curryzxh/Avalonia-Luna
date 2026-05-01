using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using System;
using Toast = Luna.Mobile.Controls.Toast;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class DateTimePickerDemoView : UserControl
{
        private DateTimePickerDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public DateTimePickerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        DateTimePickerHost.Confirmed += OnDateTimePickerConfirmed;
    }

    private void OnDatePickerClick(object? sender, RoutedEventArgs e)
    {
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "选择日期",
            Mode = DateTimePickerMode.Date,
            Value = DateTime.Today,
            Format = "YYYY-MM-DD"
        });
    }

    private void OnMinutePickerClick(object? sender, RoutedEventArgs e)
    {
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "选择日期时间",
            Mode = DateTimePickerMode.Minute,
            Value = DateTime.Now,
            Format = "YYYY-MM-DD HH:mm"
        });
    }

    private void OnRangePickerClick(object? sender, RoutedEventArgs e)
    {
        var end = DateTime.Today.AddDays(7).AddHours(23).AddMinutes(59);
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "最近 7 天",
            Mode = DateTimePickerMode.Date,
            Start = DateTime.Today,
            End = end,
            Value = DateTime.Today.AddDays(2),
            Format = "YYYY-MM-DD"
        });
    }

    private void OnStepPickerClick(object? sender, RoutedEventArgs e)
    {
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "会议开始时间",
            Mode = DateTimePickerMode.Minute,
            Value = DateTime.Today.AddHours(9).AddMinutes(30),
            Format = "YYYY-MM-DD HH:mm",
            Steps = new DateTimePickerStepOptions
            {
                Minute = 5
            }
        });
    }

    private void OnWeekPickerClick(object? sender, RoutedEventArgs e)
    {
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "带星期的日期",
            Mode = DateTimePickerMode.Date,
            Value = DateTime.Today,
            ShowWeek = true,
            Format = "YYYY-MM-DD"
        });
    }

    private void OnFormatPickerClick(object? sender, RoutedEventArgs e)
    {
        DateTimePickerHost.Show(new DateTimePickerOptions
        {
            Title = "自定义格式",
            Mode = DateTimePickerMode.Second,
            Value = DateTime.Now,
            SheetHeight = 360,
            Format = "YYYY年MM月DD日 HH:mm:ss"
        });
    }

    private void OnDateTimePickerConfirmed(object? sender, DateTimePickerConfirmedEventArgs e)
    {
        Toast.Show(e.FormattedValue);
    }
}
