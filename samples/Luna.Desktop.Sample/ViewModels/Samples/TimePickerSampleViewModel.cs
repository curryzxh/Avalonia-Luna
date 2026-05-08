using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class TimePickerSampleViewModel()
    : SampleDetailViewModelBase("输入", "TimePicker", "时间选择器，支持时、分、秒选择和格式化。")
{
    [ObservableProperty]
    private TimeSpan? selectedTime = DateTime.Now.TimeOfDay;
}
