using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class PaginationSampleViewModel()
    : SampleDetailViewModelBase("导航", "Pagination", "分页控件，展示页码导航、总数显示和翻页。")
{
    [ObservableProperty]
    private int currentPage = 1;

    [ObservableProperty]
    private int totalCount = 256;

    [ObservableProperty]
    private int pageSize = 10;

    [ObservableProperty]
    private bool showTotal = true;

    public string PageInfo => $"第 {CurrentPage} 页，每页 {PageSize} 条，共 {TotalCount} 条";

    [RelayCommand]
    private void OnPageChanged()
    {
        OnPropertyChanged(nameof(PageInfo));
    }
}
