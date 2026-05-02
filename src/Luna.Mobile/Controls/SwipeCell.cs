using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Presenters;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// SwipeCell 的打开方向。
/// </summary>
public enum SwipeCellOpenMode
{
    /// <summary>
    /// 关闭状态。
    /// </summary>
    None,

    /// <summary>
    /// 打开左侧操作区。
    /// </summary>
    Left,

    /// <summary>
    /// 打开右侧操作区。
    /// </summary>
    Right,
}

/// <summary>
/// SwipeCell 打开状态变化事件参数。
/// </summary>
public sealed class SwipeCellOpenModeChangedEventArgs : EventArgs
{
    /// <summary>
    /// 初始化 <see cref="SwipeCellOpenModeChangedEventArgs"/> 的新实例。
    /// </summary>
    public SwipeCellOpenModeChangedEventArgs(SwipeCellOpenMode oldMode, SwipeCellOpenMode newMode)
    {
        OldMode = oldMode;
        NewMode = newMode;
    }

    /// <summary>
    /// 获取变化前的状态。
    /// </summary>
    public SwipeCellOpenMode OldMode { get; }

    /// <summary>
    /// 获取变化后的状态。
    /// </summary>
    public SwipeCellOpenMode NewMode { get; }
}

/// <summary>
/// 滑动单元格，支持左右滑出操作区。
/// </summary>
[TemplatePart(ContentHostPartName, typeof(Border))]
[TemplatePart(LeftPresenterPartName, typeof(ContentPresenter))]
[TemplatePart(RightPresenterPartName, typeof(ContentPresenter))]
public sealed class SwipeCell : ContentControl
{
    private const string ContentHostPartName = "PART_ContentHost";
    private const string LeftPresenterPartName = "PART_LeftPresenter";
    private const string RightPresenterPartName = "PART_RightPresenter";
    private const double DragThreshold = 6d;
    private static readonly List<WeakReference<SwipeCell>> Instances = [];

    private readonly TranslateTransform _contentTransform = new();
    private Border? _contentHost;
    private ContentPresenter? _leftPresenter;
    private ContentPresenter? _rightPresenter;
    private bool _isPointerPressed;
    private bool _isDragging;
    private Point _pressedPoint;
    private double _pressedOffset;
    private double _currentOffset;
    private bool _hasLeft;
    private bool _hasRight;
    private bool _isOpen;

    /// <inheritdoc cref="Left" />
    public static readonly StyledProperty<object?> LeftProperty =
        AvaloniaProperty.Register<SwipeCell, object?>(nameof(Left));

    /// <inheritdoc cref="Right" />
    public static readonly StyledProperty<object?> RightProperty =
        AvaloniaProperty.Register<SwipeCell, object?>(nameof(Right));

    /// <inheritdoc cref="OpenMode" />
    public static readonly StyledProperty<SwipeCellOpenMode> OpenModeProperty =
        AvaloniaProperty.Register<SwipeCell, SwipeCellOpenMode>(nameof(OpenMode));

    /// <inheritdoc cref="OpenThresholdRatio" />
    public static readonly StyledProperty<double> OpenThresholdRatioProperty =
        AvaloniaProperty.Register<SwipeCell, double>(nameof(OpenThresholdRatio), 0.5);

    /// <inheritdoc cref="HasLeft" />
    public static readonly DirectProperty<SwipeCell, bool> HasLeftProperty =
        AvaloniaProperty.RegisterDirect<SwipeCell, bool>(
            nameof(HasLeft),
            o => o.HasLeft);

    /// <inheritdoc cref="HasRight" />
    public static readonly DirectProperty<SwipeCell, bool> HasRightProperty =
        AvaloniaProperty.RegisterDirect<SwipeCell, bool>(
            nameof(HasRight),
            o => o.HasRight);

    /// <inheritdoc cref="IsOpen" />
    public static readonly DirectProperty<SwipeCell, bool> IsOpenProperty =
        AvaloniaProperty.RegisterDirect<SwipeCell, bool>(
            nameof(IsOpen),
            o => o.IsOpen);

    static SwipeCell()
    {
        ClipToBoundsProperty.OverrideDefaultValue<SwipeCell>(true);
        LeftProperty.Changed.AddClassHandler<SwipeCell>((control, _) => control.UpdateState());
        RightProperty.Changed.AddClassHandler<SwipeCell>((control, _) => control.UpdateState());
        OpenModeProperty.Changed.AddClassHandler<SwipeCell>((control, args) => control.HandleOpenModeChanged(args));
    }

