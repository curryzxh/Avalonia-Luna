using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class AnchorSampleViewModel()
    : SampleDetailViewModelBase("导航", "Anchor", "锚点导航，用于页面内快速跳转定位。")
{
    public ObservableCollection<AnchorSection> Sections { get; } =
    [
        new("概述", "overview",
            "Luna Desktop 是一套基于 Avalonia UI 的桌面端组件库，遵循 TDesign 设计规范。提供丰富的控件、主题和布局支持，帮助开发者快速构建美观的桌面应用。它覆盖了基础、表单、数据展示、反馈和导航五大控件域，同时支持明暗主题切换和 Token 化设计。"),
        new("特性", "features",
            "支持明暗主题切换、Token 化设计、MVVM 绑定、响应式布局。覆盖基础、表单、数据展示、反馈和导航五大控件域。所有控件均遵循统一视觉语言，支持桌面端与移动端一致体验。"),
        new("快速开始", "quick-start",
            "通过 NuGet 安装 Luna.Desktop 包，在 App.axaml 中引用主题资源，即可开始使用所有控件。添加命名空间 xmlns:controls=\"using:Luna.Desktop.Controls\" 后，直接在 XAML 中使用 Luna 控件。"),
        new("API 参考", "api",
            "Anchor 控件提供 ItemsSource 数据绑定、TargetContainer 关联滚动容器、AnchorId 附加属性标记锚点目标、TopOffset 滚动偏移量等核心 API。AnchorItem 作为容器控件支持 Header 模板和选中状态。"),
        new("常见问题", "faq",
            "常见问题包括：锚点定位不准确时检查 TopOffset 是否设置；滚动高亮不生效时确认 TargetContainer 已正确关联；嵌套锚点场景使用 AnchorItem 子项实现多级导航。")
    ];
}

public partial class AnchorSection : ObservableObject
{
    [ObservableProperty]
    private string _title;

    [ObservableProperty]
    private string _anchorId;

    [ObservableProperty]
    private string _description;

    public AnchorSection(string title, string anchorId, string description)
    {
        _title = title;
        _anchorId = anchorId;
        _description = description;
    }
}
