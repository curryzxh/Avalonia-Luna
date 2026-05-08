using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public sealed partial class DesktopBadgeSampleViewModel()
    : SampleDetailViewModelBase("基础", "DesktopBadge", "对齐 TDesign Badge 的 count、dot、maxCount、offset、shape。")
{
    [ObservableProperty]
    private int _count = 8;

    [ObservableProperty]
    private int _maxCount = 99;

    [ObservableProperty]
    private bool _showDot;

    [ObservableProperty]
    private double _offsetX;

    [ObservableProperty]
    private double _offsetY;
}
