using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Shapes;
using Avalonia.Controls.Templates;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

public class Watermark : ContentControl
{
    public static readonly StyledProperty<string> WatermarkTextProperty =
        AvaloniaProperty.Register<Watermark, string>(nameof(WatermarkText), "Luna");

    public static readonly StyledProperty<double> WatermarkFontSizeProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(WatermarkFontSize), 14);

    public static readonly StyledProperty<double> WatermarkOpacityProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(WatermarkOpacity), 0.12);

    public static readonly StyledProperty<double> RotateProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(Rotate), -22);

    public static readonly StyledProperty<double> GapProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(Gap), 120);

    public static readonly StyledProperty<double> LineSpacingProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(LineSpacing), 80);

    public static readonly StyledProperty<bool> IsScrollingProperty =
        AvaloniaProperty.Register<Watermark, bool>(nameof(IsScrolling), false);

    private WatermarkOverlay? _overlay;

    public string WatermarkText
    {
        get => GetValue(WatermarkTextProperty);
        set => SetValue(WatermarkTextProperty, value);
    }

    public double WatermarkFontSize
    {
        get => GetValue(WatermarkFontSizeProperty);
        set => SetValue(WatermarkFontSizeProperty, value);
    }

    public double WatermarkOpacity
    {
        get => GetValue(WatermarkOpacityProperty);
        set => SetValue(WatermarkOpacityProperty, value);
    }

    public double Rotate
    {
        get => GetValue(RotateProperty);
        set => SetValue(RotateProperty, value);
    }

    public double Gap
    {
        get => GetValue(GapProperty);
        set => SetValue(GapProperty, value);
    }

    public double LineSpacing
    {
        get => GetValue(LineSpacingProperty);
        set => SetValue(LineSpacingProperty, value);
    }

    public bool IsScrolling
    {
        get => GetValue(IsScrollingProperty);
        set => SetValue(IsScrollingProperty, value);
    }

    static Watermark()
    {
        WatermarkTextProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        WatermarkFontSizeProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        WatermarkOpacityProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        RotateProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        GapProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        LineSpacingProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
        IsScrollingProperty.Changed.AddClassHandler<Watermark>(OnRenderPropertyChanged);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _overlay = this.FindDescendantOfType<WatermarkOverlay>();
        if (_overlay != null)
        {
            _overlay.Watermark = this;
            UpdateOverlay();
        }
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var result = base.MeasureOverride(availableSize);
        UpdateOverlay();
        return result;
    }

    private static void OnRenderPropertyChanged(Watermark sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdateOverlay();
    }

    private void UpdateOverlay()
    {
        _overlay?.InvalidateVisual();
    }

    internal void DrawWatermark(DrawingContext context, Size size)
    {
        if (string.IsNullOrEmpty(WatermarkText) || size.Width <= 0 || size.Height <= 0)
            return;

        var text = WatermarkText;
        var fontSize = WatermarkFontSize;
        var opacity = WatermarkOpacity;
        var rotate = Rotate;
        var gap = Gap;
        var lineSpacing = LineSpacing;

        var brush = new SolidColorBrush(Colors.Black, opacity);
        var typeface = new Typeface(FontFamily, FontStyle.Normal, FontWeight.Normal);

        var formattedText = new FormattedText(
            text,
            System.Globalization.CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            fontSize,
            brush);

        var textWidth = formattedText.Width;
        var textHeight = formattedText.Height;

        var totalWidth = textWidth + gap;
        var totalHeight = textHeight + lineSpacing;

        var diagonal = Math.Sqrt(size.Width * size.Width + size.Height * size.Height);
        var centerX = size.Width / 2;
        var centerY = size.Height / 2;

        var rotateRad = rotate * Math.PI / 180;

        var rows = (int)(diagonal / totalHeight) + 4;
        var cols = (int)(diagonal / totalWidth) + 4;

        var startX = centerX - (cols * totalWidth) / 2;
        var startY = centerY - (rows * totalHeight) / 2;

        using (context.PushTransform(
            Matrix.CreateTranslation(centerX, centerY) *
            Matrix.CreateRotation(rotateRad) *
            Matrix.CreateTranslation(-centerX, -centerY)))
        {
            for (var row = 0; row < rows; row++)
            {
                var offset = (row % 2 == 1) ? totalWidth / 2 : 0;
                for (var col = 0; col < cols; col++)
                {
                    var x = startX + col * totalWidth + offset;
                    var y = startY + row * totalHeight;

                    if (IsScrolling)
                    {
                        var scrollOffset = (row % 2 == 0) ? gap * 0.3 : -gap * 0.3;
                        x += scrollOffset;
                    }

                    context.DrawText(
                        new FormattedText(
                            text,
                            System.Globalization.CultureInfo.CurrentCulture,
                            FlowDirection.LeftToRight,
                            typeface,
                            fontSize,
                            brush),
                        new Point(x, y));
                }
            }
        }
    }
}

public class WatermarkOverlay : Control
{
    internal Watermark? Watermark { get; set; }

    public sealed override void Render(DrawingContext context)
    {
        base.Render(context);
        if (Watermark == null) return;

        var size = Bounds.Size;
        if (size.Width <= 0 || size.Height <= 0) return;

        Watermark.DrawWatermark(context, size);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        return new Size(availableSize.Width, availableSize.Height);
    }
}
