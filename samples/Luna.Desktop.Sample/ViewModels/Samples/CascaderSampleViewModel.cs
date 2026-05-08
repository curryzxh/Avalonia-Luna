using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using Luna.Desktop.Controls;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class CascaderSampleViewModel : SampleDetailViewModelBase
{
    public CascaderSampleViewModel() : base("数据录入", "Cascader", "级联选择器，用于多级数据的选择。")
    {
    }

    [ObservableProperty]
    private ObservableCollection<CascaderOption> _options = new()
    {
        new CascaderOption
        {
            Label = "北京市",
            Value = "beijing",
            Children = new ObservableCollection<CascaderOption>
            {
                new()
                {
                    Label = "市辖区",
                    Value = "beijing_shixiaqu",
                    Children = new ObservableCollection<CascaderOption>
                    {
                        new() { Label = "东城区", Value = "dongcheng" },
                        new() { Label = "西城区", Value = "xicheng" },
                        new() { Label = "朝阳区", Value = "chaoyang" },
                        new() { Label = "海淀区", Value = "haidian" },
                    }
                }
            }
        },
        new CascaderOption
        {
            Label = "上海市",
            Value = "shanghai",
            Children = new ObservableCollection<CascaderOption>
            {
                new()
                {
                    Label = "市辖区",
                    Value = "shanghai_shixiaqu",
                    Children = new ObservableCollection<CascaderOption>
                    {
                        new() { Label = "黄浦区", Value = "huangpu" },
                        new() { Label = "徐汇区", Value = "xuhui" },
                        new() { Label = "浦东新区", Value = "pudong" },
                    }
                }
            }
        },
        new CascaderOption
        {
            Label = "广东省",
            Value = "guangdong",
            Children = new ObservableCollection<CascaderOption>
            {
                new()
                {
                    Label = "深圳市",
                    Value = "shenzhen",
                    Children = new ObservableCollection<CascaderOption>
                    {
                        new() { Label = "南山区", Value = "nanshan" },
                        new() { Label = "福田区", Value = "futian" },
                        new() { Label = "宝安区", Value = "baoan" },
                    }
                },
                new()
                {
                    Label = "广州市",
                    Value = "guangzhou",
                    Children = new ObservableCollection<CascaderOption>
                    {
                        new() { Label = "天河区", Value = "tianhe" },
                        new() { Label = "越秀区", Value = "yuexiu" },
                    }
                }
            }
        }
    };
}
