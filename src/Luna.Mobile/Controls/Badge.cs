using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 徽标的形状。
/// </summary>
public enum BadgeShape
{
    /// <summary>
    /// Circle。
    /// </summary>
    Circle,
    /// <summary>
    /// Square。
    /// </summary>
    Square,
    /// <summary>
    /// Bubble。
    /// </summary>
    Bubble,
}

/// <summary>
/// 徽标相对于内容的位置。
/// </summary>
public enum BadgePlacement
{
    /// <summary>
    /// TopRight。
    /// </summary>
    TopRight,
    /// <summary>
    /// TopLeft。
    /// </summary>
    TopLeft,
    /// <summary>
    /// BottomRight。
    /// </summary>
    BottomRight,
    /// <summary>
    /// BottomLeft。
    /// </summary>
    BottomLeft,
}

/// <summary>
/// 徽标控件，用于展示数字/点状提示，并可包裹任意内容。
/// </summary>
[TemplatePart(PART_BadgeBorder, typeof(Border))]
public sealed class Badge : ContentControl
{
    /// <summary>
    /// 模板中徽标边框部件的名称。
    /// </summary>
    public const string PART_BadgeBorder = "PART_BadgeBorder";

    private bool _isBadgeVisible;
    private bool _isBadgeTextVisible;
    private string? _displayCount;
    private Thickness _badgeMargin;
    private Vector _badgeTransformOffset;
    private HorizontalAlignment _badgeHorizontalAlignment = HorizontalAlignment.Right;
    private VerticalAlignment _badgeVerticalAlignment = VerticalAlignment.Top;
    private Border? _badgeBorder;

    /// <inheritdoc cref="Count" />
    public static readonly StyledProperty<string?> CountProperty =
        AvaloniaProperty.Register<Badge, string?>(nameof(Count));

    /// <inheritdoc cref="OverflowCount" />
    public static readonly StyledProperty<int> OverflowCountProperty =
        AvaloniaProperty.Register<Badge, int>(nameof(OverflowCount), 99);

    /// <inheritdoc cref="Dot" />
    public static readonly StyledProperty<bool> DotProperty =
        AvaloniaProperty.Register<Badge, bool>(nameof(Dot));

    /// <inheritdoc cref="Shape" />
    public static readonly StyledProperty<BadgeShape> ShapeProperty =
        AvaloniaProperty.Register<Badge, BadgeShape>(nameof(Shape), BadgeShape.Circle);

    /// <inheritdoc cref="Placement" />
    public static readonly StyledProperty<BadgePlacement> PlacementProperty =
        AvaloniaProperty.Register<Badge, BadgePlacement>(nameof(Placement), BadgePlacement.TopRight);

    /// <inheritdoc cref="Offset" />
    public static readonly StyledProperty<Vector> OffsetProperty =
        AvaloniaProperty.Register<Badge, Vector>(nameof(Offset));

    /// <inheritdoc cref="IsBadgeVisible" />
    public static readonly DirectProperty<Badge, bool> IsBadgeVisibleProperty =
        AvaloniaProperty.RegisterDirect<Badge, bool>(
            nameof(IsBadgeVisible),
            o => o.IsBadgeVisible);

    /// <inheritdoc cref="IsBadgeTextVisible" />
    public static readonly DirectProperty<Badge, bool> IsBadgeTextVisibleProperty =
        AvaloniaProperty.RegisterDirect<Badge, bool>(
            nameof(IsBadgeTextVisible),
            o => o.IsBadgeTextVisible);

    /// <inheritdoc cref="DisplayCount" />
    public static readonly DirectProperty<Badge, string?> DisplayCountProperty =
        AvaloniaProperty.RegisterDirect<Badge, string?>(
            nameof(DisplayCount),
            o => o.DisplayCount);

    /// <inheritdoc cref="BadgeHorizontalAlignment" />
    public static readonly DirectProperty<Badge, HorizontalAlignment> BadgeHorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<Badge, HorizontalAlignment>(
            nameof(BadgeHorizontalAlignment),
            o => o.BadgeHorizontalAlignment);

    /// <inheritdoc cref="BadgeVerticalAlignment" />
    public static readonly DirectProperty<Badge, VerticalAlignment> BadgeVerticalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<Badge, VerticalAlignment>(
            nameof(BadgeVerticalAlignment),
            o => o.BadgeVerticalAlignment);

    /// <inheritdoc cref="BadgeMargin" />
    public static readonly DirectProperty<Badge, Thickness> BadgeMarginProperty =
        AvaloniaProperty.RegisterDirect<Badge, Thickness>(
            nameof(BadgeMargin),
            o => o.BadgeMargin);

    /// <inheritdoc cref="BadgeTransformOffset" />
    public static readonly DirectProperty<Badge, Vector> BadgeTransformOffsetProperty =
        AvaloniaProperty.RegisterDirect<Badge, Vector>(
            nameof(BadgeTransformOffset),
            o => o.BadgeTransformOffset);

