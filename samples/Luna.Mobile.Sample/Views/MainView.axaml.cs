using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Media;
using System.ComponentModel;
using System.Collections.Generic;

namespace Luna.Mobile.Sample.Views;

public partial class MainView : UserControl
{
    private Control? _catalogContent;

    public string Title { get; } = "Luna 适配 Avalonia 的移动端组件库";

    public IReadOnlyList<CatalogSection> Sections { get; } =
    [
        new("全局配置", "internet", Icons.Internet, true,
        [
            new("全局特性配置", "/global-config"),
        ]),
        new("基础", "app", Icons.App, true,
        [
            new("Button 按钮", "/button"),
            new("Divider 分割线", "/divider"),
            new("Fab 悬浮按钮", "/fab"),
            new("Icon 图标", "/icon"),
            new("Layout 布局", "/layout"),
            new("Link 链接", "/link"),
        ]),
        new("导航", "view-module", Icons.ViewModule, true,
        [
            new("BackTop 返回顶部", "/backtop"),
            new("Drawer 抽屉", "/drawer"),
            new("Indexes 索引", "/indexes"),
            new("Navbar 导航栏", "/navbar"),
            new("SideBar 侧边栏", "/sidebar"),
            new("Steps 步骤条", "/steps"),
            new("TabBar 底部标签栏", "/tabbar"),
            new("Tabs 选项卡", "/tabs"),
        ]),
        new("输入", "bulletpoint", Icons.Bulletpoint, true,
        [
            new("Calendar 日历", "/calendar"),
            new("Cascader 级联选择器", "/cascader"),
            new("CheckBox 多选框", "/checkbox"),
            new("ColorPicker 颜色选择器", "/colorpicker"),
            new("DateTimePicker 时间选择器", "/datetimepicker"),
            new("Form 表单", "/form"),
            new("Input 输入框", "/input"),
            new("Picker 选择器", "/picker"),
            new("Radio 单选框", "/radio"),
            new("Rate 评分", "/rate"),
            new("Search 搜索框", "/search"),
            new("Slider 滑动选择器", "/slider"),
            new("Stepper 步进器", "/stepper"),
            new("Switch 开关", "/switch"),
            new("Textarea 多行文本框", "/textarea"),
            new("TreeSelect 树形选择器", "/treeselect"),
            new("Upload 上传", "/upload"),
        ]),
        new("数据展示", "image", Icons.Image, true,
        [
            new("Avatar 头像", "/avatar"),
            new("Badge 徽标", "/badge"),
            new("Cell 单元格", "/cell"),
            new("Collapse 折叠面板", "/collapse"),
            new("CountDown 倒计时", "/countdown"),
            new("Empty 空状态", "/empty"),
            new("Footer 页脚", "/footer"),
            new("Grid 宫格", "/grid"),
            new("Image 图片", "/image"),
            new("ImageViewer 图片预览", "/imageviewer"),
            new("List 列表", "/list"),
            new("Progress 进度条", "/progress"),
            new("QRCode 二维码", "/qrcode"),
            new("Result 结果", "/result"),
            new("Segmented 分段控制器", "/segmented"),
            new("Skeleton 骨架屏", "/skeleton"),
            new("Sticky 吸顶容器", "/sticky"),
            new("Swiper 轮播图", "/swiper"),
            new("Table 表格", "/table"),
            new("Tag 标签", "/tag"),
            new("Watermark 水印", "/watermark"),
        ]),
        new("反馈", "chevron-up", Icons.ChevronUp, true,
        [
            new("ActionSheet 动作面板", "/actionsheet"),
            new("Dialog 对话框", "/dialog"),
            new("DropdownMenu 下拉菜单", "/dropdownmenu"),
            new("Guide 引导", "/guide"),
            new("Loading 加载", "/loading"),
            new("Message 消息通知", "/message"),
            new("NoticeBar 公告栏", "/noticebar"),
            new("Overlay 遮罩层", "/overlay"),
            new("Popover 弹出气泡", "/popover"),
            new("Popup 弹出层", "/popup"),
            new("PullDownRefresh 下拉刷新", "/pulldownrefresh"),
            new("SwipeCell 滑动操作", "/swipecell"),
            new("Toast 轻提示", "/toast"),
        ]),
    ];

    public MainView()
    {
        InitializeComponent();
        DataContext = this;
        _catalogContent = Content as Control;
    }

    private void OnCatalogItemTapped(object? sender, TappedEventArgs e)
    {
        if (sender is not Control { Tag: string path })
        {
            return;
        }

        Content = path switch
        {
            "/button" => AttachBackHandler(new ButtonDemoView()),
            "/radio" => AttachBackHandler(new RadioDemoView()),
            "/switch" => AttachBackHandler(new SwitchDemoView()),
            _ => _catalogContent,
        };
    }

    private UserControl AttachBackHandler(ButtonDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(SwitchDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }

    private UserControl AttachBackHandler(RadioDemoView view)
    {
        view.BackRequested += (_, _) => Content = _catalogContent;
        return view;
    }
}

public sealed class CatalogSection : INotifyPropertyChanged
{
    private bool _isExpanded;

    public string Title { get; }
    public string Icon { get; }
    public Geometry IconData { get; }
    public IReadOnlyList<CatalogItem> Children { get; }

    public bool IsExpanded
    {
        get => _isExpanded;
        set
        {
            if (value == _isExpanded)
            {
                return;
            }

            _isExpanded = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsExpanded)));
        }
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    public CatalogSection(string title, string icon, Geometry iconData, bool isExpanded, IReadOnlyList<CatalogItem> children)
    {
        Title = title;
        Icon = icon;
        IconData = iconData;
        _isExpanded = isExpanded;
        Children = children;
    }
}

public sealed record CatalogItem(string Name, string Path);

internal static class Icons
{
    public static Geometry Internet { get; } = Geometry.Parse(
        "M3 12H21 M2 12C2 6.47715 6.47715 2 12 2C17.5228 2 22 6.47715 22 12C22 17.5229 17.5228 22 12 22C6.47715 22 2 17.5229 2 12Z M11.4987 21.9877C6.88584 16.2216 6.88581 7.77835 11.4988 2.01231 M12.5 21.9877C17.113 16.2216 17.1129 7.77835 12.4999 2.01231");

    public static Geometry App { get; } = Geometry.Parse(
        "M3 3H10V10H3V3Z M14 14H21V21H14V14Z M3 14H10V21H3V14Z M21.5 6.5C21.5 8.70914 19.7091 10.5 17.5 10.5C15.2909 10.5 13.5 8.70914 13.5 6.5C13.5 4.29086 15.2909 2.5 17.5 2.5C19.7091 2.5 21.5 4.29086 21.5 6.5Z");

    public static Geometry ViewModule { get; } = Geometry.Parse(
        "M8.6665 4V20 M15.333 4V20 M2 12H22 M2 20H22V4H2V20Z");

    public static Geometry Bulletpoint { get; } = Geometry.Parse(
        "M8 5H21 M8 12H21 M8 19H21 M3 5H3.01 M3 12H3.01 M3 19H3.01");

    public static Geometry Image { get; } = Geometry.Parse(
        "M3 16V3H21V21H20 M3 16V21H20 M3 16L9 10L20 21 M15.75 6.25A2 2 0 1 1 15.75 10.25A2 2 0 1 1 15.75 6.25");

    public static Geometry ChevronUp { get; } = Geometry.Parse("M17.5 14.5L12 9L6.5 14.5");
}
