using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

[PseudoClasses(":collapsed")]
public class StickyTool : ContentControl
{
    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<StickyTool, double>(nameof(Offset), 100);

    public static readonly StyledProperty<ControlsPosition> PositionProperty =
        AvaloniaProperty.Register<StickyTool, ControlsPosition>(nameof(Position), ControlsPosition.Right);

    public static readonly StyledProperty<bool> IsCollapsedProperty =
        AvaloniaProperty.Register<StickyTool, bool>(nameof(IsCollapsed), false);

    private ScrollViewer? _scrollContainer;
    private double _lastScrollOffset;
    private bool _isScrollingDown;

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public ControlsPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }

    public bool IsCollapsed
    {
        get => GetValue(IsCollapsedProperty);
        set => SetValue(IsCollapsedProperty, value);
    }

    static StickyTool()
    {
        IsCollapsedProperty.Changed.AddClassHandler<StickyTool>(OnIsCollapsedChanged);
        PositionProperty.Changed.AddClassHandler<StickyTool>(OnPositionChanged);
    }

    private static void OnIsCollapsedChanged(StickyTool sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":collapsed", sender.IsCollapsed);
    }

    private static void OnPositionChanged(StickyTool sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":left", sender.Position == ControlsPosition.Left);
        sender.PseudoClasses.Set(":right", sender.Position == ControlsPosition.Right);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _scrollContainer = this.FindAncestorOfType<ScrollViewer>();
        if (_scrollContainer != null)
        {
            _scrollContainer.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        }

        PseudoClasses.Set(":right", Position == ControlsPosition.Right);
        PseudoClasses.Set(":left", Position == ControlsPosition.Left);
        PseudoClasses.Set(":collapsed", IsCollapsed);
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_scrollContainer != null)
        {
            _scrollContainer.RemoveHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
            _scrollContainer = null;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollContainer == null) return;

        var currentOffset = _scrollContainer.Offset.Y;
        _isScrollingDown = currentOffset > _lastScrollOffset;
        _lastScrollOffset = currentOffset;

        if (currentOffset > Offset)
        {
            if (!IsCollapsed && _isScrollingDown)
            {
                IsCollapsed = true;
            }
        }
        else
        {
            IsCollapsed = false;
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (IsCollapsed)
        {
            IsCollapsed = false;
        }
    }
}

public enum ControlsPosition
{
    Right,
    Left,
}
