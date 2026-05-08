using System.Collections.Generic;
using Luna.Desktop.Sample.ViewModels.Samples;

namespace Luna.Desktop.Sample.Models;

public static class ControlSampleCatalog
{
    public static ControlCategory Base { get; } = new("Base", "基础", "按钮、徽标和桌面基础操作入口。");

    public static ControlCategory Form { get; } = new("Form", "输入", "文本、选择、开关、滑块和表单常用输入。");

    public static ControlCategory Data { get; } = new("Data", "数据展示", "状态、标签、头像、空状态和进度反馈。");

    public static ControlCategory Message { get; } = new("Message", "反馈", "通知、对话、弹层和全局反馈。");

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

            Create(Data, new ProgressSampleViewModel(), "https://tdesign.tencent.com/react/components/progress"),
            Create(Data, new TagSampleViewModel(), "https://tdesign.tencent.com/react/components/tag"),
            Create(Data, new AvatarSampleViewModel(), "https://tdesign.tencent.com/react/components/avatar"),
            Create(Data, new EmptySampleViewModel(), "https://tdesign.tencent.com/react/components/empty"),

            Create(Message, new NotificationSampleViewModel(), "https://tdesign.tencent.com/react/components/notification"),
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
