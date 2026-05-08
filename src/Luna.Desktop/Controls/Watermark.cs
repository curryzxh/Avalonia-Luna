using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Media;

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

    private WatermarkOverlay? _overlay;

    static Watermark()
    {
        WatermarkTextProperty.Changed.AddClassHandler<Watermark>((x, _) => x.InvalidateWatermark());
        WatermarkFontSizeProperty.Changed.AddClassHandler<Watermark>((x, _) => x.InvalidateWatermark());
        WatermarkOpacityProperty.Changed.AddClassHandler<Watermark>((x, _) => x.InvalidateWatermark());
        RotateProperty.Changed.AddClassHandler<Watermark>((x, _) => x.InvalidateWatermark());
        GapProperty.Changed.AddClassHandler<Watermark>((x, _) => x.InvalidateWatermark());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _overlay = e.NameScope.Find<WatermarkOverlay>("PART_WatermarkOverlay");
        SyncOverlay();
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        var result = base.ArrangeOverride(finalSize);
        SyncOverlay();
        return result;
    }

    private void InvalidateWatermark()
    {
        if (_overlay != null)
        {
            SyncOverlay();
            _overlay.InvalidateVisual();
        }
    }

    private void SyncOverlay()
    {
        if (_overlay == null) return;

        _overlay.WatermarkText = WatermarkText;
        _overlay.WatermarkFontSize = WatermarkFontSize;
        _overlay.Opacity = WatermarkOpacity;
        _overlay.Rotation = Rotate;
        _overlay.Gap = Gap;
    }
}

public class WatermarkOverlay : Control
{
    public static readonly StyledProperty<string> WatermarkTextProperty =
        AvaloniaProperty.Register<WatermarkOverlay, string>(nameof(WatermarkText), "Luna");

    public static readonly StyledProperty<double> WatermarkFontSizeProperty =
        AvaloniaProperty.Register<WatermarkOverlay, double>(nameof(WatermarkFontSize), 14);

    public static readonly StyledProperty<double> RotationProperty =
        AvaloniaProperty.Register<WatermarkOverlay, double>(nameof(Rotation), -22);

    public static readonly StyledProperty<double> GapProperty =
        AvaloniaProperty.Register<WatermarkOverlay, double>(nameof(Gap), 120);

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

    public double Rotation
    {
        get => GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    public double Gap
    {
        get => GetValue(GapProperty);
        set => SetValue(GapProperty, value);
    }

    static WatermarkOverlay()
    {
        AffectsRender<WatermarkOverlay>(
            WatermarkTextProperty,
            WatermarkFontSizeProperty,
            RotationProperty,
            GapProperty);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);

        var text = WatermarkText;
        if (string.IsNullOrEmpty(text))
            return;

        var bounds = Bounds;
        if (bounds.Width <= 0 || bounds.Height <= 0)
            return;

        var fontSize = WatermarkFontSize;
        var gap = Math.Max(Gap, 1);
        var rotation = Rotation;
        var radians = rotation * Math.PI / 180.0;

        var brush = (IBrush?)this.FindResource("Luna.Brush.Text.Primary") ?? Brushes.Black;

        var typeface = new Typeface(FontFamily, FontStyle, FontWeight);

        var lines = text.Split('\n');
        var lineHeight = fontSize * 1.4;
        var totalTextHeight = lineHeight * lines.Length;

        var textWidth = 0.0;
        var formattedLines = new FormattedText[lines.Length];
        for (var i = 0; i < lines.Length; i++)
        {
            var ft = new FormattedText(
                lines[i],
                System.Globalization.CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                typeface,
                fontSize,
                brush);
            formattedLines[i] = ft;
            textWidth = Math.Max(textWidth, ft.Bounds.Width);
        }

        var cellWidth = textWidth + gap;
        var cellHeight = totalTextHeight + gap;

        var diagonal = Math.Sqrt(bounds.Width * bounds.Width + bounds.Height * bounds.Height) * 1.5;
        var halfDiag = diagonal / 2;
        var cx = bounds.Width / 2;
        var cy = bounds.Height / 2;

        var cos = Math.Cos(radians);
        var sin = Math.Sin(radians);

        var startX = cx - halfDiag;
        var startY = cy - halfDiag;
        var endX = cx + halfDiag;
        var endY = cy + halfDiag;

        var stepX = cellWidth;
        var stepY = cellHeight;

        var offset = (startY - endX) / 2;

        using var push = context.PushTransform(Matrix.CreateRotation(radians));

        for (var y = startY; y < endY; y += stepY)
        {
            var rowOffset = (((y - startY) / stepY) % 2 == 1) ? stepX / 2 : 0;

            for (var x = startX - rowOffset; x < endX; x += stepX)
            {
                for (var li = 0; li < formattedLines.Length; li++)
                {
                    var lineY = y + li * lineHeight;
                    context.DrawText(formattedLines[li], new Point(x, lineY));
                }
            }
        }
    }
}
