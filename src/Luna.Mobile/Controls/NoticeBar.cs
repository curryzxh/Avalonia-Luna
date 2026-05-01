using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Luna.Mobile.Controls;

/// <summary>
/// 通知栏主题类型。
/// </summary>
public enum NoticeBarTheme
{
    /// <summary>
    /// Info。
    /// </summary>
    Info,
    /// <summary>
    /// Success。
    /// </summary>
    Success,
    /// <summary>
    /// Warning。
    /// </summary>
    Warning,
    /// <summary>
    /// Error。
    /// </summary>
    Error,
}

/// <summary>
/// 通知栏滚动方向。
/// </summary>
public enum NoticeBarDirection
{
    /// <summary>
    /// Horizontal。
    /// </summary>
    Horizontal,
    /// <summary>
    /// Vertical。
    /// </summary>
    Vertical,
}

/// <summary>
/// 通知栏控件，用于展示提示信息，支持跑马灯/竖向轮播，以及关闭与操作区域。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_ContentViewport：<see cref="Border"/></description></item>
/// <item><description>PART_ContentText：<see cref="TextBlock"/></description></item>
/// <item><description>PART_CloseButton：<see cref="Button"/></description></item>
/// <item><description>PART_SuffixButton：<see cref="Button"/></description></item>
/// </list>
/// </remarks>
[TemplatePart(ContentViewportPartName, typeof(Border))]
[TemplatePart(ContentTextPartName, typeof(TextBlock))]
[TemplatePart(CloseButtonPartName, typeof(Button))]
[TemplatePart(SuffixButtonPartName, typeof(Button))]
public sealed class NoticeBar : TemplatedControl
{
    private const string ContentViewportPartName = "PART_ContentViewport";
    private const string ContentTextPartName = "PART_ContentText";
    private const string CloseButtonPartName = "PART_CloseButton";
    private const string SuffixButtonPartName = "PART_SuffixButton";

    private static readonly DispatcherTimer SharedMarqueeTimer;
    private static readonly List<WeakReference<NoticeBar>> MarqueeInstances = [];

    private Border? _contentViewport;
    private TextBlock? _contentText;
    private Button? _closeButton;
    private Button? _suffixButton;
    private double _textOffset;
    private long _startTimestamp;
    private double _marqueeWidth;
    private int _verticalIndex;
    private DispatcherTimer? _verticalTimer;
    private bool _hasOperation;
    private bool _hasSuffix;

    /// <inheritdoc cref="Visible" />
    public static readonly StyledProperty<bool> VisibleProperty =
        AvaloniaProperty.Register<NoticeBar, bool>(nameof(Visible), true);

    /// <inheritdoc cref="Theme" />
    public static readonly StyledProperty<NoticeBarTheme> ThemeProperty =
        AvaloniaProperty.Register<NoticeBar, NoticeBarTheme>(nameof(Theme), NoticeBarTheme.Info);

    /// <inheritdoc cref="Background" />
    public static readonly StyledProperty<IBrush?> BackgroundProperty =
        AvaloniaProperty.Register<NoticeBar, IBrush?>(nameof(Background));

    /// <inheritdoc cref="Foreground" />
    public static readonly StyledProperty<IBrush?> ForegroundProperty =
        AvaloniaProperty.Register<NoticeBar, IBrush?>(nameof(Foreground));

    /// <inheritdoc cref="IconForeground" />
    public static readonly StyledProperty<IBrush?> IconForegroundProperty =
        AvaloniaProperty.Register<NoticeBar, IBrush?>(nameof(IconForeground));

    /// <inheritdoc cref="Content" />
    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<NoticeBar, string?>(nameof(Content));

    /// <inheritdoc cref="ContentList" />
    public static readonly StyledProperty<IReadOnlyList<string>?> ContentListProperty =
        AvaloniaProperty.Register<NoticeBar, IReadOnlyList<string>?>(nameof(ContentList));

    /// <inheritdoc cref="ShowIcon" />
    public static readonly StyledProperty<bool> ShowIconProperty =
        AvaloniaProperty.Register<NoticeBar, bool>(nameof(ShowIcon), true);

    /// <inheritdoc cref="ShowClose" />
    public static readonly StyledProperty<bool> ShowCloseProperty =
        AvaloniaProperty.Register<NoticeBar, bool>(nameof(ShowClose));

    /// <inheritdoc cref="OperationText" />
    public static readonly StyledProperty<string?> OperationTextProperty =
        AvaloniaProperty.Register<NoticeBar, string?>(nameof(OperationText));

