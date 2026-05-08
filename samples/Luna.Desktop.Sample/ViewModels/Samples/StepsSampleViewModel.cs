using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class StepsSampleViewModel()
    : SampleDetailViewModelBase("导航", "Steps", "步骤条，展示横向/纵向步骤和多种状态。")
{
    [ObservableProperty]
    private string currentStep = "1";

    [ObservableProperty]
    private bool isVertical;

    [ObservableProperty]
    private bool isDotTheme;
}
