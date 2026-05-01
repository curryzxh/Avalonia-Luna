using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.ViewModels;

public partial class DateTimePickerDemoViewModel : DemoViewModelBase
{
    public event Action<DateTimePickerOptions>? DateTimePickerRequested;

    [RelayCommand]
    private void DatePicker()
    {
        RequestPicker(new DateTimePickerOptions
        {
            Title = "选择日期",
            Mode = DateTimePickerMode.Date,
            Value = DateTime.Today,
            Format = "YYYY-MM-DD",
        });
    }

    [RelayCommand]
    private void MinutePicker()
    {
        RequestPicker(new DateTimePickerOptions
        {
            Title = "选择日期时间",
            Mode = DateTimePickerMode.Minute,
            Value = DateTime.Now,
            Format = "YYYY-MM-DD HH:mm",
        });
    }

    [RelayCommand]
    private void RangePicker()
    {
        var end = DateTime.Today.AddDays(7).AddHours(23).AddMinutes(59);
        RequestPicker(new DateTimePickerOptions
        {
            Title = "最近 7 天",
            Mode = DateTimePickerMode.Date,
            Start = DateTime.Today,
            End = end,
            Value = DateTime.Today.AddDays(2),
            Format = "YYYY-MM-DD",
        });
    }

    [RelayCommand]
    private void StepPicker()
    {
        RequestPicker(new DateTimePickerOptions
        {
            Title = "会议开始时间",
            Mode = DateTimePickerMode.Minute,
            Value = DateTime.Today.AddHours(9).AddMinutes(30),
            Format = "YYYY-MM-DD HH:mm",
            Steps = new DateTimePickerStepOptions
            {
                Minute = 5,
            },
        });
    }

    [RelayCommand]
    private void WeekPicker()
    {
        RequestPicker(new DateTimePickerOptions
        {
            Title = "带星期的日期",
            Mode = DateTimePickerMode.Date,
            Value = DateTime.Today,
            ShowWeek = true,
            Format = "YYYY-MM-DD",
        });
    }

    [RelayCommand]
    private void FormatPicker()
    {
        RequestPicker(new DateTimePickerOptions
        {
            Title = "自定义格式",
            Mode = DateTimePickerMode.Second,
            Value = DateTime.Now,
            SheetHeight = 360,
            Format = "YYYY年MM月DD日 HH:mm:ss",
        });
    }

    public void OnDateTimePickerConfirmed(DateTimePickerConfirmedEventArgs e)
    {
        Toast.Show(e.FormattedValue);
    }

    private void RequestPicker(DateTimePickerOptions options)
    {
        DateTimePickerRequested?.Invoke(options);
    }
}
