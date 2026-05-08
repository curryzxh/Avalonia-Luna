using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

[PseudoClasses(":visible")]
public class BackTop : ContentControl
{
    public static readonly StyledProperty<double> VisibilityHeightProperty =
        AvaloniaProperty.Register<BackTop, double>(nameof(VisibilityHeight), 400);

    public static readonly StyledProperty<double> RightProperty =
        AvaloniaProperty.Register<BackTop, double>(nameof(Right), 24);

    public static readonly StyledProperty<double> BottomProperty =
        AvaloniaProperty.Register<BackTop, double>(nameof(Bottom), 40);

    public double VisibilityHeight
    {
        get => GetValue(VisibilityHeightProperty);
        set => SetValue(VisibilityHeightProperty, value);
    }

    public double Right
    {
        get => GetValue(RightProperty);
        set => SetValue(RightProperty, value);
    }

    public double Bottom
    {
        get => GetValue(BottomProperty);
        set => SetValue(BottomProperty, value);
    }

    private ScrollViewer? _scrollViewer;

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _scrollViewer = this.FindAncestorOfType<ScrollViewer>();
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged += OnScrollChanged;
            UpdateVisibility(_scrollViewer.Offset.Y);
        }
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (_scrollViewer != null)
        {
            _scrollViewer.ScrollChanged -= OnScrollChanged;
            _scrollViewer = null;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        if (_scrollViewer != null)
            UpdateVisibility(_scrollViewer.Offset.Y);
    }

    private void UpdateVisibility(double verticalOffset)
    {
        var visible = verticalOffset >= VisibilityHeight;
        PseudoClasses.Set(":visible", visible);
        IsVisible = visible;
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (_scrollViewer != null)
            _scrollViewer.Offset = new Vector(0, 0);
    }
}
