using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class InputNumberSampleViewModel()
    : SampleDetailViewModelBase("输入", "InputNumber", "数字输入框，支持步进、最大最小值限制和小数精度。")
{
    [ObservableProperty]
    private double? quantity = 5;

    [ObservableProperty]
    private double? price = 29.99;

    [ObservableProperty]
    private double? disabledValue = 100;

    [ObservableProperty]
    private double? clampedValue = 50;

    [ObservableProperty]
    private double? decimalValue = 3.14;
}
