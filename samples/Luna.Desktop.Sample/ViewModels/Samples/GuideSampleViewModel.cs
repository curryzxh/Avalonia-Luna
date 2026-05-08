using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class GuideSampleViewModel : SampleDetailViewModelBase
{
    private Guide? _guide;

    public GuideSampleViewModel() : base("导航", "Guide", "引导，用于分步引导用户了解功能。")
    {
    }

    [ObservableProperty]
    private ObservableCollection<GuideStep> _steps = new()
    {
        new() { Title = "欢迎", Description = "欢迎使用 Luna Desktop 控件库！接下来将引导您了解核心功能。" },
        new() { Title = "主题系统", Description = "Luna 使用 Token 化主题系统，支持明暗主题切换。所有颜色、字号、间距、圆角均可通过 Token 调整。" },
        new() { Title = "控件库", Description = "Luna 提供了 60+ 高质量控件，覆盖基础、表单、数据展示、反馈和导航五大类。" },
        new() { Title = "开始使用", Description = "通过 NuGet 安装 Luna.Desktop，在 App.axaml 中引用主题即可开始使用！" }
    };

    [ObservableProperty]
    private bool _isGuideActive;

    [ObservableProperty]
    private string _guideStatus = "引导未开始";

    [RelayCommand]
    private void StartGuide()
    {
        IsGuideActive = true;
        GuideStatus = "引导进行中...";
    }

    [RelayCommand]
    private void NextStep()
    {
    }

    [RelayCommand]
    private void FinishGuide()
    {
        IsGuideActive = false;
        GuideStatus = "引导已完成";
    }

    [RelayCommand]
    private void ResetGuide()
    {
        IsGuideActive = false;
        GuideStatus = "引导未开始";
    }
}