    static Badge()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Badge>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<Badge>(HorizontalAlignment.Left);

        CountProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        OverflowCountProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        DotProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        ShapeProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        PlacementProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateState());
        OffsetProperty.Changed.AddClassHandler<Badge>((control, _) => control.UpdateBadgePosition());
    }

    /// <summary>
    /// 获取或设置徽标数量文本。
    /// </summary>
    public string? Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>
    /// 获取或设置数量溢出阈值；超过后显示为 “{OverflowCount}+”。
    /// </summary>
    public int OverflowCount
    {
        get => GetValue(OverflowCountProperty);
        set => SetValue(OverflowCountProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示点状徽标（dot）。
    /// </summary>
    public bool Dot
    {
        get => GetValue(DotProperty);
        set => SetValue(DotProperty, value);
    }

    /// <summary>
    /// 获取或设置徽标形状。
    /// </summary>
    public BadgeShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    /// <summary>
    /// 获取或设置徽标位置。
    /// </summary>
    public BadgePlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// 获取或设置徽标偏移量。
    /// </summary>
    public Vector Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    /// <summary>
    /// 获取徽标整体是否可见（由 <see cref="Dot"/> / <see cref="Count"/> 自动计算）。
    /// </summary>
    public bool IsBadgeVisible
    {
        get => _isBadgeVisible;
        private set => SetAndRaise(IsBadgeVisibleProperty, ref _isBadgeVisible, value);
    }

    /// <summary>
    /// 获取徽标文本是否可见（由 <see cref="Dot"/> / <see cref="Count"/> 自动计算）。
    /// </summary>
    public bool IsBadgeTextVisible
    {
        get => _isBadgeTextVisible;
        private set => SetAndRaise(IsBadgeTextVisibleProperty, ref _isBadgeTextVisible, value);
    }

    /// <summary>
    /// 获取最终展示的数量文本（包含溢出处理）。
    /// </summary>
    public string? DisplayCount
    {
        get => _displayCount;
        private set => SetAndRaise(DisplayCountProperty, ref _displayCount, value);
    }

    /// <summary>
    /// 获取徽标的水平对齐方式（由 <see cref="Placement"/> 自动计算）。
    /// </summary>
    public HorizontalAlignment BadgeHorizontalAlignment
    {
        get => _badgeHorizontalAlignment;
        private set => SetAndRaise(BadgeHorizontalAlignmentProperty, ref _badgeHorizontalAlignment, value);
    }

    /// <summary>
    /// 获取徽标的垂直对齐方式（由 <see cref="Placement"/> 自动计算）。
    /// </summary>
    public VerticalAlignment BadgeVerticalAlignment
    {
        get => _badgeVerticalAlignment;
        private set => SetAndRaise(BadgeVerticalAlignmentProperty, ref _badgeVerticalAlignment, value);
    }

    /// <summary>
    /// 获取徽标的外边距（可结合偏移使用）。
    /// </summary>
    public Thickness BadgeMargin
    {
        get => _badgeMargin;
        private set => SetAndRaise(BadgeMarginProperty, ref _badgeMargin, value);
    }

    /// <summary>
    /// 获取徽标的实际平移偏移量（由 <see cref="Placement"/>、徽标尺寸和 <see cref="Offset"/> 共同计算）。
    /// </summary>
    public Vector BadgeTransformOffset
    {
        get => _badgeTransformOffset;
        private set => SetAndRaise(BadgeTransformOffsetProperty, ref _badgeTransformOffset, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        if (_badgeBorder is not null)
        {
            _badgeBorder.SizeChanged -= OnBadgeBorderSizeChanged;
        }

        base.OnApplyTemplate(e);

        _badgeBorder = e.NameScope.Find<Border>(PART_BadgeBorder);
        if (_badgeBorder is not null)
        {
            _badgeBorder.SizeChanged += OnBadgeBorderSizeChanged;
        }

        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        if (_badgeBorder is not null)
        {
            _badgeBorder.SizeChanged -= OnBadgeBorderSizeChanged;
        }

        base.OnDetachedFromVisualTree(e);
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

        BadgeMargin = default;
        UpdateBadgePosition();
    }

    private void OnBadgeBorderSizeChanged(object? sender, SizeChangedEventArgs e)
    {
        UpdateBadgePosition();
    }

    private void UpdateBadgePosition()
    {
        var horizontal = BadgeHorizontalAlignment == HorizontalAlignment.Left ? -1d : 1d;
        var vertical = BadgeVerticalAlignment == VerticalAlignment.Top ? -1d : 1d;
        var bounds = _badgeBorder?.Bounds ?? default;

        BadgeTransformOffset = new Vector(
            Offset.X + (horizontal * bounds.Width / 2d),
            Offset.Y + (vertical * bounds.Height / 2d));
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
