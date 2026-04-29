using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// Toast 宿主控件，负责渲染 Toast 以及遮罩/自动关闭逻辑。
/// </summary>
/// <remarks>
/// 通常每个页面放置一个实例；静态入口 <see cref="Toast"/> 会使用最近附加到可视树的 <see cref="Current"/>。
/// </remarks>
public sealed class ToastHost : TemplatedControl
{
    private const string CloseButtonPartName = "PART_CloseButton";

    private static ToastHost? _current;

    private readonly DispatcherTimer _timer;
    private Button? _closeButton;
    private TimeSpan _duration;
    private bool _isOverlayVisible;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<ToastHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<ToastHost, string?>(nameof(Message));

    public static readonly StyledProperty<string?> IconGlyphProperty =
        AvaloniaProperty.Register<ToastHost, string?>(nameof(IconGlyph));

    public static readonly StyledProperty<bool> IsIconVisibleProperty =
        AvaloniaProperty.Register<ToastHost, bool>(nameof(IsIconVisible));

    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<ToastHost, bool>(nameof(ShowOverlay));

    public static readonly DirectProperty<ToastHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<ToastHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    public static readonly StyledProperty<bool> ShowCloseProperty =
        AvaloniaProperty.Register<ToastHost, bool>(nameof(ShowClose));

    public static readonly StyledProperty<Orientation> ToastOrientationProperty =
        AvaloniaProperty.Register<ToastHost, Orientation>(nameof(ToastOrientation), Orientation.Horizontal);

    public static readonly StyledProperty<VerticalAlignment> ToastVerticalAlignmentProperty =
        AvaloniaProperty.Register<ToastHost, VerticalAlignment>(nameof(ToastVerticalAlignment), VerticalAlignment.Center);

    static ToastHost()
    {
        IsOpenProperty.Changed.AddClassHandler<ToastHost>((control, _) => control.UpdateOverlayVisible());
        ShowOverlayProperty.Changed.AddClassHandler<ToastHost>((control, _) => control.UpdateOverlayVisible());
    }

    public ToastHost()
    {
        _timer = new DispatcherTimer();
        _timer.Tick += (_, _) => Clear();
    }

    public static ToastHost? Current => _current;

    /// <summary>
    /// 获取或设置当前是否显示 Toast。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置消息文本。
    /// </summary>
    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    /// <summary>
    /// 获取或设置图标字符（由主题决定）。
    /// </summary>
    public string? IconGlyph
    {
        get => GetValue(IconGlyphProperty);
        set => SetValue(IconGlyphProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示图标区域。
    /// </summary>
    public bool IsIconVisible
    {
        get => GetValue(IsIconVisibleProperty);
        set => SetValue(IsIconVisibleProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    /// <summary>
    /// 获取当前遮罩层是否可见（由 <see cref="IsOpen"/> 与 <see cref="ShowOverlay"/> 自动计算）。
    /// </summary>
    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
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
    /// 获取或设置 Toast 内容布局方向。
    /// </summary>
    public Orientation ToastOrientation
    {
        get => GetValue(ToastOrientationProperty);
        set => SetValue(ToastOrientationProperty, value);
    }

    /// <summary>
    /// 获取或设置 Toast 的垂直对齐方式（对应 Top/Middle/Bottom）。
    /// </summary>
    public VerticalAlignment ToastVerticalAlignment
    {
        get => GetValue(ToastVerticalAlignmentProperty);
        set => SetValue(ToastVerticalAlignmentProperty, value);
    }

    /// <summary>
    /// 使用默认参数显示 Toast。
    /// </summary>
    public void Show(string message)
    {
        Show(new ToastOptions { Message = message });
    }

    /// <summary>
    /// 使用自定义参数显示 Toast。
    /// </summary>
    public void Show(ToastOptions options)
    {
        _duration = options.Duration;
        Message = options.Message;
        ShowOverlay = options.ShowOverlay;
        ShowClose = options.ShowClose;
        ToastOrientation = options.Direction == ToastDirection.Column ? Orientation.Vertical : Orientation.Horizontal;
        ToastVerticalAlignment = options.Placement switch
        {
            ToastPlacement.Top => VerticalAlignment.Top,
            ToastPlacement.Bottom => VerticalAlignment.Bottom,
            _ => VerticalAlignment.Center,
        };
        IconGlyph = options.Theme switch
        {
            ToastTheme.Success => "✓",
            ToastTheme.Warning => "!",
            ToastTheme.Error => "×",
            ToastTheme.Loading => "…",
            _ => null,
        };
        IsIconVisible = options.Theme != ToastTheme.Default;
        IsOpen = true;

        _timer.Stop();
        if (_duration > TimeSpan.Zero)
        {
            _timer.Interval = _duration;
            _timer.Start();
        }
    }

    /// <summary>
    /// 清除/关闭当前 Toast。
    /// </summary>
    public void Clear()
    {
        _timer.Stop();
        IsOpen = false;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseButtonClick;
        }

        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseButtonClick;
        }
    }

    private void OnCloseButtonClick(object? sender, EventArgs e)
    {
        Clear();
    }

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen && ShowOverlay;
    }
}
