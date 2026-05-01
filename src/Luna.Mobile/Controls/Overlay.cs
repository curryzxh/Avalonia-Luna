using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 轻量遮罩层控件，可用于承载模态背景或自定义浮层内容。
/// </summary>
[TemplatePart(BackdropPartName, typeof(Border))]
public sealed class Overlay : ContentControl
{
    private const string BackdropPartName = "PART_Backdrop";

    private Border? _backdrop;
    private DispatcherTimer? _transitionTimer;
    private bool _isRendered;
    private bool _isBackdropInteractive;
    private double _overlayOpacity;

    /// <inheritdoc cref="Visible" />
    public static readonly StyledProperty<bool> VisibleProperty =
        AvaloniaProperty.Register<Overlay, bool>(nameof(Visible));

    /// <inheritdoc cref="OverlayBrush" />
    public static readonly StyledProperty<IBrush?> OverlayBrushProperty =
        AvaloniaProperty.Register<Overlay, IBrush?>(nameof(OverlayBrush));

    /// <inheritdoc cref="Duration" />
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<Overlay, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(300));

    /// <inheritdoc cref="PreventScrollThrough" />
    public static readonly StyledProperty<bool> PreventScrollThroughProperty =
        AvaloniaProperty.Register<Overlay, bool>(nameof(PreventScrollThrough), true);

    /// <inheritdoc cref="ZIndex" />
    public static readonly new StyledProperty<int> ZIndexProperty =
        Panel.ZIndexProperty.AddOwner<Overlay>();

    /// <inheritdoc cref="IsRendered" />
    public static readonly DirectProperty<Overlay, bool> IsRenderedProperty =
        AvaloniaProperty.RegisterDirect<Overlay, bool>(
            nameof(IsRendered),
            o => o.IsRendered);

    /// <inheritdoc cref="IsBackdropInteractive" />
    public static readonly DirectProperty<Overlay, bool> IsBackdropInteractiveProperty =
        AvaloniaProperty.RegisterDirect<Overlay, bool>(
            nameof(IsBackdropInteractive),
            o => o.IsBackdropInteractive);

    /// <inheritdoc cref="OverlayOpacity" />
    public static readonly DirectProperty<Overlay, double> OverlayOpacityProperty =
        AvaloniaProperty.RegisterDirect<Overlay, double>(
            nameof(OverlayOpacity),
            o => o.OverlayOpacity);

    static Overlay()
    {
        VisibleProperty.Changed.AddClassHandler<Overlay>((control, args) =>
        {
            control.UpdateVisibility(args.GetNewValue<bool>());
        });

        PreventScrollThroughProperty.Changed.AddClassHandler<Overlay>((control, _) =>
        {
            control.UpdateBackdropInteractive();
        });
    }

    /// <summary>
    /// 初始化 <see cref="Overlay" /> 的新实例。
    /// </summary>
    public Overlay()
    {
        IsRendered = Visible;
        OverlayOpacity = Visible ? 1 : 0;
        UpdateBackdropInteractive();
    }

    /// <summary>
    /// 获取或设置遮罩当前是否显示。
    /// </summary>
    public bool Visible
    {
        get => GetValue(VisibleProperty);
        set => SetValue(VisibleProperty, value);
    }

    /// <summary>
    /// 获取或设置遮罩背景画刷。
    /// </summary>
    public IBrush? OverlayBrush
    {
        get => GetValue(OverlayBrushProperty);
        set => SetValue(OverlayBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置显示/隐藏过渡时长。
    /// </summary>
    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>
    /// 获取或设置是否阻止点击和滚动穿透到底层内容。
    /// </summary>
    public bool PreventScrollThrough
    {
        get => GetValue(PreventScrollThroughProperty);
        set => SetValue(PreventScrollThroughProperty, value);
    }

    /// <summary>
    /// 获取或设置遮罩的层级。
    /// </summary>
    public new int ZIndex
    {
        get => GetValue(ZIndexProperty);
        set => SetValue(ZIndexProperty, value);
    }

    /// <summary>
    /// 获取当前是否应渲染遮罩节点。
    /// </summary>
    public bool IsRendered
    {
        get => _isRendered;
        private set => SetAndRaise(IsRenderedProperty, ref _isRendered, value);
    }

    /// <summary>
    /// 获取当前背景层是否拦截输入。
    /// </summary>
    public bool IsBackdropInteractive
    {
        get => _isBackdropInteractive;
        private set => SetAndRaise(IsBackdropInteractiveProperty, ref _isBackdropInteractive, value);
    }

    /// <summary>
    /// 获取当前遮罩透明度，用于模板过渡动画。
    /// </summary>
    public double OverlayOpacity
    {
        get => _overlayOpacity;
        private set => SetAndRaise(OverlayOpacityProperty, ref _overlayOpacity, value);
    }

    /// <summary>
    /// 当用户点击遮罩背景时触发。
    /// </summary>
    public event EventHandler? Clicked;

    /// <summary>
    /// 遮罩开始显示前触发。
    /// </summary>
    public event EventHandler? Opening;

    /// <summary>
    /// 遮罩显示完成后触发。
    /// </summary>
    public event EventHandler? Opened;

    /// <summary>
    /// 遮罩开始关闭前触发。
    /// </summary>
    public event EventHandler? Closing;

    /// <summary>
    /// 遮罩关闭完成后触发。
    /// </summary>
    public event EventHandler? Closed;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_backdrop is not null)
        {
            _backdrop.PointerPressed -= OnBackdropPressed;
        }

        _backdrop = e.NameScope.Find<Border>(BackdropPartName);
        if (_backdrop is not null)
        {
            _backdrop.PointerPressed += OnBackdropPressed;
        }
    }

    private void OnBackdropPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsRendered || !IsBackdropInteractive)
        {
            return;
        }

        e.Handled = true;
        Clicked?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateVisibility(bool isVisible)
    {
        StopTransitionTimer();

        if (isVisible)
        {
            Opening?.Invoke(this, EventArgs.Empty);
            IsRendered = true;
            UpdateBackdropInteractive();
            OverlayOpacity = 0;

            Dispatcher.UIThread.Post(() =>
            {
                if (Visible)
                {
                    OverlayOpacity = 1;
                }
            }, DispatcherPriority.Render);

            StartTransitionTimer(() =>
            {
                if (Visible)
                {
                    Opened?.Invoke(this, EventArgs.Empty);
                }
            });

            return;
        }

        if (!IsRendered)
        {
            return;
        }

        Closing?.Invoke(this, EventArgs.Empty);
        OverlayOpacity = 0;

        StartTransitionTimer(() =>
        {
            if (Visible)
            {
                return;
            }

            IsRendered = false;
            UpdateBackdropInteractive();
            Closed?.Invoke(this, EventArgs.Empty);
        });
    }

    private void UpdateBackdropInteractive()
    {
        IsBackdropInteractive = IsRendered && PreventScrollThrough;
    }

    private void StartTransitionTimer(Action completed)
    {
        var duration = Duration;
        if (duration <= TimeSpan.Zero)
        {
            completed();
            return;
        }

        _transitionTimer = new DispatcherTimer
        {
            Interval = duration,
        };
        _transitionTimer.Tick += (_, _) =>
        {
            StopTransitionTimer();
            completed();
        };
        _transitionTimer.Start();
    }

    private void StopTransitionTimer()
    {
        if (_transitionTimer is null)
        {
            return;
        }

        _transitionTimer.Stop();
        _transitionTimer = null;
    }
}
