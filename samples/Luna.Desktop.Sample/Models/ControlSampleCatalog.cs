using System.Collections.Generic;
using Luna.Desktop.Sample.ViewModels.Samples;

namespace Luna.Desktop.Sample.Models;

public static class ControlSampleCatalog
{
    public static ControlCategory Base { get; } = new("Base", "基础", "按钮、徽标和桌面基础操作入口。");

    public static ControlCategory Form { get; } = new("Form", "输入", "文本、选择、开关、滑块和表单常用输入。");

    public static ControlCategory Data { get; } = new("Data", "数据展示", "状态、标签、头像、空状态和进度反馈。");

    public static ControlCategory Message { get; } = new("Message", "反馈", "通知、对话、弹层和全局反馈。");

    public static ControlCategory Navigation { get; } = new("Navigation", "导航", "菜单、标签页、面包屑、分页和步骤导航。");

    public static ControlCategory Layout { get; } = new("Layout", "布局", "分割线、间距、网格、描述列表和页面布局。");

    public static IReadOnlyList<SampleNavigationItem> CreateSamples()
    {
        return
        [
            Create(Base, new DesktopBadgeSampleViewModel(), "https://tdesign.tencent.com/react/components/badge"),
            Create(Base, new ButtonSampleViewModel(), "https://tdesign.tencent.com/react/components/button"),

            Create(Form, new InputSampleViewModel(), "https://tdesign.tencent.com/react/components/input"),
            Create(Form, new CheckBoxSampleViewModel(), "https://tdesign.tencent.com/react/components/checkbox"),
            Create(Form, new RadioSampleViewModel(), "https://tdesign.tencent.com/react/components/radio"),
            Create(Form, new SwitchSampleViewModel(), "https://tdesign.tencent.com/react/components/switch"),
            Create(Form, new SliderSampleViewModel(), "https://tdesign.tencent.com/react/components/slider"),
            Create(Form, new TextareaSampleViewModel(), "https://tdesign.tencent.com/react/components/textarea"),
            Create(Form, new InputNumberSampleViewModel(), "https://tdesign.tencent.com/react/components/input-number"),
            Create(Form, new SelectSampleViewModel(), "https://tdesign.tencent.com/react/components/select"),
            Create(Form, new AutoCompleteSampleViewModel(), "https://tdesign.tencent.com/react/components/auto-complete"),
            Create(Form, new FormSampleViewModel(), "https://tdesign.tencent.com/react/components/form"),
            Create(Form, new DatePickerSampleViewModel(), "https://tdesign.tencent.com/react/components/date-picker"),
            Create(Form, new TimePickerSampleViewModel(), "https://tdesign.tencent.com/react/components/time-picker"),
            Create(Form, new UploadSampleViewModel(), "https://tdesign.tencent.com/react/components/upload"),
            Create(Form, new CascaderSampleViewModel(), "https://tdesign.tencent.com/react/components/cascader"),
            Create(Form, new RateSampleViewModel(), "https://tdesign.tencent.com/react/components/rate"),
            Create(Form, new ColorPickerSampleViewModel(), "https://tdesign.tencent.com/react/components/color-picker"),

            Create(Data, new ProgressSampleViewModel(), "https://tdesign.tencent.com/react/components/progress"),
            Create(Data, new TagSampleViewModel(), "https://tdesign.tencent.com/react/components/tag"),
            Create(Data, new AvatarSampleViewModel(), "https://tdesign.tencent.com/react/components/avatar"),
            Create(Data, new EmptySampleViewModel(), "https://tdesign.tencent.com/react/components/empty"),

            Create(Message, new NotificationSampleViewModel(), "https://tdesign.tencent.com/react/components/notification"),

            Create(Navigation, new TabsSampleViewModel(), "https://tdesign.tencent.com/react/components/tabs"),
            Create(Navigation, new MenuSampleViewModel(), "https://tdesign.tencent.com/react/components/menu"),
            Create(Navigation, new DropdownSampleViewModel(), "https://tdesign.tencent.com/react/components/dropdown"),
            Create(Navigation, new PaginationSampleViewModel(), "https://tdesign.tencent.com/react/components/pagination"),
            Create(Navigation, new BreadcrumbSampleViewModel(), "https://tdesign.tencent.com/react/components/breadcrumb"),
            Create(Navigation, new StepsSampleViewModel(), "https://tdesign.tencent.com/react/components/steps"),
            Create(Navigation, new AnchorSampleViewModel(), "https://tdesign.tencent.com/react/components/anchor"),
            Create(Navigation, new AffixSampleViewModel(), "https://tdesign.tencent.com/react/components/affix"),
            Create(Navigation, new StickyToolSampleViewModel(), "https://tdesign.tencent.com/react/components/back-top"),

            Create(Message, new AlertSampleViewModel(), "https://tdesign.tencent.com/react/components/alert"),
            Create(Message, new DialogSampleViewModel(), "https://tdesign.tencent.com/react/components/dialog"),
            Create(Message, new DrawerSampleViewModel(), "https://tdesign.tencent.com/react/components/drawer"),
            Create(Message, new MessageSampleViewModel(), "https://tdesign.tencent.com/react/components/message"),
            Create(Message, new LoadingSampleViewModel(), "https://tdesign.tencent.com/react/components/loading"),
            Create(Message, new PopupSampleViewModel(), "https://tdesign.tencent.com/react/components/popup"),
            Create(Message, new PopconfirmSampleViewModel(), "https://tdesign.tencent.com/react/components/popconfirm"),
            Create(Message, new SwiperSampleViewModel(), "https://tdesign.tencent.com/react/components/swiper"),
        ];
    }

    private static SampleNavigationItem Create(
        ControlCategory category,
        SampleDetailViewModelBase content,
        string? documentationUrl)
    {
        return new SampleNavigationItem(category, content, documentationUrl);
    }
}
