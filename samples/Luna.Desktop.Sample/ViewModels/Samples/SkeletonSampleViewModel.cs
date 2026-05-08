using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class SkeletonSampleViewModel()
    : SampleDetailViewModelBase("数据展示", "Skeleton", "骨架屏占位，在内容加载前展示页面结构，支持文本、头像和段落预设。")
{
    [ObservableProperty]
    private bool isLoading = true;
}
