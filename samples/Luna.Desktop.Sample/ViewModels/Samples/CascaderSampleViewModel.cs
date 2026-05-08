using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class CascaderSampleViewModel : SampleDetailViewModelBase
{
    public CascaderSampleViewModel() : base("输入", "Cascader", "级联选择器，支持多级数据联动选择。")
    {
    }

    [ObservableProperty]
    private ObservableCollection<CascaderItem> _items = new()
    {
        new("浙江省", new ObservableCollection<CascaderItem>
        {
            new("杭州市", new ObservableCollection<CascaderItem>
            {
                new("西湖区"), new("余杭区"), new("滨江区")
            }),
            new("宁波市", new ObservableCollection<CascaderItem>
            {
                new("海曙区"), new("江北区")
            })
        }),
        new("江苏省", new ObservableCollection<CascaderItem>
        {
            new("南京市", new ObservableCollection<CascaderItem>
            {
                new("玄武区"), new("鼓楼区")
            }),
            new("苏州市", new ObservableCollection<CascaderItem>
            {
                new("姑苏区"), new("工业园区")
            })
        })
    };

    [ObservableProperty]
    private string _selectedPath = "请选择";
}

public class CascaderItem
{
    public string Label { get; }
    public ObservableCollection<CascaderItem>? Children { get; }

    public CascaderItem(string label, ObservableCollection<CascaderItem>? children = null)
    {
        Label = label;
        Children = children;
    }
}