    /// <inheritdoc cref="HasOperation" />
    public static readonly DirectProperty<NoticeBar, bool> HasOperationProperty =
        AvaloniaProperty.RegisterDirect<NoticeBar, bool>(
            nameof(HasOperation),
            o => o.HasOperation);

    /// <inheritdoc cref="PrefixGlyph" />
    public static readonly StyledProperty<string?> PrefixGlyphProperty =
        AvaloniaProperty.Register<NoticeBar, string?>(nameof(PrefixGlyph), "!");

    /// <inheritdoc cref="SuffixGlyph" />
    public static readonly StyledProperty<string?> SuffixGlyphProperty =
        AvaloniaProperty.Register<NoticeBar, string?>(nameof(SuffixGlyph));

    /// <inheritdoc cref="HasSuffix" />
    public static readonly DirectProperty<NoticeBar, bool> HasSuffixProperty =
        AvaloniaProperty.RegisterDirect<NoticeBar, bool>(
            nameof(HasSuffix),
            o => o.HasSuffix);

    /// <inheritdoc cref="Marquee" />
    public static readonly StyledProperty<bool> MarqueeProperty =
        AvaloniaProperty.Register<NoticeBar, bool>(nameof(Marquee));

    /// <inheritdoc cref="Direction" />
    public static readonly StyledProperty<NoticeBarDirection> DirectionProperty =
        AvaloniaProperty.Register<NoticeBar, NoticeBarDirection>(nameof(Direction), NoticeBarDirection.Horizontal);

    /// <inheritdoc cref="MarqueeSpeed" />
    public static readonly StyledProperty<double> MarqueeSpeedProperty =
        AvaloniaProperty.Register<NoticeBar, double>(nameof(MarqueeSpeed), 36);

    /// <inheritdoc cref="MarqueeGap" />
    public static readonly StyledProperty<double> MarqueeGapProperty =
        AvaloniaProperty.Register<NoticeBar, double>(nameof(MarqueeGap), 32);

    /// <inheritdoc cref="VerticalInterval" />
    public static readonly StyledProperty<TimeSpan> VerticalIntervalProperty =
        AvaloniaProperty.Register<NoticeBar, TimeSpan>(nameof(VerticalInterval), TimeSpan.FromSeconds(2));

    static NoticeBar()
    {
        SharedMarqueeTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(16),
        };
        SharedMarqueeTimer.Tick += (_, _) =>
        {
            for (var i = MarqueeInstances.Count - 1; i >= 0; i--)
            {
                if (!MarqueeInstances[i].TryGetTarget(out var target))
                {
                    MarqueeInstances.RemoveAt(i);
                    continue;
                }

                if (!target.IsMarqueeActive())
                {
                    continue;
                }

                target.UpdateMarqueeFrame();
            }

            if (MarqueeInstances.Count == 0)
            {
                SharedMarqueeTimer.Stop();
            }
        };

        ThemeProperty.Changed.AddClassHandler<NoticeBar>((control, args) =>
        {
            control.UpdateThemePseudo(args.GetNewValue<NoticeBarTheme>());
        });

        DirectionProperty.Changed.AddClassHandler<NoticeBar>((control, args) =>
        {
            control.UpdateDirectionPseudo(args.GetNewValue<NoticeBarDirection>());
            control.UpdateVerticalTimer();
            control.RestartMarquee();
        });

        MarqueeProperty.Changed.AddClassHandler<NoticeBar>((control, _) =>
        {
            control.UpdateMarqueePseudo();
            control.UpdateVerticalTimer();
            control.RestartMarquee();
        });

        ContentProperty.Changed.AddClassHandler<NoticeBar>((control, _) =>
        {
            control.RestartMarquee();
        });

        ContentListProperty.Changed.AddClassHandler<NoticeBar>((control, _) =>
        {
            control.UpdateVerticalTimer();
        });

        VisibleProperty.Changed.AddClassHandler<NoticeBar>((control, args) =>
        {
            control.IsVisible = args.GetNewValue<bool>();
            control.UpdateMarqueePseudo();
            control.UpdateVerticalTimer();
            control.RestartMarquee();
        });

        OperationTextProperty.Changed.AddClassHandler<NoticeBar>((control, args) =>
        {
            control.HasOperation = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });

