using Avalonia;
using Avalonia.Controls;

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
}
