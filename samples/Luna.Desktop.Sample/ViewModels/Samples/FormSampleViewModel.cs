using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class FormSampleViewModel()
    : SampleDetailViewModelBase("输入", "Form", "表单容器，统一管理标签、校验和布局。")
{
    [ObservableProperty]
    private string name = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string phone = string.Empty;

    [ObservableProperty]
    private string remark = string.Empty;
}
