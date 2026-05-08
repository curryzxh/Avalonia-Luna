using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class LoadingSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Loading", "加载状态指示器，支持 Spinner 和文字提示。")
{
    [ObservableProperty]
    private bool isLoading = true;

    [ObservableProperty]
    private string loadingText = "加载中...";

    [ObservableProperty]
    private double loadingSize = 24;

    [ObservableProperty]
    private bool wrapLoading = true;
}
