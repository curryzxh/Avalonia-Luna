using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class DatePickerSampleViewModel()
    : SampleDetailViewModelBase("输入", "DatePicker", "日期选择器，支持日期范围和格式化显示。")
{
    [ObservableProperty]
    private DateTimeOffset? selectedDate = DateTimeOffset.Now;
}
