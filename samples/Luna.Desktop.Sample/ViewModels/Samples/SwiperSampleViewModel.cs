using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SwiperSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Swiper", "轮播组件（占位示例，待完整实现）。")
{
    [ObservableProperty]
    private int currentIndex;
}
