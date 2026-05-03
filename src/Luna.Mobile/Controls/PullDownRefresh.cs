using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Threading;
using System;
using System.Windows.Input;

namespace Luna.Mobile.Controls;

/// <summary>
/// 下拉刷新控件状态。
/// </summary>
public enum PullDownRefreshStatus
{
    Idle,
    Pulling,
    ReleaseToRefresh,
    Refreshing,
    Complete,
}

/// <summary>
/// 下拉刷新控件，提供下拉阈值、释放刷新和完成提示。
/// </summary>
[TemplatePart(ScrollViewerPartName, typeof(ScrollViewer))]
public sealed class PullDownRefresh : ContentControl
{
    private const string ScrollViewerPartName = "PART_ScrollViewer";
    private const double DragStartThreshold = 4d;

    private readonly TranslateTransform _contentTransform = new();
    private ScrollViewer? _scrollViewer;
    private bool _isPointerPressed;
    private bool _isDragging;
    private bool _isCompleting;
    private Point _pressedPoint;
    private double _pullDistance;
    private double _indicatorVisibleHeight;
    private double _contentOffset;
    private string _statusText = "下拉刷新";
    private string _statusGlyph = "↓";
    private bool _showLoadingIndicator;
    private bool _showStatusGlyph = true;
    private PullDownRefreshStatus _status;
    private DispatcherTimer? _completionTimer;

    public static readonly StyledProperty<bool> IsRefreshingProperty =
        AvaloniaProperty.Register<PullDownRefresh, bool>(nameof(IsRefreshing));

    public static readonly StyledProperty<double> ThresholdProperty =
        AvaloniaProperty.Register<PullDownRefresh, double>(nameof(Threshold), 66d);

    public static readonly StyledProperty<double> IndicatorHeightProperty =
        AvaloniaProperty.Register<PullDownRefresh, double>(nameof(IndicatorHeight), 66d);

    public static readonly StyledProperty<double> MaxPullDistanceProperty =
        AvaloniaProperty.Register<PullDownRefresh, double>(nameof(MaxPullDistance), 132d);

    public static readonly StyledProperty<string> PullTextProperty =
        AvaloniaProperty.Register<PullDownRefresh, string>(nameof(PullText), "下拉刷新");

    public static readonly StyledProperty<string> ReleaseTextProperty =
        AvaloniaProperty.Register<PullDownRefresh, string>(nameof(ReleaseText), "松开刷新");

    public static readonly StyledProperty<string> RefreshingTextProperty =
        AvaloniaProperty.Register<PullDownRefresh, string>(nameof(RefreshingText), "正在刷新");

    public static readonly StyledProperty<string> CompleteTextProperty =
        AvaloniaProperty.Register<PullDownRefresh, string>(nameof(CompleteText), "刷新完成");

    public static readonly StyledProperty<TimeSpan> CompleteVisibleDurationProperty =
        AvaloniaProperty.Register<PullDownRefresh, TimeSpan>(nameof(CompleteVisibleDuration), TimeSpan.FromMilliseconds(600));

    public static readonly StyledProperty<ICommand?> RefreshCommandProperty =
        AvaloniaProperty.Register<PullDownRefresh, ICommand?>(nameof(RefreshCommand));

    public static readonly StyledProperty<object?> RefreshCommandParameterProperty =
        AvaloniaProperty.Register<PullDownRefresh, object?>(nameof(RefreshCommandParameter));

    public static readonly DirectProperty<PullDownRefresh, double> PullDistanceProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, double>(
            nameof(PullDistance),
            o => o.PullDistance);

    public static readonly DirectProperty<PullDownRefresh, double> IndicatorVisibleHeightProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, double>(
            nameof(IndicatorVisibleHeight),
            o => o.IndicatorVisibleHeight);

    public static readonly DirectProperty<PullDownRefresh, double> ContentOffsetProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, double>(
            nameof(ContentOffset),
            o => o.ContentOffset);

    public static readonly DirectProperty<PullDownRefresh, string> StatusTextProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, string>(
            nameof(StatusText),
            o => o.StatusText);

    public static readonly DirectProperty<PullDownRefresh, string> StatusGlyphProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, string>(
            nameof(StatusGlyph),
            o => o.StatusGlyph);

    public static readonly DirectProperty<PullDownRefresh, bool> ShowLoadingIndicatorProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, bool>(
            nameof(ShowLoadingIndicator),
            o => o.ShowLoadingIndicator);

    public static readonly DirectProperty<PullDownRefresh, bool> ShowStatusGlyphProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, bool>(
            nameof(ShowStatusGlyph),
            o => o.ShowStatusGlyph);

