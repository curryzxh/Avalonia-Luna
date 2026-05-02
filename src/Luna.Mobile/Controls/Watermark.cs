using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Luna.Mobile.Controls;

/// <summary>
/// 水印布局模式。
/// </summary>
public enum WatermarkLayout
{
    /// <summary>
    /// 矩形网格。
    /// </summary>
    Rectangular,

    /// <summary>
    /// 六边形交错网格。
    /// </summary>
    Hexagonal,
}

/// <summary>
/// 水印单元内容。
/// </summary>
public sealed class WatermarkItem
{
    /// <summary>
    /// 获取或设置文本内容。
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// 获取或设置图片源，支持 <see cref="IImage"/>、<see cref="Uri"/> 或字符串路径。
    /// </summary>
    public object? Source { get; set; }

    /// <summary>
    /// 获取或设置文本前景。
    /// </summary>
    public IBrush? Foreground { get; set; }

    /// <summary>
    /// 获取或设置文本字号。
    /// </summary>
    public double FontSize { get; set; } = 14;

    /// <summary>
    /// 获取或设置是否按灰阶风格绘制。
    /// </summary>
    public bool IsGrayscale { get; set; }
}

/// <summary>
/// 水印控件，支持文本、图片和图文混排水印。
/// </summary>
[TemplatePart(SurfacePartName, typeof(WatermarkSurface))]
public sealed class Watermark : ContentControl
{
    private const string SurfacePartName = "PART_Surface";
    private readonly AvaloniaList<WatermarkItem> _items = [];
    private WatermarkSurface? _surface;

    /// <inheritdoc cref="Items" />
    public static readonly DirectProperty<Watermark, AvaloniaList<WatermarkItem>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<Watermark, AvaloniaList<WatermarkItem>>(
            nameof(Items),
            o => o.Items);

