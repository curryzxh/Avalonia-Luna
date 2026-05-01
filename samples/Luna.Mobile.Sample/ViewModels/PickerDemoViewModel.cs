using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using System.Collections.Generic;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.ViewModels;

public partial class PickerDemoViewModel : DemoViewModelBase
{
    public event Action<PickerOptions>? PickerRequested;

    [RelayCommand]
    private void CityPicker()
    {
        RequestPicker(new PickerOptions
        {
            Columns =
            [
                new PickerColumn
                {
                    Items = ["北京市", "上海市", "广州市", "深圳市", "杭州市", "成都市", "长沙市"],
                    SelectedIndex = 0,
                },
            ],
        });
    }

    [RelayCommand]
    private void SeasonPicker()
    {
        var currentYear = DateTime.Now.Year;
        var years = new List<string>();
        for (var i = 0; i < 10; i++)
        {
            years.Add((currentYear - i).ToString());
        }

        RequestPicker(new PickerOptions
        {
            Columns =
            [
                new PickerColumn { Items = years, SelectedIndex = 0 },
                new PickerColumn { Items = ["春", "夏", "秋", "冬"], SelectedIndex = 0 },
            ],
        });
    }

    [RelayCommand]
    private void TitlePicker()
    {
        RequestPicker(new PickerOptions
        {
            Title = "选择城市",
            Columns =
            [
                new PickerColumn { Items = ["北京市", "上海市", "广州市", "深圳市"], SelectedIndex = 0 },
            ],
        });
    }

    [RelayCommand]
    private void CustomHeightPicker()
    {
        RequestPicker(new PickerOptions
        {
            Title = "自定义高度",
            SheetHeight = 420,
            Columns =
            [
                new PickerColumn { Items = ["北京市", "上海市", "广州市", "深圳市"], SelectedIndex = 0 },
                new PickerColumn { Items = ["A", "B", "C", "D", "E"], SelectedIndex = 2 },
            ],
        });
    }

    public void OnPickerConfirmed(PickerConfirmedEventArgs e)
    {
        Toast.Show(string.Join(" ", e.Values));
    }

    private void RequestPicker(PickerOptions options)
    {
        PickerRequested?.Invoke(options);
    }
}
