using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class DesktopBadge : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<DesktopBadge, string?>(nameof(Text));

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<DesktopBadge, int>(nameof(Count), defaultValue: 0);

    public static readonly StyledProperty<int> MaxCountProperty =
        AvaloniaProperty.Register<DesktopBadge, int>(nameof(MaxCount), defaultValue: 99);

    public static readonly StyledProperty<bool> IsDotProperty =
        AvaloniaProperty.Register<DesktopBadge, bool>(nameof(IsDot), defaultValue: false);

    public static readonly StyledProperty<DesktopBadgeShape> ShapeProperty =
        AvaloniaProperty.Register<DesktopBadge, DesktopBadgeShape>(nameof(Shape), defaultValue: DesktopBadgeShape.Round);

    public static readonly StyledProperty<Thickness> OffsetProperty =
        AvaloniaProperty.Register<DesktopBadge, Thickness>(nameof(Offset), defaultValue: default);

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public int MaxCount
    {
        get => GetValue(MaxCountProperty);
        set => SetValue(MaxCountProperty, value);
    }

    public bool IsDot
    {
        get => GetValue(IsDotProperty);
        set => SetValue(IsDotProperty, value);
    }

    public DesktopBadgeShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public Thickness Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    static DesktopBadge()
    {
        CountProperty.Changed.AddClassHandler<DesktopBadge>(OnCountChanged);
        MaxCountProperty.Changed.AddClassHandler<DesktopBadge>(OnCountChanged);
        TextProperty.Changed.AddClassHandler<DesktopBadge>(OnTextChanged);
        IsDotProperty.Changed.AddClassHandler<DesktopBadge>((x, _) => x.UpdatePseudoClasses());
        ShapeProperty.Changed.AddClassHandler<DesktopBadge>((x, _) => x.UpdatePseudoClasses());
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UpdateDisplayText();
        UpdatePseudoClasses();
    }

    private static void OnCountChanged(DesktopBadge badge, AvaloniaPropertyChangedEventArgs e)
    {
        badge.UpdateDisplayText();
    }

    private static void OnTextChanged(DesktopBadge badge, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.NewValue is not null)
        {
            badge.UpdatePseudoClasses();
        }
    }

    private void UpdateDisplayText()
    {
        if (Text is not null)
            return;

        if (Count <= 0)
        {
            SetValue(TextProperty, null);
            PseudoClasses.Set(":hidden", true);
            return;
        }

        PseudoClasses.Set(":hidden", false);
        SetValue(TextProperty, Count > MaxCount ? $"{MaxCount}+" : Count.ToString());
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":dot", IsDot);
        PseudoClasses.Set(":round", Shape == DesktopBadgeShape.Round);
        PseudoClasses.Set(":circle", Shape == DesktopBadgeShape.Circle);
        PseudoClasses.Set(":hidden", !IsDot && Count <= 0 && Text is null);
    }
}

public enum DesktopBadgeShape
{
    Round,
    Circle,
}
