using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class BreadcrumbSampleViewModel()
    : SampleDetailViewModelBase("导航", "Breadcrumb", "面包屑导航，展示页面层级路径。")
{
    [ObservableProperty]
    private string selectedPath = "首页";
}
