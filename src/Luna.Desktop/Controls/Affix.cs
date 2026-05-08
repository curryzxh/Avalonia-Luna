using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

[PseudoClasses(":fixed")]
public class Affix : ContentControl
{
    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<Affix, double>(nameof(Offset), 0);

    public static readonly StyledProperty<ScrollViewer?> ContainerProperty =
        AvaloniaProperty.Register<Affix, ScrollViewer?>(nameof(Container));

    private ScrollViewer? _scrollContainer;
    private Border? _placeholder;
    private Border? _contentBorder;
    private double _originalTop;
    private bool _isFixed;

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public ScrollViewer? Container
    {
        get => GetValue(ContainerProperty);
        set => SetValue(ContainerProperty, value);
    }

    static Affix()
    {
        ContainerProperty.Changed.AddClassHandler<Affix>(OnContainerChanged);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _placeholder = this.FindDescendantOfType<Border>();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        AttachScrollContainer();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        DetachScrollContainer();
    }

    private static void OnContainerChanged(Affix sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.DetachScrollContainer();
        sender.AttachScrollContainer();
    }

    private void AttachScrollContainer()
    {
        _scrollContainer = Container ?? this.FindAncestorOfType<ScrollViewer>();
        if (_scrollContainer != null)
        {
            _scrollContainer.AddHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
        }
    }

    private void DetachScrollContainer()
    {
        if (_scrollContainer != null)
        {
            _scrollContainer.RemoveHandler(ScrollViewer.ScrollChangedEvent, OnScrollChanged);
            _scrollContainer = null;
        }
    }

    private void OnScrollChanged(object? sender, ScrollChangedEventArgs e)
    {
        UpdateFixedState();
    }

    private void UpdateFixedState()
    {
        if (_scrollContainer == null || _placeholder == null) return;

        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var point = _placeholder.TranslatePoint(new Point(0, 0), topLevel);
        if (!point.HasValue) return;

        var shouldFix = point.Value.Y <= Offset;

        if (shouldFix != _isFixed)
        {
            _isFixed = shouldFix;
            PseudoClasses.Set(":fixed", _isFixed);

            if (_isFixed)
            {
                _placeholder.Height = _placeholder.Bounds.Height;
            }
            else
            {
                _placeholder.Height = double.NaN;
            }
        }

        if (_isFixed)
        {
            _contentBorder?.SetValue(Canvas.TopProperty, Offset);
        }
    }
}