    public static readonly DirectProperty<PullDownRefresh, PullDownRefreshStatus> StatusProperty =
        AvaloniaProperty.RegisterDirect<PullDownRefresh, PullDownRefreshStatus>(
            nameof(Status),
            o => o.Status);

    public static readonly RoutedEvent<RoutedEventArgs> RefreshRequestedEvent =
        RoutedEvent.Register<PullDownRefresh, RoutedEventArgs>(nameof(RefreshRequested), RoutingStrategies.Bubble);

    static PullDownRefresh()
    {
        ClipToBoundsProperty.OverrideDefaultValue<PullDownRefresh>(true);
        IsRefreshingProperty.Changed.AddClassHandler<PullDownRefresh>((control, args) => control.OnIsRefreshingChanged(args.GetNewValue<bool>()));
        ThresholdProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateStatusState());
        IndicatorHeightProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateOffsets());
        PullTextProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateStatusState());
        ReleaseTextProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateStatusState());
        RefreshingTextProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateStatusState());
        CompleteTextProperty.Changed.AddClassHandler<PullDownRefresh>((control, _) => control.UpdateStatusState());
    }

    public PullDownRefresh()
    {
        UpdateStatusState();
    }

    /// <summary>
    /// 刷新请求触发时发生。
    /// </summary>
    public event EventHandler<RoutedEventArgs> RefreshRequested
    {
        add => AddHandler(RefreshRequestedEvent, value);
        remove => RemoveHandler(RefreshRequestedEvent, value);
    }

    /// <summary>
    /// 获取或设置当前是否处于刷新中。
    /// </summary>
    public bool IsRefreshing
    {
        get => GetValue(IsRefreshingProperty);
        set => SetValue(IsRefreshingProperty, value);
    }

    /// <summary>
    /// 获取或设置触发刷新所需的下拉阈值。
    /// </summary>
    public double Threshold
    {
        get => GetValue(ThresholdProperty);
        set => SetValue(ThresholdProperty, value);
    }

    /// <summary>
    /// 获取或设置刷新指示区高度。
    /// </summary>
    public double IndicatorHeight
    {
        get => GetValue(IndicatorHeightProperty);
        set => SetValue(IndicatorHeightProperty, value);
    }

    /// <summary>
    /// 获取或设置最大允许下拉距离。
    /// </summary>
    public double MaxPullDistance
    {
        get => GetValue(MaxPullDistanceProperty);
        set => SetValue(MaxPullDistanceProperty, value);
    }

    /// <summary>
    /// 获取或设置普通下拉时的提示文案。
    /// </summary>
    public string PullText
    {
        get => GetValue(PullTextProperty);
        set => SetValue(PullTextProperty, value);
    }

    /// <summary>
    /// 获取或设置达到阈值后的提示文案。
    /// </summary>
    public string ReleaseText
    {
        get => GetValue(ReleaseTextProperty);
        set => SetValue(ReleaseTextProperty, value);
    }

    /// <summary>
    /// 获取或设置刷新中的提示文案。
    /// </summary>
    public string RefreshingText
    {
        get => GetValue(RefreshingTextProperty);
        set => SetValue(RefreshingTextProperty, value);
    }

    /// <summary>
    /// 获取或设置刷新完成提示文案。
    /// </summary>
    public string CompleteText
    {
        get => GetValue(CompleteTextProperty);
        set => SetValue(CompleteTextProperty, value);
    }

    /// <summary>
    /// 获取或设置完成态停留时长。
    /// </summary>
    public TimeSpan CompleteVisibleDuration
    {
        get => GetValue(CompleteVisibleDurationProperty);
        set => SetValue(CompleteVisibleDurationProperty, value);
    }

    /// <summary>
    /// 获取或设置下拉刷新触发时执行的命令。
    /// </summary>
    public ICommand? RefreshCommand
    {
        get => GetValue(RefreshCommandProperty);
        set => SetValue(RefreshCommandProperty, value);
    }

    /// <summary>
    /// 获取或设置下拉刷新命令参数。
    /// </summary>
    public object? RefreshCommandParameter
    {
        get => GetValue(RefreshCommandParameterProperty);
        set => SetValue(RefreshCommandParameterProperty, value);
    }

    /// <summary>
    /// 获取当前实际下拉距离。
    /// </summary>
    public double PullDistance
    {
        get => _pullDistance;
        private set => SetAndRaise(PullDistanceProperty, ref _pullDistance, value);
    }

    /// <summary>
    /// 获取当前指示区可见高度。
    /// </summary>
    public double IndicatorVisibleHeight
    {
        get => _indicatorVisibleHeight;
        private set => SetAndRaise(IndicatorVisibleHeightProperty, ref _indicatorVisibleHeight, value);
    }

    /// <summary>
    /// 获取当前内容区域偏移量。
    /// </summary>
    public double ContentOffset
    {
        get => _contentOffset;
        private set => SetAndRaise(ContentOffsetProperty, ref _contentOffset, value);
    }

    /// <summary>
    /// 获取当前状态文案。
    /// </summary>
    public string StatusText
    {
        get => _statusText;
        private set => SetAndRaise(StatusTextProperty, ref _statusText, value);
    }

    /// <summary>
    /// 获取当前状态符号。
    /// </summary>
    public string StatusGlyph
    {
        get => _statusGlyph;
        private set => SetAndRaise(StatusGlyphProperty, ref _statusGlyph, value);
    }

    /// <summary>
    /// 获取当前是否显示刷新指示器。
    /// </summary>
    public bool ShowLoadingIndicator
    {
        get => _showLoadingIndicator;
        private set => SetAndRaise(ShowLoadingIndicatorProperty, ref _showLoadingIndicator, value);
    }

    /// <summary>
    /// 获取当前是否显示状态符号。
    /// </summary>
    public bool ShowStatusGlyph
    {
        get => _showStatusGlyph;
        private set => SetAndRaise(ShowStatusGlyphProperty, ref _showStatusGlyph, value);
    }

    /// <summary>
    /// 获取当前刷新状态。
    /// </summary>
    public PullDownRefreshStatus Status
    {
        get => _status;
        private set => SetAndRaise(StatusProperty, ref _status, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        DetachScrollViewer();
        base.OnApplyTemplate(e);

        _scrollViewer = e.NameScope.Find<ScrollViewer>(ScrollViewerPartName);
        if (_scrollViewer is not null)
        {
            _scrollViewer.RenderTransform = _contentTransform;
            _scrollViewer.PointerPressed += OnScrollViewerPointerPressed;
            _scrollViewer.PointerMoved += OnScrollViewerPointerMoved;
            _scrollViewer.PointerReleased += OnScrollViewerPointerReleased;
            _scrollViewer.PointerCaptureLost += OnScrollViewerPointerCaptureLost;
        }

        UpdateOffsets();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        StopCompletionTimer();
        DetachScrollViewer();
        base.OnDetachedFromVisualTree(e);
    }

    private void OnScrollViewerPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsEnabled || IsRefreshing || _scrollViewer is null)
        {
            return;
        }

        if (!e.GetCurrentPoint(_scrollViewer).Properties.IsLeftButtonPressed || !IsAtTop())
        {
            return;
        }

        _isPointerPressed = true;
        _isDragging = false;
        _pressedPoint = e.GetPosition(this);
    }

    private void OnScrollViewerPointerMoved(object? sender, PointerEventArgs e)
    {
        if (!_isPointerPressed || IsRefreshing || _scrollViewer is null)
        {
            return;
        }

        var currentPoint = e.GetPosition(this);
        var deltaY = currentPoint.Y - _pressedPoint.Y;

        if (!_isDragging)
        {
            if (deltaY <= DragStartThreshold || !IsAtTop())
            {
                return;
            }

            _isDragging = true;
            e.Pointer.Capture(_scrollViewer);
        }

        if (deltaY <= 0)
        {
            UpdatePullDistance(0);
            e.Handled = true;
            return;
        }

        UpdatePullDistance(ApplyResistance(deltaY));
        UpdateStatusState();
        e.Handled = true;
    }

    private void OnScrollViewerPointerReleased(object? sender, PointerReleasedEventArgs e)
    {
        if (!_isPointerPressed)
        {
            return;
        }

        if (_isDragging && PullDistance >= Threshold)
        {
            BeginRefresh();
        }
        else if (!_isCompleting && !IsRefreshing)
        {
            ResetToIdle();
        }

        CleanupPointer(e.Pointer);
    }

    private void OnScrollViewerPointerCaptureLost(object? sender, PointerCaptureLostEventArgs e)
    {
        CleanupPointer(e.Pointer);
        if (!IsRefreshing && !_isCompleting)
        {
            ResetToIdle();
        }
    }

    private void CleanupPointer(IPointer? pointer)
    {
        if (_scrollViewer is not null && pointer?.Captured == _scrollViewer)
        {
            pointer.Capture(null);
        }

        _isPointerPressed = false;
        _isDragging = false;
    }

    private void BeginRefresh()
    {
        StopCompletionTimer();
        _isCompleting = false;
        IsRefreshing = true;
        RaiseEvent(new RoutedEventArgs(RefreshRequestedEvent));
        ExecuteRefreshCommand();
    }

    private void ExecuteRefreshCommand()
    {
        var command = RefreshCommand;
        if (command is null)
        {
            return;
        }

        var parameter = RefreshCommandParameter;
        if (command.CanExecute(parameter))
        {
            command.Execute(parameter);
        }
    }

    private void OnIsRefreshingChanged(bool isRefreshing)
    {
        if (isRefreshing)
        {
            StopCompletionTimer();
            _isCompleting = false;
            PullDistance = Math.Max(IndicatorHeight, Threshold);
            UpdateOffsets();
            UpdateStatusState();
            return;
        }

        ShowCompleteState();
    }

    private void ShowCompleteState()
    {
        StopCompletionTimer();
        _isCompleting = true;
        PullDistance = Math.Max(IndicatorHeight, Threshold);
        UpdateOffsets();
        UpdateStatusState();

        _completionTimer = new DispatcherTimer
        {
            Interval = CompleteVisibleDuration <= TimeSpan.Zero
                ? TimeSpan.FromMilliseconds(1)
                : CompleteVisibleDuration,
        };
        _completionTimer.Tick += OnCompletionTimerTick;
        _completionTimer.Start();
    }

    private void OnCompletionTimerTick(object? sender, EventArgs e)
    {
        StopCompletionTimer();
        _isCompleting = false;
        ResetToIdle();
    }

    private void StopCompletionTimer()
    {
        if (_completionTimer is null)
        {
            return;
        }

        _completionTimer.Tick -= OnCompletionTimerTick;
        _completionTimer.Stop();
        _completionTimer = null;
    }

    private void ResetToIdle()
    {
        PullDistance = 0;
        UpdateOffsets();
        UpdateStatusState();
    }

    private void UpdatePullDistance(double value)
    {
        var maxDistance = Math.Max(MaxPullDistance, Threshold);
        PullDistance = Math.Clamp(value, 0, maxDistance);
        UpdateOffsets();
    }

    private void UpdateOffsets()
    {
        var indicatorHeight = Math.Max(IndicatorHeight, 0);
        IndicatorVisibleHeight = Math.Min(PullDistance, indicatorHeight);
        ContentOffset = PullDistance;
        _contentTransform.Y = ContentOffset;
    }

    private void UpdateStatusState()
    {
        if (IsRefreshing)
        {
            ApplyStatus(PullDownRefreshStatus.Refreshing, RefreshingText, " ", true);
            return;
        }

        if (_isCompleting)
        {
            ApplyStatus(PullDownRefreshStatus.Complete, CompleteText, "✓", false);
            return;
        }

        if (PullDistance >= Threshold && PullDistance > 0)
        {
            ApplyStatus(PullDownRefreshStatus.ReleaseToRefresh, ReleaseText, "↑", false);
            return;
        }

        if (PullDistance > 0)
        {
            ApplyStatus(PullDownRefreshStatus.Pulling, PullText, "↓", false);
            return;
        }

        ApplyStatus(PullDownRefreshStatus.Idle, PullText, "↓", false);
    }

    private void ApplyStatus(PullDownRefreshStatus status, string text, string glyph, bool showLoading)
    {
        Status = status;
        StatusText = text;
        StatusGlyph = glyph;
        ShowLoadingIndicator = showLoading;
        ShowStatusGlyph = !showLoading;

        PseudoClasses.Set(":pulling", status == PullDownRefreshStatus.Pulling);
        PseudoClasses.Set(":ready", status == PullDownRefreshStatus.ReleaseToRefresh);
        PseudoClasses.Set(":refreshing", status == PullDownRefreshStatus.Refreshing);
        PseudoClasses.Set(":complete", status == PullDownRefreshStatus.Complete);
    }

    private bool IsAtTop()
    {
        return _scrollViewer is not null && _scrollViewer.Offset.Y <= 0.5d;
    }

    private double ApplyResistance(double distance)
    {
        if (distance <= 0)
        {
            return 0;
        }

        var threshold = Math.Max(Threshold, 1d);
        if (distance <= threshold)
        {
            return distance * 0.75d;
        }

        var extra = distance - threshold;
        return (threshold * 0.75d) + (extra * 0.35d);
    }

    private void DetachScrollViewer()
    {
        if (_scrollViewer is not null)
        {
            _scrollViewer.PointerPressed -= OnScrollViewerPointerPressed;
            _scrollViewer.PointerMoved -= OnScrollViewerPointerMoved;
            _scrollViewer.PointerReleased -= OnScrollViewerPointerReleased;
            _scrollViewer.PointerCaptureLost -= OnScrollViewerPointerCaptureLost;
        }

        _scrollViewer = null;
    }
}
