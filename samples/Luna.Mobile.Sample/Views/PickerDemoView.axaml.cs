using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using System;
using System.Collections.Generic;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.Views;

public partial class PickerDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public PickerDemoView()
    {
        InitializeComponent();
        PickerHost.Confirmed += OnPickerConfirmed;
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnCityPickerClick(object? sender, RoutedEventArgs e)
    {
        PickerHost.Show(new PickerOptions
        {
            Columns =
            [
                new PickerColumn
                {
                    Items = new[]
                    {
                        "北京市",
                        "上海市",
                        "广州市",
                        "深圳市",
                        "杭州市",
                        "成都市",
                        "长沙市",
                    },
                    SelectedIndex = 0
                },
            ],
        });
    }

    private void OnSeasonPickerClick(object? sender, RoutedEventArgs e)
    {
        var currentYear = DateTime.Now.Year;
        var years = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            years.Add((currentYear - i).ToString());
        }

        PickerHost.Show(new PickerOptions
        {
            Columns =
            [
                new PickerColumn { Items = years, SelectedIndex = 0 },
                new PickerColumn { Items = new[] { "春", "夏", "秋", "冬" }, SelectedIndex = 0 },
            ],
        });
    }

    private void OnTitlePickerClick(object? sender, RoutedEventArgs e)
    {
        PickerHost.Show(new PickerOptions
        {
            Title = "选择城市",
            Columns =
            [
                new PickerColumn { Items = new[] { "北京市", "上海市", "广州市", "深圳市" }, SelectedIndex = 0 },
            ],
        });
    }

    private void OnCustomHeightPickerClick(object? sender, RoutedEventArgs e)
    {
        PickerHost.Show(new PickerOptions
        {
            Title = "自定义高度",
            SheetHeight = 420,
            Columns =
            [
                new PickerColumn { Items = new[] { "北京市", "上海市", "广州市", "深圳市" }, SelectedIndex = 0 },
                new PickerColumn { Items = new[] { "A", "B", "C", "D", "E" }, SelectedIndex = 2 },
            ],
        });
    }

    private void OnPickerConfirmed(object? sender, PickerConfirmedEventArgs e)
    {
        var values = string.Join(" ", e.Values);
        Toast.Show(values);
    }
}

