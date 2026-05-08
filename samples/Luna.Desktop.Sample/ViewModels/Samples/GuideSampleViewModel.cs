using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class GuideSampleViewModel : SampleDetailViewModelBase
{
    public GuideSampleViewModel() : base("基础", "Guide", "新手引导，通过遮罩高亮逐步引导用户了解功能。")
    {
    }

    [ObservableProperty]
    private bool _isGuideVisible;

    [ObservableProperty]
    private string _guideTitle = "欢迎使用 Luna";

    [ObservableProperty]
    private string _guideDescription = "Luna 是一套基于 Avalonia 的桌面端组件库，提供丰富的控件和主题支持。";

    [ObservableProperty]
    private int _currentStep;

    [ObservableProperty]
    private int _totalSteps = 3;

    [RelayCommand]
    private void StartGuide()
    {
        CurrentStep = 1;
        IsGuideVisible = true;
    }

    [RelayCommand]
    private void NextStep()
    {
        if (CurrentStep < TotalSteps)
            CurrentStep++;
        else
            IsGuideVisible = false;
    }

    [RelayCommand]
    private void CloseGuide()
    {
        IsGuideVisible = false;
    }
}