    /// <summary>
    /// 获取或设置左侧操作内容。
    /// </summary>
    public object? Left
    {
        get => GetValue(LeftProperty);
        set => SetValue(LeftProperty, value);
    }

    /// <summary>
    /// 获取或设置右侧操作内容。
    /// </summary>
    public object? Right
    {
        get => GetValue(RightProperty);
        set => SetValue(RightProperty, value);
    }

    /// <summary>
    /// 获取或设置当前打开方向。
    /// </summary>
    public SwipeCellOpenMode OpenMode
    {
        get => GetValue(OpenModeProperty);
        set => SetValue(OpenModeProperty, value);
    }

    /// <summary>
    /// 获取或设置吸附阈值比例。
    /// </summary>
    public double OpenThresholdRatio
    {
        get => GetValue(OpenThresholdRatioProperty);
        set => SetValue(OpenThresholdRatioProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在左侧操作区。
    /// </summary>
    public bool HasLeft
    {
        get => _hasLeft;
        private set => SetAndRaise(HasLeftProperty, ref _hasLeft, value);
    }

    /// <summary>
    /// 获取当前是否存在右侧操作区。
    /// </summary>
    public bool HasRight
    {
        get => _hasRight;
        private set => SetAndRaise(HasRightProperty, ref _hasRight, value);
    }

    /// <summary>
    /// 获取当前是否处于打开状态。
    /// </summary>
    public bool IsOpen
    {
        get => _isOpen;
        private set => SetAndRaise(IsOpenProperty, ref _isOpen, value);
    }

    /// <summary>
    /// 当打开方向变化时触发。
    /// </summary>
    public event EventHandler<SwipeCellOpenModeChangedEventArgs>? OpenModeChanged;

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _contentHost = e.NameScope.Find<Border>(ContentHostPartName);
        _leftPresenter = e.NameScope.Find<ContentPresenter>(LeftPresenterPartName);
        _rightPresenter = e.NameScope.Find<ContentPresenter>(RightPresenterPartName);

        if (_contentHost is not null)
        {
            _contentHost.RenderTransform = _contentTransform;
        }

        UpdateState();
        ApplyOpenMode();
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        Instances.Add(new WeakReference<SwipeCell>(this));
        UpdateState();
        ApplyOpenMode();
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
    protected override Size ArrangeOverride(Size finalSize)
    {
        var arranged = base.ArrangeOverride(finalSize);
        if (!_isDragging)
        {
            ApplyOpenMode();
        }

        return arranged;
    }

    /// <inheritdoc />
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!IsEnabled || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        var point = e.GetPosition(this);
        if (IsPointOnExposedActionArea(point))
        {
            return;
        }

        CloseOtherInstances(this);
        _isPointerPressed = true;
        _pressedPoint = point;
        _pressedOffset = _currentOffset;
    }

    /// <inheritdoc />
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isPointerPressed)
        {
            return;
        }

        var point = e.GetPosition(this);
        var delta = point - _pressedPoint;

        if (!_isDragging)
        {
            if (Math.Abs(delta.X) < DragThreshold || Math.Abs(delta.X) <= Math.Abs(delta.Y))
            {
                return;
            }

            _isDragging = true;
            PseudoClasses.Set(":dragging", true);
            e.Pointer.Capture(this);
        }

