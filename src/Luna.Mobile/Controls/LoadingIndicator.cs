using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Luna.Mobile.Controls;

/// <summary>
/// Loading 的绘制指示器控件（支持 Circular/Spinner/Dots）。
/// </summary>
public sealed class LoadingIndicator : Control
{
    private static readonly DispatcherTimer SharedTimer;
    private static readonly List<WeakReference<LoadingIndicator>> Instances = [];

    private long _startTimestamp;
    private double _progress;

    static LoadingIndicator()
    {
        SharedTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16),
        };
        SharedTimer.Tick += (_, _) =>
        {
            for (var i = Instances.Count - 1; i >= 0; i--)
            {
                if (!Instances[i].TryGetTarget(out var target))
                {
                    Instances.RemoveAt(i);
                    continue;
                }

                target.UpdateProgress();
                target.InvalidateVisual();
            }

            if (Instances.Count == 0)
            {
                SharedTimer.Stop();
            }
        };
    }

    /// <inheritdoc cref="Theme" />
    public static readonly new StyledProperty<LoadingTheme> ThemeProperty =
        AvaloniaProperty.Register<LoadingIndicator, LoadingTheme>(nameof(Theme), LoadingTheme.Circular);

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<LoadingIndicator, double>(nameof(Size), 20);

    /// <inheritdoc cref="Duration" />
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<LoadingIndicator, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(800));

    /// <inheritdoc cref="Foreground" />
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<LoadingIndicator, IBrush?>(nameof(Foreground));

    /// <summary>
    /// 获取或设置绘制主题。
    /// </summary>
    public new LoadingTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置绘制尺寸。
    /// </summary>
    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置单次动画周期。
    /// </summary>
    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>
    /// 获取或设置前景画刷。
    /// </summary>
    public IBrush? Foreground
    {
        get => GetValue(ForegroundProperty);
        set => SetValue(ForegroundProperty, value);
    }

    /// <summary>
    /// 初始化 <see cref="LoadingIndicator" /> 的新实例。
    /// </summary>
    public LoadingIndicator()
    {
        _startTimestamp = Stopwatch.GetTimestamp();
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _startTimestamp = Stopwatch.GetTimestamp();
        Instances.Add(new WeakReference<LoadingIndicator>(this));
        if (!SharedTimer.IsEnabled)
        {
            SharedTimer.Start();
        }
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        for (var i = Instances.Count - 1; i >= 0; i--)
        {
            if (Instances[i].TryGetTarget(out var target) && ReferenceEquals(target, this))
            {
                Instances.RemoveAt(i);
            }
        }
    }

    /// <inheritdoc />
    public override void Render(DrawingContext context)
    {
        var size = Size;
        if (size <= 0)
        {
            return;
        }

        var availableWidth = Bounds.Width > 0 ? Bounds.Width : size;
        var availableHeight = Bounds.Height > 0 ? Bounds.Height : size;
        var drawSize = Math.Min(size, Math.Min(availableWidth, availableHeight));
        var offsetX = (availableWidth - drawSize) / 2;
        var offsetY = (availableHeight - drawSize) / 2;
        var bounds = new Rect(offsetX, offsetY, drawSize, drawSize);
        var brush = Foreground ?? Brushes.Black;

        switch (Theme)
        {
            case LoadingTheme.Dots:
                RenderDots(context, bounds, brush);
                break;
            case LoadingTheme.Spinner:
                RenderSpinner(context, bounds, brush);
                break;
            default:
                RenderCircular(context, bounds, brush);
                break;
        }
    }

    /// <inheritdoc />
    protected override Size MeasureOverride(Size availableSize)
    {
        var s = Size;
        return new Size(s, s);
    }

    private void RenderCircular(DrawingContext context, Rect bounds, IBrush brush)
    {
        var diameter = Math.Min(bounds.Width, bounds.Height);
        var stroke = Math.Max(1.8, diameter * 0.085);
        var radius = Math.Max(0, (diameter - stroke) / 2);
        if (radius <= 0)
        {
            return;
        }

        var center = bounds.Center;
        var pen = new Pen(brush, stroke, lineCap: PenLineCap.Round);
        const int segmentCount = 24;
        var stepAngle = 360.0 / segmentCount;
        var sweepAngle = stepAngle * 0.72;
        var rotation = _progress * 360.0;

        for (var i = 0; i < segmentCount; i++)
        {
            var opacity = 0.16 + 0.84 * ((i + 1) / (double)segmentCount);
            var startAngle = -90 + rotation + i * stepAngle;
            using (context.PushOpacity(opacity))
            {
                DrawArc(context, center, radius, startAngle, sweepAngle, pen);
            }
        }
    }

    private void RenderSpinner(DrawingContext context, Rect bounds, IBrush brush)
    {
        var center = bounds.Center;
        var diameter = Math.Min(bounds.Width, bounds.Height);
        var radius = diameter / 2;
        var lineLength = radius * 0.34;
        var innerOffset = radius * 0.28;
        var thickness = Math.Max(1.4, diameter * 0.055);
        var count = 12;
        var head = (int)Math.Floor(_progress * count) % count;
        var pen = new Pen(brush, thickness, lineCap: PenLineCap.Round);

        for (var i = 0; i < count; i++)
        {
            var t = (i - head + count) % count;
            var opacity = 0.18 + 0.82 * ((count - t) / (double)count);
            var angle = i * (Math.PI * 2 / count);
            var p1 = new Point(center.X + innerOffset * Math.Cos(angle), center.Y + innerOffset * Math.Sin(angle));
            var p2 = new Point(center.X + (innerOffset + lineLength) * Math.Cos(angle), center.Y + (innerOffset + lineLength) * Math.Sin(angle));
            using (context.PushOpacity(opacity))
            {
                context.DrawLine(pen, p1, p2);
            }
        }
    }

    private void RenderDots(DrawingContext context, Rect bounds, IBrush brush)
    {
        var diameter = Math.Min(bounds.Width, bounds.Height);
        var dot = Math.Max(4, diameter * 0.16);
        var gap = dot * 0.55;
        var total = dot * 3 + gap * 2;
        var startX = (bounds.Width - total) / 2;
        var y = bounds.Height / 2;

        for (var i = 0; i < 3; i++)
        {
            var phase = NormalizeProgress(_progress - i * 0.18);
            var wave = 0.5 - 0.5 * Math.Cos(phase * Math.PI * 2);
            var scale = 0.72 + 0.28 * wave;
            var opacity = 0.28 + 0.72 * wave;
            var scaledDot = dot * scale;
            var x = startX + i * (dot + gap) + (dot - scaledDot) / 2;
            var rect = new Rect(x, y - scaledDot / 2, scaledDot, scaledDot);
            using (context.PushOpacity(opacity))
            {
                context.DrawEllipse(brush, null, rect.Center, scaledDot / 2, scaledDot / 2);
            }
        }
    }

    private static void DrawArc(DrawingContext context, Point center, double radius, double startAngle, double sweepAngle, Pen pen)
    {
        if (radius <= 0 || sweepAngle <= 0)
        {
            return;
        }

        var geometry = new StreamGeometry();
        using (var geometryContext = geometry.Open())
        {
            var start = PointOnCircle(center, radius, startAngle);
            var end = PointOnCircle(center, radius, startAngle + sweepAngle);
            geometryContext.BeginFigure(start, false);
            geometryContext.ArcTo(end, new Size(radius, radius), 0, sweepAngle > 180, SweepDirection.Clockwise);
            geometryContext.EndFigure(false);
        }

        context.DrawGeometry(null, pen, geometry);
    }

    private static Point PointOnCircle(Point center, double radius, double angleDegrees)
    {
        var radians = angleDegrees * Math.PI / 180.0;
        return new Point(
            center.X + radius * Math.Cos(radians),
            center.Y + radius * Math.Sin(radians));
    }

    private static double NormalizeProgress(double value)
    {
        value %= 1.0;
        return value < 0 ? value + 1.0 : value;
    }

    private void UpdateProgress()
    {
        var duration = Duration.TotalSeconds;
        if (duration <= 0)
        {
            duration = 0.8;
        }

        var elapsedSeconds = (Stopwatch.GetTimestamp() - _startTimestamp) / (double)Stopwatch.Frequency;
        _progress = (elapsedSeconds % duration) / duration;
    }
}
