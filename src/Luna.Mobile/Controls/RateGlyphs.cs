using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 用于渲染评分图标的绘制控件（支持部分填充）。
/// </summary>
public sealed class RateGlyphs : Control
{
    private static readonly Geometry StarGeometry = Geometry.Parse("M12 17.27L18.18 21 16.54 13.97 22 9.24 14.81 8.63 12 2 9.19 8.63 2 9.24 7.46 13.97 5.82 21z");

    /// <inheritdoc cref="Count" />
    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<RateGlyphs, int>(nameof(Count), 5);

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<RateGlyphs, double>(nameof(Value));

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<RateGlyphs, double>(nameof(Size), 24);

    /// <inheritdoc cref="Spacing" />
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<RateGlyphs, double>(nameof(Spacing), 8);

    /// <inheritdoc cref="ActiveBrush" />
    public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
        AvaloniaProperty.Register<RateGlyphs, IBrush?>(nameof(ActiveBrush));

    /// <inheritdoc cref="InactiveBrush" />
    public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
        AvaloniaProperty.Register<RateGlyphs, IBrush?>(nameof(InactiveBrush));

    /// <inheritdoc cref="IconGeometry" />
    public static readonly StyledProperty<Geometry?> IconGeometryProperty =
        AvaloniaProperty.Register<RateGlyphs, Geometry?>(nameof(IconGeometry), StarGeometry);

    static RateGlyphs()
    {
        CountProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateMeasure());
        SizeProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateMeasure());
        SpacingProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateMeasure());
        ValueProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateVisual());
        ActiveBrushProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateVisual());
        InactiveBrushProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateVisual());
        IconGeometryProperty.Changed.AddClassHandler<RateGlyphs>((control, _) => control.InvalidateVisual());
    }

    /// <summary>
    /// 获取或设置图标总数。
    /// </summary>
    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>
    /// 获取或设置当前填充值。
    /// </summary>
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 获取或设置单个图标尺寸。
    /// </summary>
    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置图标间距。
    /// </summary>
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置激活状态画刷。
    /// </summary>
    public IBrush? ActiveBrush
    {
        get => GetValue(ActiveBrushProperty);
        set => SetValue(ActiveBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置未激活状态画刷。
    /// </summary>
    public IBrush? InactiveBrush
    {
        get => GetValue(InactiveBrushProperty);
        set => SetValue(InactiveBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置自定义图标几何。
    /// </summary>
    public Geometry? IconGeometry
    {
        get => GetValue(IconGeometryProperty);
        set => SetValue(IconGeometryProperty, value);
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        var count = Math.Max(0, Count);
        var size = Math.Max(0, Size);
        var spacing = Math.Max(0, Spacing);
        var width = count == 0 ? 0 : count * size + (count - 1) * spacing;
        return new Size(width, size);
    }

    /// <inheritdoc />
    public override void Render(DrawingContext context)
    {
        var count = Math.Max(0, Count);
        if (count == 0)
        {
            return;
        }

        var size = Math.Max(0, Size);
        if (size <= 0)
        {
            return;
        }

        var spacing = Math.Max(0, Spacing);
        var inactive = InactiveBrush ?? Brushes.Gray;
        var active = ActiveBrush ?? Brushes.Gold;
        var geometry = IconGeometry ?? StarGeometry;

        var scale = size / 24.0;
        var v = Math.Clamp(Value, 0, count);

        for (var i = 0; i < count; i++)
        {
            var x = i * (size + spacing);
            using (context.PushTransform(Matrix.CreateTranslation(x, 0) * Matrix.CreateScale(scale, scale)))
            {
                context.DrawGeometry(inactive, null, geometry);

                var fill = Math.Clamp(v - i, 0, 1);
                if (fill > 0)
                {
                    using (context.PushClip(new Rect(0, 0, 24 * fill, 24)))
                    {
                        context.DrawGeometry(active, null, geometry);
                    }
                }
            }
        }
    }
}