        var target = Math.Clamp(_pressedOffset + delta.X, -GetRightWidth(), GetLeftWidth());
        SetContentOffset(target);
        e.Handled = true;
    }

    /// <inheritdoc />
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (!_isPointerPressed)
        {
            return;
        }

        if (_isDragging)
        {
            SettleOpenState();
            e.Handled = true;
        }
        else if (OpenMode != SwipeCellOpenMode.None)
        {
            Close();
            e.Handled = true;
        }

        FinishPointerInteraction(e.Pointer);
    }

    /// <inheritdoc />
    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        FinishPointerInteraction(null);

        if (!_isDragging)
        {
            ApplyOpenMode();
        }
    }

    /// <summary>
    /// 打开左侧操作区。
    /// </summary>
    public void OpenLeft()
    {
        if (HasLeft)
        {
            OpenMode = SwipeCellOpenMode.Left;
        }
    }

    /// <summary>
    /// 打开右侧操作区。
    /// </summary>
    public void OpenRight()
    {
        if (HasRight)
        {
            OpenMode = SwipeCellOpenMode.Right;
        }
    }

    /// <summary>
    /// 关闭操作区。
    /// </summary>
    public void Close()
    {
        OpenMode = SwipeCellOpenMode.None;
    }

    private void FinishPointerInteraction(IPointer? pointer)
    {
        if (pointer?.Captured == this)
        {
            pointer.Capture(null);
        }

        _isPointerPressed = false;
        _isDragging = false;
        PseudoClasses.Set(":dragging", false);
    }

    private void SettleOpenState()
    {
        var leftWidth = GetLeftWidth();
        var rightWidth = GetRightWidth();
        var ratio = Math.Clamp(OpenThresholdRatio, 0.1, 0.9);

        if (_currentOffset > 0 && leftWidth > 0)
        {
            OpenMode = _currentOffset >= leftWidth * ratio ? SwipeCellOpenMode.Left : SwipeCellOpenMode.None;
            return;
        }

        if (_currentOffset < 0 && rightWidth > 0)
        {
            OpenMode = Math.Abs(_currentOffset) >= rightWidth * ratio ? SwipeCellOpenMode.Right : SwipeCellOpenMode.None;
            return;
        }

        OpenMode = SwipeCellOpenMode.None;
    }

    private void HandleOpenModeChanged(AvaloniaPropertyChangedEventArgs e)
    {
        var oldMode = e.GetOldValue<SwipeCellOpenMode>();
        var targetMode = e.GetNewValue<SwipeCellOpenMode>();
        if (targetMode == SwipeCellOpenMode.Left && !HasLeft)
        {
            targetMode = SwipeCellOpenMode.None;
        }
        else if (targetMode == SwipeCellOpenMode.Right && !HasRight)
        {
            targetMode = SwipeCellOpenMode.None;
        }

        if (!Equals(targetMode, e.GetNewValue<SwipeCellOpenMode>()))
        {
            OpenMode = targetMode;
            return;
        }

        if (targetMode != SwipeCellOpenMode.None)
        {
            CloseOtherInstances(this);
        }

        ApplyOpenMode();
        OpenModeChanged?.Invoke(this, new SwipeCellOpenModeChangedEventArgs(oldMode, targetMode));
    }

    private void ApplyOpenMode()
    {
        var targetOffset = OpenMode switch
        {
            SwipeCellOpenMode.Left => GetLeftWidth(),
            SwipeCellOpenMode.Right => -GetRightWidth(),
            _ => 0,
        };

        SetContentOffset(targetOffset);
    }

    private void SetContentOffset(double offset)
    {
        _currentOffset = offset;
        _contentTransform.X = offset;

        IsOpen = Math.Abs(offset) > 0.5;
        PseudoClasses.Set(":left-open", offset > 0.5);
        PseudoClasses.Set(":right-open", offset < -0.5);
        PseudoClasses.Set(":open", IsOpen);
    }

    private double GetLeftWidth()
    {
        if (!HasLeft)
        {
            return 0;
        }

        return _leftPresenter is null
            ? 0
            : Math.Max(_leftPresenter.Bounds.Width, _leftPresenter.DesiredSize.Width);
    }

    private double GetRightWidth()
    {
        if (!HasRight)
        {
            return 0;
        }

        return _rightPresenter is null
            ? 0
            : Math.Max(_rightPresenter.Bounds.Width, _rightPresenter.DesiredSize.Width);
    }

    private bool IsPointOnExposedActionArea(Point point)
    {
        if (_currentOffset > 0.5)
        {
            return point.X <= _currentOffset;
        }

        if (_currentOffset < -0.5)
        {
            return point.X >= Bounds.Width + _currentOffset;
        }

        return false;
    }

    private static void CloseOtherInstances(SwipeCell current)
    {
        for (var i = Instances.Count - 1; i >= 0; i--)
        {
            if (!Instances[i].TryGetTarget(out var target))
            {
                Instances.RemoveAt(i);
                continue;
            }

            if (!ReferenceEquals(target, current) && target.OpenMode != SwipeCellOpenMode.None)
            {
                target.Close();
            }
        }
    }

    private void UpdateState()
    {
        HasLeft = Left is not null;
        HasRight = Right is not null;

        PseudoClasses.Set(":has-left", HasLeft);
        PseudoClasses.Set(":has-right", HasRight);
        PseudoClasses.Set(":open", IsOpen);

        if (OpenMode == SwipeCellOpenMode.Left && !HasLeft)
        {
            OpenMode = SwipeCellOpenMode.None;
            return;
        }

        if (OpenMode == SwipeCellOpenMode.Right && !HasRight)
        {
            OpenMode = SwipeCellOpenMode.None;
            return;
        }

        ApplyOpenMode();
    }
}
