using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.ViewModels;

public partial class CascaderDemoViewModel : DemoViewModelBase
{
    public event Action<CascaderOptions>? CascaderRequested;

    [RelayCommand]
    private void AddressCascader()
    {
        RequestCascader(new CascaderOptions
        {
            Title = "选择地址",
            Placeholder = "请选择",
            SubTitles = ["省份", "城市", "区县"],
            Options = BuildAddressOptions(),
            Value = "hangzhou-xihu",
        });
    }

    [RelayCommand]
    private void CategoryCascader()
    {
        RequestCascader(new CascaderOptions
        {
            Title = "选择分类",
            Placeholder = "请选择分类",
            Theme = CascaderTheme.Tab,
            Options = BuildCategoryOptions(),
        });
    }

    [RelayCommand]
    private void StrictCascader()
    {
        RequestCascader(new CascaderOptions
        {
            Title = "父节点可选",
            Placeholder = "请选择节点",
            CheckStrictly = true,
            Options = BuildCategoryOptions(),
        });
    }

    [RelayCommand]
    private void CustomHeightCascader()
    {
        RequestCascader(new CascaderOptions
        {
            Title = "自定义高度",
            Placeholder = "请选择业务线",
            SheetHeight = 420,
            SubTitles = ["业务", "模块", "页面"],
            Options = BuildBusinessOptions(),
        });
    }

    public void OnCascaderChanged(CascaderChangedEventArgs e)
    {
        var text = string.Join(" / ", e.SelectedOptions.Select(option => option.Label));
        Toast.Show(string.IsNullOrWhiteSpace(text) ? e.Value : text);
    }

    private void RequestCascader(CascaderOptions options)
    {
        CascaderRequested?.Invoke(options);
    }

    private static IReadOnlyList<CascaderOption> BuildAddressOptions()
    {
        return
        [
            new CascaderOption
            {
                Label = "浙江省",
                Value = "zhejiang",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "杭州市",
                        Value = "hangzhou",
                        Children =
                        [
                            new CascaderOption { Label = "西湖区", Value = "hangzhou-xihu" },
                            new CascaderOption { Label = "滨江区", Value = "hangzhou-binjiang" },
                            new CascaderOption { Label = "余杭区", Value = "hangzhou-yuhang" },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = "宁波市",
                        Value = "ningbo",
                        Children =
                        [
                            new CascaderOption { Label = "海曙区", Value = "ningbo-haishu" },
                            new CascaderOption { Label = "江北区", Value = "ningbo-jiangbei" },
                            new CascaderOption { Label = "鄞州区", Value = "ningbo-yinzhou" },
                        ],
                    },
                ],
            },
            new CascaderOption
            {
                Label = "广东省",
                Value = "guangdong",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "广州市",
                        Value = "guangzhou",
                        Children =
                        [
                            new CascaderOption { Label = "天河区", Value = "guangzhou-tianhe" },
                            new CascaderOption { Label = "越秀区", Value = "guangzhou-yuexiu" },
                            new CascaderOption { Label = "番禺区", Value = "guangzhou-panyu" },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = "深圳市",
                        Value = "shenzhen",
                        Children =
                        [
                            new CascaderOption { Label = "南山区", Value = "shenzhen-nanshan" },
                            new CascaderOption { Label = "福田区", Value = "shenzhen-futian" },
                            new CascaderOption { Label = "龙岗区", Value = "shenzhen-longgang" },
                        ],
                    },
                ],
            },
        ];
    }

    private static IReadOnlyList<CascaderOption> BuildCategoryOptions()
    {
        return
        [
            new CascaderOption
            {
                Label = "前端",
                Value = "frontend",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "桌面端",
                        Value = "desktop",
                        Children =
                        [
                            new CascaderOption { Label = "Avalonia", Value = "avalonia" },
                            new CascaderOption { Label = "WPF", Value = "wpf", IsDisabled = true },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = "Web",
                        Value = "web",
                        Children =
                        [
                            new CascaderOption { Label = "React", Value = "react" },
                            new CascaderOption { Label = "Vue", Value = "vue" },
                        ],
                    },
                ],
            },
            new CascaderOption
            {
                Label = "服务端",
                Value = "backend",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "JVM",
                        Value = "jvm",
                        Children =
                        [
                            new CascaderOption { Label = "Kotlin", Value = "kotlin" },
                            new CascaderOption { Label = "Java", Value = "java" },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = ".NET",
                        Value = "dotnet",
                        Children =
                        [
                            new CascaderOption { Label = "ASP.NET Core", Value = "aspnet-core" },
                            new CascaderOption { Label = "Worker", Value = "worker" },
                        ],
                    },
                ],
            },
        ];
    }

    private static IReadOnlyList<CascaderOption> BuildBusinessOptions()
    {
        return
        [
            new CascaderOption
            {
                Label = "交易",
                Value = "trade",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "下单",
                        Value = "order",
                        Children =
                        [
                            new CascaderOption { Label = "现货下单页", Value = "spot-order" },
                            new CascaderOption { Label = "合约下单页", Value = "future-order" },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = "仓位",
                        Value = "position",
                        Children =
                        [
                            new CascaderOption { Label = "当前持仓", Value = "active-position" },
                            new CascaderOption { Label = "历史持仓", Value = "history-position" },
                        ],
                    },
                ],
            },
            new CascaderOption
            {
                Label = "策略",
                Value = "strategy",
                Children =
                [
                    new CascaderOption
                    {
                        Label = "创建",
                        Value = "create",
                        Children =
                        [
                            new CascaderOption { Label = "模板市场", Value = "template-market" },
                            new CascaderOption { Label = "空白策略", Value = "blank-strategy" },
                        ],
                    },
                    new CascaderOption
                    {
                        Label = "运行",
                        Value = "runtime",
                        Children =
                        [
                            new CascaderOption { Label = "实时监控", Value = "runtime-monitor" },
                            new CascaderOption { Label = "回测报告", Value = "backtest-report" },
                        ],
                    },
                ],
            },
        ];
    }
}
