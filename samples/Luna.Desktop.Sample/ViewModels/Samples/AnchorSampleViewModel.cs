using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class AnchorSampleViewModel : SampleDetailViewModelBase
{
    public AnchorSampleViewModel() : base("导航", "Anchor", "锚点导航，用于页面内快速跳转定位。")
    {
    }

    [ObservableProperty]
    private ObservableCollection<AnchorSection> _sections = new()
    {
        new("概述", "overview", "Luna Desktop 是一套基于 Avalonia UI 的桌面端组件库，遵循 TDesign 设计规范。提供丰富的控件、主题和布局支持，帮助开发者快速构建美观的桌面应用。Luna 的目标是统一桌面端与移动端的视觉语言和控件 API，实现跨平台一致体验。"),
        new("特性", "features", "支持明暗主题切换、Token 化设计、MVVM 绑定、响应式布局。覆盖基础、表单、数据展示、反馈和导航五大控件域。所有控件均支持深色主题和浅色主题，使用 Luna.* token 资源体系。"),
        new("快速开始", "quickstart", "通过 NuGet 安装 Luna.Desktop 包，在 App.axaml 中引用主题资源，即可开始使用所有控件。控件库提供完整的示例项目 Luna.Desktop.Sample，可通过 catalog 浏览所有控件。"),
        new("主题系统", "theme", "Luna 使用 Token 化主题系统，支持品牌色、功能色、文本色、背景色、边框色、圆角、间距、字号、阴影等设计变量。切换浅色/深色主题时，所有控件自动适配。"),
        new("控件列表", "controls", "Luna Desktop 当前包含 60+ 控件，涵盖 Button、Input、Select、Table、Dialog、Menu、Tabs、Steps、Pagination 等常用控件。每个控件都有完整的主题样式和示例页面。")
    };
}

public class AnchorSection
{
    public string Title { get; }
    public string Id { get; }
    public string Description { get; }

    public AnchorSection(string title, string id, string description)
    {
        Title = title;
        Id = id;
        Description = description;
    }
}
