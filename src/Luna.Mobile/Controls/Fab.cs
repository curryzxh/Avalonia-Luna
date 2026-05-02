using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// Fab 的拖拽方向。
/// </summary>
public enum FabDragMode
{
    /// <summary>
    /// 禁止拖拽。
    /// </summary>
    None,

    /// <summary>
    /// 允许任意方向拖拽。
    /// </summary>
    All,

    /// <summary>
    /// 仅允许垂直方向拖拽。
    /// </summary>
    Vertical,

    /// <summary>
    /// 仅允许水平方向拖拽。
    /// </summary>
    Horizontal,
}

/// <summary>
/// 悬浮操作按钮，支持图标、文本和拖拽定位。
/// </summary>
/// <remarks>
/// 伪类：:extended、:icon-only、:dragging。
/// </remarks>
[TemplatePart(ContainerPartName, typeof(Border))]
public sealed class Fab : Button
{
    private const string ContainerPartName = "PART_Container";
    private const double DragThreshold = 4d;

    private readonly TranslateTransform _translateTransform = new();
    private Border? _container;
    private bool _isPointerPressed;
    private bool _isDragging;
    private bool _suppressClick;
    private Point _pressedPointInParent;
    private Vector _pressedOffset;
    private bool _hasText;
    private bool _hasIcon;

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Fab, object?>(nameof(Icon));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Fab, string?>(nameof(Text));

    public static readonly StyledProperty<FabDragMode> DragModeProperty =
        AvaloniaProperty.Register<Fab, FabDragMode>(nameof(DragMode));

    public static readonly DirectProperty<Fab, bool> HasTextProperty =
        AvaloniaProperty.RegisterDirect<Fab, bool>(
            nameof(HasText),
            o => o.HasText);

    public static readonly DirectProperty<Fab, bool> HasIconProperty =
        AvaloniaProperty.RegisterDirect<Fab, bool>(
            nameof(HasIcon),
            o => o.HasIcon);

    static Fab()
    {
        FocusableProperty.OverrideDefaultValue<Fab>(true);
        HorizontalAlignmentProperty.OverrideDefaultValue<Fab>(Avalonia.Layout.HorizontalAlignment.Right);
        VerticalAlignmentProperty.OverrideDefaultValue<Fab>(Avalonia.Layout.VerticalAlignment.Bottom);

        IconProperty.Changed.AddClassHandler<Fab>((control, _) => control.UpdateState());
        TextProperty.Changed.AddClassHandler<Fab>((control, _) => control.UpdateState());
        DragModeProperty.Changed.AddClassHandler<Fab>((control, _) => control.UpdateState());
    }

    public Fab()
    {
        RenderTransform = _translateTransform;
        UpdateState();
    }

    /// <summary>
    /// 获取或设置 Fab 的图标内容。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取或设置 Fab 的文本内容。
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 获取或设置 Fab 的拖拽方向。
    /// </summary>
    public FabDragMode DragMode
    {
        get => GetValue(DragModeProperty);
        set => SetValue(DragModeProperty, value);
    }

    /// <summary>
    /// 获取当前是否包含文本。
    /// </summary>
    public bool HasText
    {
        get => _hasText;
        private set => SetAndRaise(HasTextProperty, ref _hasText, value);
    }

    /// <summary>
    /// 获取当前是否包含图标。
    /// </summary>
    public bool HasIcon
    {
        get => _hasIcon;
        private set => SetAndRaise(HasIconProperty, ref _hasIcon, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _container = e.NameScope.Find<Border>(ContainerPartName);
        UpdateState();
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);

        if (!IsEnabled || DragMode == FabDragMode.None || !e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
        {
            return;
        }

        if (this.GetVisualParent() is not Visual parent)
        {
            return;
        }

        _isPointerPressed = true;
        _pressedPointInParent = e.GetPosition(parent);
        _pressedOffset = new Vector(_translateTransform.X, _translateTransform.Y);
        _suppressClick = false;
        e.Pointer.Capture(this);
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);

        if (!_isPointerPressed || this.GetVisualParent() is not Visual parent)
        {
            return;
        }

        var currentPoint = e.GetPosition(parent);
        var delta = currentPoint - _pressedPointInParent;

        if (!_isDragging)
        {
            if (Math.Abs(delta.X) < DragThreshold && Math.Abs(delta.Y) < DragThreshold)
            {
                return;
            }

            _isDragging = true;
            _suppressClick = true;
            PseudoClasses.Set(":dragging", true);
        }

        var targetX = _pressedOffset.X;
        var targetY = _pressedOffset.Y;

        if (DragMode is FabDragMode.All or FabDragMode.Horizontal)
        {
            targetX += delta.X;
        }

        if (DragMode is FabDragMode.All or FabDragMode.Vertical)
        {
            targetY += delta.Y;
        }

        var minX = -Bounds.X;
        var maxX = Math.Max(minX, parent.Bounds.Width - Bounds.Width - Bounds.X);
        var minY = -Bounds.Y;
        var maxY = Math.Max(minY, parent.Bounds.Height - Bounds.Height - Bounds.Y);

        _translateTransform.X = Math.Clamp(targetX, minX, maxX);
        _translateTransform.Y = Math.Clamp(targetY, minY, maxY);
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        FinishPointerInteraction(e.Pointer);
    }

    protected override void OnPointerCaptureLost(PointerCaptureLostEventArgs e)
    {
        base.OnPointerCaptureLost(e);
        FinishPointerInteraction(null);
    }

    protected override void OnClick()
    {
        if (_suppressClick)
        {
            _suppressClick = false;
            return;
        }

        base.OnClick();
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

    private void UpdateState()
    {
        HasText = !string.IsNullOrWhiteSpace(Text);
        HasIcon = Icon is not null;

        PseudoClasses.Set(":extended", HasText);
        PseudoClasses.Set(":icon-only", !HasText);
        PseudoClasses.Set(":dragging", _isDragging);
    }
}
