using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class AutoCompleteSampleViewModel()
    : SampleDetailViewModelBase("输入", "AutoComplete", "自动完成输入框，基于用户输入过滤候选列表。")
{
    [ObservableProperty]
    private string? searchText;

    public ObservableCollection<string> Suggestions { get; } =
        ["Apple", "Banana", "Cherry", "Durian", "Elderberry", "Fig", "Grape"];

    public ObservableCollection<string> FilteredItems { get; } = [];
}
