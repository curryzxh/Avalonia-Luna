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
        AvaloniaProperty.Register<LoadingIndicator, double>(nameof(Size), 22);

    /// <inheritdoc cref="Duration" />
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<LoadingIndicator, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(3000));

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

        var bounds = new Rect(0, 0, size, size);
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
        var stroke = Math.Max(2, bounds.Width * 0.1);
        var center = bounds.Center;
        var radius = (bounds.Width - stroke) / 2;
        var circumference = 2 * Math.PI * radius;
        var dash = new DashStyle([circumference * 0.75, circumference * 0.25], -_progress * circumference);
        var pen = new Pen(brush, stroke, lineCap: PenLineCap.Round, dashStyle: dash);
        context.DrawEllipse(null, pen, center, radius, radius);
    }

    private void RenderSpinner(DrawingContext context, Rect bounds, IBrush brush)
    {
        var center = bounds.Center;
        var radius = bounds.Width / 2;
        var lineLength = radius * 0.45;
        var thickness = Math.Max(2, bounds.Width * 0.08);
        var count = 12;
        var head = (int)Math.Floor(_progress * count) % count;
        var pen = new Pen(brush, thickness, lineCap: PenLineCap.Round);

        for (var i = 0; i < count; i++)
        {
            var t = (i - head + count) % count;
            var opacity = 1.0 - t / (double)count;
            var angle = i * (Math.PI * 2 / count);
            var p1 = new Point(center.X + (radius * 0.15) * Math.Cos(angle), center.Y + (radius * 0.15) * Math.Sin(angle));
            var p2 = new Point(center.X + (radius * 0.15 + lineLength) * Math.Cos(angle), center.Y + (radius * 0.15 + lineLength) * Math.Sin(angle));
            using (context.PushOpacity(opacity))
            {
                context.DrawLine(pen, p1, p2);
            }
        }
    }

    private void RenderDots(DrawingContext context, Rect bounds, IBrush brush)
    {
        var dot = bounds.Width * 0.18;
        var gap = bounds.Width * 0.12;
        var total = dot * 3 + gap * 2;
        var startX = (bounds.Width - total) / 2;
        var y = bounds.Height / 2;

        for (var i = 0; i < 3; i++)
        {
            var phase = (_progress + i * 0.15) % 1.0;
            var opacity = 0.25 + 0.75 * Math.Sin(phase * Math.PI);
            var rect = new Rect(startX + i * (dot + gap), y - dot / 2, dot, dot);
            using (context.PushOpacity(opacity))
            {
                context.DrawEllipse(brush, null, rect.Center, dot / 2, dot / 2);
            }
        }
    }

    private void UpdateProgress()
    {
        var duration = Duration.TotalSeconds;
        if (duration <= 0)
        {
            duration = 3;
        }

        var elapsedSeconds = (Stopwatch.GetTimestamp() - _startTimestamp) / (double)Stopwatch.Frequency;
        _progress = (elapsedSeconds % duration) / duration;
    }
}