        SuffixGlyphProperty.Changed.AddClassHandler<NoticeBar>((control, args) =>
        {
            control.HasSuffix = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });
    }

    /// <summary>
    /// 初始化 <see cref="NoticeBar" /> 的新实例。
    /// </summary>
    public NoticeBar()
    {
        IsVisible = Visible;
        UpdateThemePseudo(Theme);
        UpdateDirectionPseudo(Direction);
        UpdateMarqueePseudo();
        HasOperation = !string.IsNullOrWhiteSpace(OperationText);
        HasSuffix = !string.IsNullOrWhiteSpace(SuffixGlyph);
    }

    /// <summary>
    /// 获取或设置通知栏是否可见。
    /// </summary>
    public bool Visible
    {
        get => GetValue(VisibleProperty);
        set => SetValue(VisibleProperty, value);
    }

    /// <summary>
    /// 获取或设置通知栏主题。
    /// </summary>
    public NoticeBarTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置当前显示的通知文本。
    /// </summary>
    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// 获取或设置轮播文本列表。
    /// </summary>
    public IReadOnlyList<string>? ContentList
    {
        get => GetValue(ContentListProperty);
        set => SetValue(ContentListProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示前缀图标。
    /// </summary>
    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示关闭按钮。
    /// </summary>
    public bool ShowClose
    {
        get => GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    /// <summary>
    /// 获取或设置操作文案。
    /// </summary>
    public string? OperationText
    {
        get => GetValue(OperationTextProperty);
        set => SetValue(OperationTextProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在操作文案。
    /// </summary>
    public bool HasOperation
    {
        get => _hasOperation;
        private set => SetAndRaise(HasOperationProperty, ref _hasOperation, value);
    }

    /// <summary>
    /// 获取或设置前缀图标字符。
    /// </summary>
    public string? PrefixGlyph
    {
        get => GetValue(PrefixGlyphProperty);
        set => SetValue(PrefixGlyphProperty, value);
    }

    /// <summary>
    /// 获取或设置后缀图标字符。
    /// </summary>
    public string? SuffixGlyph
    {
        get => GetValue(SuffixGlyphProperty);
        set => SetValue(SuffixGlyphProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在后缀图标。
    /// </summary>
    public bool HasSuffix
    {
        get => _hasSuffix;
        private set => SetAndRaise(HasSuffixProperty, ref _hasSuffix, value);
    }

    /// <summary>
    /// 获取或设置是否启用滚动播放。
    /// </summary>
    public bool Marquee
    {
        get => GetValue(MarqueeProperty);
        set => SetValue(MarqueeProperty, value);
    }

    /// <summary>
    /// 获取或设置滚动方向。
    /// </summary>
    public NoticeBarDirection Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    /// <summary>
    /// 获取或设置横向跑马灯速度。
    /// </summary>
    public double MarqueeSpeed
    {
        get => GetValue(MarqueeSpeedProperty);
        set => SetValue(MarqueeSpeedProperty, value);
    }

    /// <summary>
    /// 获取或设置横向跑马灯循环间距。
    /// </summary>
    public double MarqueeGap
    {
        get => GetValue(MarqueeGapProperty);
        set => SetValue(MarqueeGapProperty, value);
    }

    /// <summary>
    /// 获取或设置竖向轮播切换间隔。
    /// </summary>
    public TimeSpan VerticalInterval
    {
        get => GetValue(VerticalIntervalProperty);
        set => SetValue(VerticalIntervalProperty, value);
    }

    /// <summary>
    /// 获取或设置背景画刷。
    /// </summary>
    public IBrush? Background
    {
        get => GetValue(BackgroundProperty);
        set => SetValue(BackgroundProperty, value);
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
    /// 获取或设置图标前景画刷。
    /// </summary>
    public IBrush? IconForeground
    {
        get => GetValue(IconForegroundProperty);
        set => SetValue(IconForegroundProperty, value);
    }

    /// <summary>
    /// 用户点击关闭按钮时触发。
    /// </summary>
    public event EventHandler? CloseRequested;
    /// <summary>
    /// 用户点击后缀操作区域时触发。
    /// </summary>
    public event EventHandler? SuffixRequested;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        _contentViewport = e.NameScope.Find<Border>(ContentViewportPartName);
        _contentText = e.NameScope.Find<TextBlock>(ContentTextPartName);

        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseClick;
        }
        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseClick;
        }

        if (_suffixButton is not null)
        {
            _suffixButton.Click -= OnSuffixClick;
        }
        _suffixButton = e.NameScope.Find<Button>(SuffixButtonPartName);
        if (_suffixButton is not null)
        {
            _suffixButton.Click += OnSuffixClick;
        }

        if (_contentViewport is not null)
        {
            _contentViewport.AttachedToVisualTree += (_, _) => RestartMarquee();
            _contentViewport.DetachedFromVisualTree += (_, _) => StopMarquee();
            _contentViewport.SizeChanged += (_, _) => RestartMarquee();
        }

        RestartMarquee();
        UpdateVerticalTimer();
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (MarqueeInstances.Count == 0)
        {
            SharedMarqueeTimer.Start();
        }
        MarqueeInstances.Add(new WeakReference<NoticeBar>(this));
        _startTimestamp = Stopwatch.GetTimestamp();
        UpdateVerticalTimer();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        StopMarquee();
        StopVerticalTimer();

        for (var i = MarqueeInstances.Count - 1; i >= 0; i--)
        {
            if (MarqueeInstances[i].TryGetTarget(out var target) && ReferenceEquals(target, this))
            {
                MarqueeInstances.RemoveAt(i);
            }
        }
    }

    private void OnCloseClick(object? sender, EventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnSuffixClick(object? sender, EventArgs e)
    {
        SuffixRequested?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateThemePseudo(NoticeBarTheme theme)
    {
        PseudoClasses.Set(":info", theme == NoticeBarTheme.Info);
        PseudoClasses.Set(":success", theme == NoticeBarTheme.Success);
        PseudoClasses.Set(":warning", theme == NoticeBarTheme.Warning);
        PseudoClasses.Set(":error", theme == NoticeBarTheme.Error);
    }

    private void UpdateDirectionPseudo(NoticeBarDirection direction)
    {
        PseudoClasses.Set(":horizontal", direction == NoticeBarDirection.Horizontal);
        PseudoClasses.Set(":vertical", direction == NoticeBarDirection.Vertical);
    }

    private void UpdateMarqueePseudo()
    {
        PseudoClasses.Set(":marquee", Marquee);
    }

    private bool IsMarqueeActive()
    {
        return Visible
               && Marquee
               && Direction == NoticeBarDirection.Horizontal
               && _contentViewport is not null
               && _contentText is not null
               && _marqueeWidth > 0;
    }

    private void RestartMarquee()
    {
        if (_contentViewport is null || _contentText is null)
        {
            return;
        }

        _textOffset = 0;
        _startTimestamp = Stopwatch.GetTimestamp();
        _contentText.RenderTransform = new TranslateTransform(0, 0);

        if (!Visible || !Marquee || Direction != NoticeBarDirection.Horizontal)
        {
            _marqueeWidth = 0;
            return;
        }

        _contentText.Measure(new Size(double.PositiveInfinity, _contentViewport.Bounds.Height));
        var textWidth = _contentText.DesiredSize.Width;
        var viewportWidth = _contentViewport.Bounds.Width;
        _marqueeWidth = textWidth - viewportWidth;

        if (_marqueeWidth <= 1)
        {
            _marqueeWidth = 0;
        }
    }

    private void StopMarquee()
    {
        _marqueeWidth = 0;
        _textOffset = 0;
        if (_contentText is not null)
        {
            _contentText.RenderTransform = new TranslateTransform(0, 0);
        }
    }

    private void UpdateMarqueeFrame()
    {
        if (_contentViewport is null || _contentText is null)
        {
            return;
        }

        var speed = MarqueeSpeed;
        if (speed <= 0)
        {
            speed = 36;
        }

        var elapsedSeconds = (Stopwatch.GetTimestamp() - _startTimestamp) / (double)Stopwatch.Frequency;
        var distance = elapsedSeconds * speed;
        var cycle = _marqueeWidth + MarqueeGap;
        if (cycle <= 0)
        {
            return;
        }

        var offset = -(distance % cycle);
        _contentText.RenderTransform = new TranslateTransform(offset, 0);
    }

    private void UpdateVerticalTimer()
    {
        if (!Visible || !Marquee || Direction != NoticeBarDirection.Vertical)
        {
            StopVerticalTimer();
            return;
        }

        var list = ContentList;
        if (list is null || list.Count == 0)
        {
            StopVerticalTimer();
            return;
        }

        _verticalIndex %= list.Count;
        Content = list[_verticalIndex];

        if (_verticalTimer is null)
        {
            _verticalTimer = new DispatcherTimer();
            _verticalTimer.Tick += (_, _) =>
            {
                var items = ContentList;
                if (items is null || items.Count == 0)
                {
                    return;
                }

                _verticalIndex = (_verticalIndex + 1) % items.Count;
                Content = items[_verticalIndex];
            };
        }

        var interval = VerticalInterval;
        if (interval <= TimeSpan.Zero)
        {
            interval = TimeSpan.FromSeconds(2);
        }

        _verticalTimer.Interval = interval;
        if (!_verticalTimer.IsEnabled)
        {
            _verticalTimer.Start();
        }
    }

    private void StopVerticalTimer()
    {
        if (_verticalTimer is null)
        {
            return;
        }

        _verticalTimer.Stop();
    }
}