    /// <inheritdoc cref="TileWidth" />
    public static readonly StyledProperty<double> TileWidthProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(TileWidth), 120);

    /// <inheritdoc cref="TileHeight" />
    public static readonly StyledProperty<double> TileHeightProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(TileHeight), 52);

    /// <inheritdoc cref="HorizontalSpacing" />
    public static readonly StyledProperty<double> HorizontalSpacingProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(HorizontalSpacing), 56);

    /// <inheritdoc cref="VerticalSpacing" />
    public static readonly StyledProperty<double> VerticalSpacingProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(VerticalSpacing), 56);

    /// <inheritdoc cref="LineSpacing" />
    public static readonly StyledProperty<double> LineSpacingProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(LineSpacing), 12);

    /// <inheritdoc cref="Angle" />
    public static readonly StyledProperty<double> AngleProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(Angle), -22);

    /// <inheritdoc cref="Alpha" />
    public static readonly StyledProperty<double> AlphaProperty =
        AvaloniaProperty.Register<Watermark, double>(nameof(Alpha), 0.14);

    /// <inheritdoc cref="Layout" />
    public static readonly StyledProperty<WatermarkLayout> LayoutProperty =
        AvaloniaProperty.Register<Watermark, WatermarkLayout>(nameof(Layout), WatermarkLayout.Rectangular);

    /// <inheritdoc cref="Movable" />
    public static readonly StyledProperty<bool> MovableProperty =
        AvaloniaProperty.Register<Watermark, bool>(nameof(Movable));

    /// <inheritdoc cref="AnimationDuration" />
    public static readonly StyledProperty<TimeSpan> AnimationDurationProperty =
        AvaloniaProperty.Register<Watermark, TimeSpan>(nameof(AnimationDuration), TimeSpan.FromSeconds(6));

    static Watermark()
    {
        TileWidthProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        TileHeightProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        HorizontalSpacingProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        VerticalSpacingProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        LineSpacingProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        AngleProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        AlphaProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        LayoutProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        MovableProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
        AnimationDurationProperty.Changed.AddClassHandler<Watermark>((control, _) => control.InvalidateSurface());
    }

    /// <summary>
    /// 初始化 <see cref="Watermark"/> 的新实例。
    /// </summary>
    public Watermark()
    {
        _items.CollectionChanged += OnItemsChanged;
    }

    /// <summary>
    /// 获取水印项集合。
    /// </summary>
    public AvaloniaList<WatermarkItem> Items => _items;

    /// <summary>
    /// 获取或设置单个水印块宽度。
    /// </summary>
    public double TileWidth
    {
        get => GetValue(TileWidthProperty);
        set => SetValue(TileWidthProperty, value);
    }

    /// <summary>
    /// 获取或设置单个水印块高度。
    /// </summary>
    public double TileHeight
    {
        get => GetValue(TileHeightProperty);
        set => SetValue(TileHeightProperty, value);
    }

    /// <summary>
    /// 获取或设置横向间距。
    /// </summary>
    public double HorizontalSpacing
    {
        get => GetValue(HorizontalSpacingProperty);
        set => SetValue(HorizontalSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置纵向间距。
    /// </summary>
    public double VerticalSpacing
    {
        get => GetValue(VerticalSpacingProperty);
        set => SetValue(VerticalSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置多行项间距。
    /// </summary>
    public double LineSpacing
    {
        get => GetValue(LineSpacingProperty);
        set => SetValue(LineSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置旋转角度。
    /// </summary>
    public double Angle
    {
        get => GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    /// <summary>
    /// 获取或设置整体透明度。
    /// </summary>
    public double Alpha
    {
        get => GetValue(AlphaProperty);
        set => SetValue(AlphaProperty, value);
    }

    /// <summary>
    /// 获取或设置布局类型。
    /// </summary>
    public WatermarkLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用移动动画。
    /// </summary>
    public bool Movable
    {
        get => GetValue(MovableProperty);
        set => SetValue(MovableProperty, value);
    }

    /// <summary>
    /// 获取或设置移动动画周期。
    /// </summary>
    public TimeSpan AnimationDuration
    {
        get => GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _surface = e.NameScope.Find<WatermarkSurface>(SurfacePartName);
        InvalidateSurface();
    }

    private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        InvalidateSurface();
    }

    private void InvalidateSurface()
    {
        _surface?.InvalidateVisual();
    }
}

/// <summary>
/// Watermark 的自绘层。
/// </summary>
public sealed class WatermarkSurface : Control
{
    private static readonly DispatcherTimer SharedTimer;
    private static readonly List<WeakReference<WatermarkSurface>> AnimatedInstances = [];
    private readonly Dictionary<string, IImage?> _imageCache = new(StringComparer.Ordinal);
    private long _startTimestamp;
    private double _progress;
    private INotifyCollectionChanged? _itemsNotifier;

    /// <inheritdoc cref="Items" />
    public static readonly StyledProperty<IReadOnlyList<WatermarkItem>?> ItemsProperty =
        AvaloniaProperty.Register<WatermarkSurface, IReadOnlyList<WatermarkItem>?>(nameof(Items));

    /// <inheritdoc cref="TileWidth" />
    public static readonly StyledProperty<double> TileWidthProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(TileWidth), 120);

    /// <inheritdoc cref="TileHeight" />
    public static readonly StyledProperty<double> TileHeightProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(TileHeight), 52);

    /// <inheritdoc cref="HorizontalSpacing" />
    public static readonly StyledProperty<double> HorizontalSpacingProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(HorizontalSpacing), 56);

    /// <inheritdoc cref="VerticalSpacing" />
    public static readonly StyledProperty<double> VerticalSpacingProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(VerticalSpacing), 56);

    /// <inheritdoc cref="LineSpacing" />
    public static readonly StyledProperty<double> LineSpacingProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(LineSpacing), 12);

    /// <inheritdoc cref="Angle" />
    public static readonly StyledProperty<double> AngleProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(Angle), -22);

    /// <inheritdoc cref="Alpha" />
    public static readonly StyledProperty<double> AlphaProperty =
        AvaloniaProperty.Register<WatermarkSurface, double>(nameof(Alpha), 0.14);

    /// <inheritdoc cref="Layout" />
    public static readonly StyledProperty<WatermarkLayout> LayoutProperty =
        AvaloniaProperty.Register<WatermarkSurface, WatermarkLayout>(nameof(Layout), WatermarkLayout.Rectangular);

    /// <inheritdoc cref="Movable" />
    public static readonly StyledProperty<bool> MovableProperty =
        AvaloniaProperty.Register<WatermarkSurface, bool>(nameof(Movable));

    /// <inheritdoc cref="AnimationDuration" />
    public static readonly StyledProperty<TimeSpan> AnimationDurationProperty =
        AvaloniaProperty.Register<WatermarkSurface, TimeSpan>(nameof(AnimationDuration), TimeSpan.FromSeconds(6));

    static WatermarkSurface()
    {
        SharedTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16),
        };
        SharedTimer.Tick += (_, _) =>
        {
            for (var i = AnimatedInstances.Count - 1; i >= 0; i--)
            {
                if (!AnimatedInstances[i].TryGetTarget(out var surface))
                {
                    AnimatedInstances.RemoveAt(i);
                    continue;
                }

                if (!surface.Movable || !surface.IsAttachedToVisualTree())
                {
                    continue;
                }

                surface.UpdateProgress();
                surface.InvalidateVisual();
            }

            if (AnimatedInstances.Count == 0)
            {
                SharedTimer.Stop();
            }
        };

        ItemsProperty.Changed.AddClassHandler<WatermarkSurface>((control, args) => control.HandleItemsChanged(args));
        TileWidthProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        TileHeightProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        HorizontalSpacingProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        VerticalSpacingProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        LineSpacingProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        AngleProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        AlphaProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        LayoutProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
        MovableProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.UpdateTimerRegistration());
        AnimationDurationProperty.Changed.AddClassHandler<WatermarkSurface>((control, _) => control.InvalidateVisual());
    }

    /// <summary>
    /// 获取或设置水印项集合。
    /// </summary>
    public IReadOnlyList<WatermarkItem>? Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    /// <summary>
    /// 获取或设置单个水印块宽度。
    /// </summary>
    public double TileWidth
    {
        get => GetValue(TileWidthProperty);
        set => SetValue(TileWidthProperty, value);
    }

    /// <summary>
    /// 获取或设置单个水印块高度。
    /// </summary>
    public double TileHeight
    {
        get => GetValue(TileHeightProperty);
        set => SetValue(TileHeightProperty, value);
    }

    /// <summary>
    /// 获取或设置横向间距。
    /// </summary>
    public double HorizontalSpacing
    {
        get => GetValue(HorizontalSpacingProperty);
        set => SetValue(HorizontalSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置纵向间距。
    /// </summary>
    public double VerticalSpacing
    {
        get => GetValue(VerticalSpacingProperty);
        set => SetValue(VerticalSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置多行项间距。
    /// </summary>
    public double LineSpacing
    {
        get => GetValue(LineSpacingProperty);
        set => SetValue(LineSpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置旋转角度。
    /// </summary>
    public double Angle
    {
        get => GetValue(AngleProperty);
        set => SetValue(AngleProperty, value);
    }

    /// <summary>
    /// 获取或设置整体透明度。
    /// </summary>
    public double Alpha
    {
        get => GetValue(AlphaProperty);
        set => SetValue(AlphaProperty, value);
    }

    /// <summary>
    /// 获取或设置布局类型。
    /// </summary>
    public WatermarkLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用移动动画。
    /// </summary>
    public bool Movable
    {
        get => GetValue(MovableProperty);
        set => SetValue(MovableProperty, value);
    }

    /// <summary>
    /// 获取或设置移动动画周期。
    /// </summary>
    public TimeSpan AnimationDuration
    {
        get => GetValue(AnimationDurationProperty);
        set => SetValue(AnimationDurationProperty, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _startTimestamp = DateTime.UtcNow.Ticks;
        UpdateTimerRegistration();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        UnregisterAnimatedInstance();
        DetachItemsNotifier();
    }

    /// <inheritdoc />
    public override void Render(DrawingContext context)
    {
        base.Render(context);

        if (Items is null || Items.Count == 0 || Bounds.Width <= 0 || Bounds.Height <= 0)
        {
            return;
        }

        var tileWidth = Math.Max(1, TileWidth);
        var tileHeight = Math.Max(1, TileHeight);
        var stepX = tileWidth + Math.Max(0, HorizontalSpacing);
        var stepY = tileHeight + Math.Max(0, VerticalSpacing);
        var shiftX = Movable ? stepX * _progress : 0;
        var shiftY = Movable ? stepY * _progress : 0;

        using (context.PushClip(new Rect(Bounds.Size)))
        {
            for (var row = -1; row * stepY - shiftY < Bounds.Height + stepY; row++)
            {
                var rowOffsetX = Layout == WatermarkLayout.Hexagonal && row % 2 != 0 ? stepX / 2 : 0;
                for (var column = -1; column * stepX - shiftX < Bounds.Width + stepX; column++)
                {
                    var x = column * stepX + rowOffsetX - shiftX;
                    var y = row * stepY - shiftY;
                    RenderTile(context, new Point(x, y), tileWidth, tileHeight);
                }
            }
        }
    }

    private void RenderTile(DrawingContext context, Point origin, double tileWidth, double tileHeight)
    {
        var items = Items;
        if (items is null || items.Count == 0)
        {
            return;
        }

        var metrics = MeasureItems(items, tileWidth, tileHeight);
        if (metrics.Count == 0)
        {
            return;
        }

        var totalHeight = 0d;
        for (var i = 0; i < metrics.Count; i++)
        {
            totalHeight += metrics[i].Height;
            if (i < metrics.Count - 1)
            {
                totalHeight += Math.Max(0, LineSpacing);
            }
        }

        var baseX = origin.X + tileWidth / 2;
        var baseY = origin.Y + Math.Max(0, (tileHeight - totalHeight) / 2);
        var radians = Angle * Math.PI / 180d;

        using (context.PushTransform(Matrix.CreateTranslation(baseX, baseY) * Matrix.CreateRotation(radians)))
        {
            var currentY = 0d;
            foreach (var metric in metrics)
            {
                var opacity = Math.Clamp(Alpha, 0, 1) * (metric.Item.IsGrayscale ? 0.8 : 1);
                using (context.PushOpacity(opacity))
                {
                    if (metric.Image is not null)
                    {
                        var imageX = -metric.Width / 2;
                        var sourceRect = new Rect(metric.Image.Size);
                        var destRect = new Rect(imageX, currentY, metric.Width, metric.Height);
                        context.DrawImage(metric.Image, sourceRect, destRect);
                    }
                    else if (metric.TextLayout is not null)
                    {
                        var textX = -metric.Width / 2;
                        context.DrawText(metric.TextLayout, new Point(textX, currentY));
                    }
                }

                currentY += metric.Height + Math.Max(0, LineSpacing);
            }
        }
    }

    private List<WatermarkMeasure> MeasureItems(IReadOnlyList<WatermarkItem> items, double tileWidth, double tileHeight)
    {
        var result = new List<WatermarkMeasure>(items.Count);
        var maxItemHeight = items.Count == 1
            ? tileHeight
            : Math.Max(1, (tileHeight - Math.Max(0, LineSpacing) * (items.Count - 1)) / items.Count);

        foreach (var item in items)
        {
            if (!string.IsNullOrWhiteSpace(item.Text))
            {
                var brush = item.Foreground ?? new SolidColorBrush(item.IsGrayscale ? Colors.Gray : Color.FromRgb(0x78, 0x78, 0x78));
                var typeface = new Typeface("Inter");
                var textLayout = new FormattedText(
                    item.Text,
                    CultureInfo.CurrentCulture,
                    FlowDirection.LeftToRight,
                    typeface,
                    item.FontSize > 0 ? item.FontSize : 14,
                    brush);

                result.Add(new WatermarkMeasure(item, textLayout, textLayout.Width, textLayout.Height));
                continue;
            }

            var image = ResolveImage(item.Source);
            if (image is not null)
            {
                var size = image.Size;
                var scale = Math.Min(tileWidth / Math.Max(1, size.Width), maxItemHeight / Math.Max(1, size.Height));
                scale = double.IsFinite(scale) && scale > 0 ? scale : 1;
                result.Add(new WatermarkMeasure(item, image, size.Width * scale, size.Height * scale));
            }
        }

        return result;
    }

    private IImage? ResolveImage(object? source)
    {
        switch (source)
        {
            case null:
                return null;
            case IImage image:
                return image;
            case Uri uri:
                return ResolveBitmap(uri.AbsoluteUri);
            case string path when !string.IsNullOrWhiteSpace(path):
                return ResolveBitmap(path);
            default:
                return null;
        }
    }

    private IImage? ResolveBitmap(string path)
    {
        if (_imageCache.TryGetValue(path, out var cached))
        {
            return cached;
        }

        try
        {
            cached = new Bitmap(path);
        }
        catch
        {
            cached = null;
        }

        _imageCache[path] = cached;
        return cached;
    }

    private void HandleItemsChanged(AvaloniaPropertyChangedEventArgs args)
    {
        DetachItemsNotifier();
        _itemsNotifier = args.GetNewValue<IReadOnlyList<WatermarkItem>?>() as INotifyCollectionChanged;
        if (_itemsNotifier is not null)
        {
            _itemsNotifier.CollectionChanged += OnItemsCollectionChanged;
        }

        InvalidateVisual();
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        InvalidateVisual();
    }

    private void DetachItemsNotifier()
    {
        if (_itemsNotifier is not null)
        {
            _itemsNotifier.CollectionChanged -= OnItemsCollectionChanged;
            _itemsNotifier = null;
        }
    }

    private void UpdateTimerRegistration()
    {
        UnregisterAnimatedInstance();

        if (!Movable || VisualRoot is null)
        {
            InvalidateVisual();
            return;
        }

        _startTimestamp = DateTime.UtcNow.Ticks;
        AnimatedInstances.Add(new WeakReference<WatermarkSurface>(this));
        if (!SharedTimer.IsEnabled)
        {
            SharedTimer.Start();
        }
    }

    private void UnregisterAnimatedInstance()
    {
        for (var i = AnimatedInstances.Count - 1; i >= 0; i--)
        {
            if (!AnimatedInstances[i].TryGetTarget(out var target) || ReferenceEquals(target, this))
            {
                AnimatedInstances.RemoveAt(i);
            }
        }
    }

    private void UpdateProgress()
    {
        var duration = AnimationDuration.TotalSeconds;
        if (duration <= 0)
        {
            duration = 6;
        }

        var elapsed = (DateTime.UtcNow.Ticks - _startTimestamp) / (double)TimeSpan.TicksPerSecond;
        _progress = (elapsed % duration) / duration;
    }

    private sealed class WatermarkMeasure
    {
        public WatermarkMeasure(WatermarkItem item, FormattedText textLayout, double width, double height)
        {
            Item = item;
            TextLayout = textLayout;
            Width = width;
            Height = height;
        }

        public WatermarkMeasure(WatermarkItem item, IImage image, double width, double height)
        {
            Item = item;
            Image = image;
            Width = width;
            Height = height;
        }

        public WatermarkItem Item { get; }

        public FormattedText? TextLayout { get; }

        public IImage? Image { get; }

        public double Width { get; }

        public double Height { get; }
    }
}
