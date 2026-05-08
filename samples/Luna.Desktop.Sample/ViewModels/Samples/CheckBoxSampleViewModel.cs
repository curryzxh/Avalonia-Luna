using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class CheckBoxSampleViewModel()
    : SampleDetailViewModelBase("输入", "CheckBox", "复刻 TDesign Checkbox 的未选、半选、选中和禁用状态。")
{
    [ObservableProperty]
    private bool allowNotification = true;

    [ObservableProperty]
    private bool allowAnalytics;

    [ObservableProperty]
    private bool? auditState;
}
