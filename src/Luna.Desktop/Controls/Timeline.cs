using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public enum TimelineMode
{
    Left,
    Right,
    Alternate,
}

public enum TimelineItemType
{
    Default,
    Success,
    Warning,
    Error,
    Primary,
}

public class TimelineItem : HeaderedContentControl
{
    public static readonly StyledProperty<string?> TimestampProperty =
        AvaloniaProperty.Register<TimelineItem, string?>(nameof(Timestamp));

    public static readonly StyledProperty<TimelineItemType> ItemTypeProperty =
        AvaloniaProperty.Register<TimelineItem, TimelineItemType>(nameof(ItemType), TimelineItemType.Default);

    public string? Timestamp
    {
        get => GetValue(TimestampProperty);
        set => SetValue(TimestampProperty, value);
    }

    public TimelineItemType ItemType
    {
        get => GetValue(ItemTypeProperty);
        set => SetValue(ItemTypeProperty, value);
    }

    static TimelineItem()
    {
        ItemTypeProperty.Changed.AddClassHandler<TimelineItem>((item, _) => item.UpdateType());
    }

    public TimelineItem()
    {
        UpdateType();
    }

    internal void SetPosition(bool isLeft)
    {
        PseudoClasses.Set(":left", isLeft);
        PseudoClasses.Set(":right", !isLeft);
    }

    internal void SetEnd(bool isFirst, bool isLast)
    {
        PseudoClasses.Set(":first", isFirst);
        PseudoClasses.Set(":last", isLast);
    }

    private void UpdateType()
    {
        PseudoClasses.Set(":default", ItemType == TimelineItemType.Default);
        PseudoClasses.Set(":success", ItemType == TimelineItemType.Success);
        PseudoClasses.Set(":warning", ItemType == TimelineItemType.Warning);
        PseudoClasses.Set(":error", ItemType == TimelineItemType.Error);
        PseudoClasses.Set(":primary", ItemType == TimelineItemType.Primary);
    }
}

public class Timeline : ItemsControl
{
    public static readonly StyledProperty<TimelineMode> ModeProperty =
        AvaloniaProperty.Register<Timeline, TimelineMode>(nameof(Mode), TimelineMode.Left);

    public TimelineMode Mode
    {
        get => GetValue(ModeProperty);
        set => SetValue(ModeProperty, value);
    }

    static Timeline()
    {
        ModeProperty.Changed.AddClassHandler<Timeline>((t, _) => t.UpdateItemPositions());
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        recycleKey = null;
        return item is not TimelineItem;
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return item is TimelineItem ti ? ti : new TimelineItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        base.PrepareContainerForItemOverride(container, item, index);
        if (container is TimelineItem ti)
        {
            ti.SetEnd(index == 0, index == ItemCount - 1);
            UpdateItemPosition(ti, index);
        }
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        UpdateItemPositions();
        return base.ArrangeOverride(finalSize);
    }

    private void UpdateItemPositions()
    {
        var index = 0;
        foreach (var child in GetVisualChildren())
        {
            if (child is TimelineItem ti)
            {
                ti.SetEnd(index == 0, index == ItemCount - 1);
                UpdateItemPosition(ti, index);
                index++;
            }
        }
    }

    private void UpdateItemPosition(TimelineItem item, int index)
    {
        switch (Mode)
        {
            case TimelineMode.Left:
                item.SetPosition(false);
                break;
            case TimelineMode.Right:
                item.SetPosition(true);
                break;
            case TimelineMode.Alternate:
                item.SetPosition(index % 2 == 0);
                break;
        }
    }
}
