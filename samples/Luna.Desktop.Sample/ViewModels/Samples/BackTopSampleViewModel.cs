using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class BackTopSampleViewModel : SampleDetailViewModelBase
{
    public BackTopSampleViewModel() : base("数据展示", "BackTop", "回到顶部按钮，当页面滚动超过一定距离后显示。")
    {
        for (int i = 1; i <= 30; i++)
            ScrollContentLines.Add($"这是第 {i} 行内容，用于演示滚动效果。");
    }

    public ObservableCollection<string> ScrollContentLines { get; } = [];

    [ObservableProperty]
    private double _visibilityHeight = 200;
}
