using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using System;

namespace Luna.Mobile.Controls;

public enum BadgeShape
{
    Circle,
    Square,
    Bubble,
}

public enum BadgePlacement
{
    TopRight,
    TopLeft,
    BottomRight,
    BottomLeft,
}

public sealed class Badge : ContentControl
{
    private bool _isBadgeVisible;
    private bool _isBadgeTextVisible;
    private string? _displayCount;
    private Thickness _badgeMargin;
    private HorizontalAlignment _badgeHorizontalAlignment = HorizontalAlignment.Right;
    private VerticalAlignment _badgeVerticalAlignment = VerticalAlignment.Top;

    public static readonly StyledProperty<string?> CountProperty =
        AvaloniaProperty.Register<Badge, string?>(nameof(Count));

    public static readonly StyledProperty<int> OverflowCountProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(OverflowCount), 99);

    public static readonly StyledProperty<bool> DotProperty =
        AvaloniaProperty.Register<Badge, bool>(nameof(Dot));

    public static readonly StyledProperty<BadgeShape> ShapeProperty =
        AvaloniaProperty.Register<Badge, BadgeShape>(nameof(Shape), BadgeShape.Circle);

    public static readonly StyledProperty<BadgePlacement> PlacementProperty =
        AvaloniaProperty.Register<Badge, BadgePlacement>(nameof(Placement), BadgePlacement.TopRight);

    public static readonly StyledProperty<Vector> OffsetProperty =
        AvaloniaProperty.Register<Badge, Vector>(nameof(Offset));

    public static readonly DirectProperty<Badge, bool> IsBadgeVisibleProperty =
        AvaloniaProperty.RegisterDirect<Badge, bool>(
            nameof(IsBadgeVisible),
            o => o.IsBadgeVisible);

    public static readonly DirectProperty<Badge, bool> IsBadgeTextVisibleProperty =
        AvaloniaProperty.RegisterDirect<Badge, bool>(
            nameof(IsBadgeTextVisible),
            o => o.IsBadgeTextVisible);

    public static readonly DirectProperty<Badge, string?> DisplayCountProperty =
        AvaloniaProperty.RegisterDirect<Badge, string?>(
            nameof(DisplayCount),
            o => o.DisplayCount);

    public static readonly DirectProperty<Badge, HorizontalAlignment> BadgeHorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<Badge, HorizontalAlignment>(
            nameof(BadgeHorizontalAlignment),
            o => o.BadgeHorizontalAlignment);

    public static readonly DirectProperty<Badge, VerticalAlignment> BadgeVerticalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<Badge, VerticalAlignment>(
            nameof(BadgeVerticalAlignment),
            o => o.BadgeVerticalAlignment);

    public static readonly DirectProperty<Badge, Thickness> BadgeMarginProperty =
        AvaloniaProperty.RegisterDirect<Badge, Thickness>(
            nameof(BadgeMargin),
            o => o.BadgeMargin);

    static Badge()
    {
        CountProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        OverflowCountProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        DotProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        ShapeProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        PlacementProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
    }

    public string? Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public int OverflowCount
    {
        get => GetValue(OverflowCountProperty);
        set => SetValue(OverflowCountProperty, value);
    }

    public bool Dot
    {
        get => GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    public BadgeShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public BadgePlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public Vector Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public bool IsBadgeVisible
    {
        get => _isBadgeVisible;
        private set => SetAndRaise(IsBadgeVisibleProperty, ref _isBadgeVisible, value);
    }

    public bool IsBadgeTextVisible
    {
        get => _isBadgeTextVisible;
        private set => SetAndRaise(IsBadgeTextVisibleProperty, ref _isBadgeTextVisible, value);
    }

    public string? DisplayCount
    {
        get => _displayCount;
        private set => SetAndRaise(DisplayCountProperty, ref _displayCount, value);
    }

    public HorizontalAlignment BadgeHorizontalAlignment
    {
        get => _badgeHorizontalAlignment;
        private set => SetAndRaise(BadgeHorizontalAlignmentProperty, ref _badgeHorizontalAlignment, value);
    }

    public VerticalAlignment BadgeVerticalAlignment
    {
        get => _badgeVerticalAlignment;
        private set => SetAndRaise(BadgeVerticalAlignmentProperty, ref _badgeVerticalAlignment, value);
    }

    public Thickness BadgeMargin
    {
        get => _badgeMargin;
        private set => SetAndRaise(BadgeMarginProperty, ref _badgeMargin, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    private void UpdateState()
    {
        var hasText = !string.IsNullOrWhiteSpace(Count);
        IsBadgeVisible = Dot || hasText;

        if (Dot || !hasText)
        {
            DisplayCount = null;
            IsBadgeTextVisible = false;
        }
        else
        {
            DisplayCount = CoerceCount(Count!, OverflowCount);
            IsBadgeTextVisible = !string.IsNullOrEmpty(DisplayCount);
        }

        PseudoClasses.Set(":dot", Dot);
        PseudoClasses.Set(":circle", Shape == BadgeShape.Circle);
        PseudoClasses.Set(":square", Shape == BadgeShape.Square);
        PseudoClasses.Set(":bubble", Shape == BadgeShape.Bubble);

        (BadgeHorizontalAlignment, BadgeVerticalAlignment) = Placement switch
        {
            BadgePlacement.TopLeft => (HorizontalAlignment.Left, VerticalAlignment.Top),
            BadgePlacement.BottomRight => (HorizontalAlignment.Right, VerticalAlignment.Bottom),
            BadgePlacement.BottomLeft => (HorizontalAlignment.Left, VerticalAlignment.Bottom),
            _ => (HorizontalAlignment.Right, VerticalAlignment.Top),
        };

        var overlap = Dot ? 4 : 6;
        BadgeMargin = Placement switch
        {
            BadgePlacement.TopLeft => new Thickness(-overlap, -overlap, 0, 0),
            BadgePlacement.BottomRight => new Thickness(0, 0, -overlap, -overlap),
            BadgePlacement.BottomLeft => new Thickness(-overlap, 0, 0, -overlap),
            _ => new Thickness(0, -overlap, -overlap, 0),
        };
    }

    private static string CoerceCount(string count, int overflowCount)
    {
        if (overflowCount <= 0)
        {
            return count;
        }

        if (!int.TryParse(count, out var value))
        {
            return count;
        }

        return value > overflowCount ? $"{overflowCount}+" : count;
    }
}
