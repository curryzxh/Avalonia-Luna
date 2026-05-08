using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class WatermarkSampleViewModel : SampleDetailViewModelBase
{
    public WatermarkSampleViewModel() : base("数据展示", "Watermark", "水印，在内容上方叠加重复的半透明文字水印。")
    {
    }

    [ObservableProperty]
    private string _watermarkText = "Luna Desktop";

    [ObservableProperty]
    private double _watermarkOpacity = 0.12;

    [ObservableProperty]
    private double _watermarkRotate = -22;

    [ObservableProperty]
    private double _watermarkGap = 120;

    [ObservableProperty]
    private double _watermarkFontSize = 14;

    [ObservableProperty]
    private bool _isScrolling;
}
