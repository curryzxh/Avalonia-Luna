using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class AffixSampleViewModel : SampleDetailViewModelBase
{
    public AffixSampleViewModel() : base("导航", "Affix", "固钉，将元素固定在页面可视区域。")
    {
    }

    [ObservableProperty]
    private double _offset;
}
