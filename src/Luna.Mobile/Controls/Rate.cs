using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// 评分文本的显示位置。
/// </summary>
public enum RateTextPlacement
{
    /// <summary>
    /// 文本显示在右侧。
    /// </summary>
    Right,

    /// <summary>
    /// 文本显示在上方。
    /// </summary>
    Top,

    /// <summary>
    /// 文本显示在下方。
    /// </summary>
    Bottom,

    /// <summary>
    /// 不显示文本。
    /// </summary>
    None,
}

/// <summary>
/// 评分值变更时的路由事件参数。
/// </summary>
public sealed class RateValueChangedEventArgs : RoutedEventArgs
{
    /// <summary>
    /// 使用评分变化信息初始化事件参数。
    /// </summary>
    /// <param name="routedEvent">关联的路由事件。</param>
    /// <param name="oldValue">变化前的评分值。</param>
    /// <param name="newValue">变化后的评分值。</param>
    public RateValueChangedEventArgs(RoutedEvent routedEvent, double oldValue, double newValue) : base(routedEvent)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }

    /// <summary>
    /// 获取评分变更前的值。
    /// </summary>
    public double OldValue { get; }

    /// <summary>
    /// 获取评分变更后的值。
    /// </summary>
    public double NewValue { get; }
}

/// <summary>
/// 评分控件，用于选择 0~N 的评分（支持半星与清空）。
/// </summary>
/// <remarks>
/// 伪类：:text-right/:text-top/:text-bottom/:text-none。
/// </remarks>
public sealed class Rate : TemplatedControl
{
    private bool _isDragging;

    /// <inheritdoc cref="ValueChanged" />
    public static readonly RoutedEvent<RateValueChangedEventArgs> ValueChangedEvent =
        RoutedEvent.Register<Rate, RateValueChangedEventArgs>(nameof(ValueChanged), RoutingStrategies.Bubble);

    /// <inheritdoc cref="Count" />
    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Count), 5);

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Value));

    /// <inheritdoc cref="AllowHalf" />
    public static readonly StyledProperty<bool> AllowHalfProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowHalf));

    /// <inheritdoc cref="AllowClear" />
    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowClear), true);

    /// <inheritdoc cref="ReadOnly" />
    public static readonly StyledProperty<bool> ReadOnlyProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(ReadOnly));

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Size), 24);

    /// <inheritdoc cref="Spacing" />
    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Spacing), 8);

    /// <inheritdoc cref="ActiveBrush" />
    public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
        AvaloniaProperty.Register<Rate, IBrush?>(nameof(ActiveBrush));

    /// <inheritdoc cref="InactiveBrush" />
    public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
        AvaloniaProperty.Register<Rate, IBrush?>(nameof(InactiveBrush));

    /// <inheritdoc cref="IconGeometry" />
    public static readonly StyledProperty<Geometry?> IconGeometryProperty =
        AvaloniaProperty.Register<Rate, Geometry?>(nameof(IconGeometry));

    /// <inheritdoc cref="Texts" />
    public static readonly StyledProperty<IReadOnlyList<string>?> TextsProperty =
        AvaloniaProperty.Register<Rate, IReadOnlyList<string>?>(nameof(Texts));

    /// <inheritdoc cref="ShowText" />
    public static readonly StyledProperty<bool> ShowTextProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(ShowText));

    /// <inheritdoc cref="Placement" />
    public static readonly StyledProperty<RateTextPlacement> PlacementProperty =
        AvaloniaProperty.Register<Rate, RateTextPlacement>(nameof(Placement), RateTextPlacement.Right);

    /// <inheritdoc cref="CurrentText" />
    public static readonly DirectProperty<Rate, string?> CurrentTextProperty =
        AvaloniaProperty.RegisterDirect<Rate, string?>(
            nameof(CurrentText),
            o => o.CurrentText);

    /// <inheritdoc cref="HasText" />
    public static readonly DirectProperty<Rate, bool> HasTextProperty =
        AvaloniaProperty.RegisterDirect<Rate, bool>(
            nameof(HasText),
            o => o.HasText);

    private string? _currentText;
    private bool _hasText;

    static Rate()
    {
        CountProperty.Changed.AddClassHandler<Rate>((control, _) => control.InvalidateMeasure());
        ValueProperty.Changed.AddClassHandler<Rate>((control, args) =>
        {
            control.UpdateCurrentText();
            control.InvalidateVisual();
            control.RaiseEvent(new RateValueChangedEventArgs(ValueChangedEvent, args.GetOldValue<double>(), args.GetNewValue<double>()));
        });
        TextsProperty.Changed.AddClassHandler<Rate>((control, _) => control.UpdateCurrentText());
        ShowTextProperty.Changed.AddClassHandler<Rate>((control, _) => control.UpdateCurrentText());
        PlacementProperty.Changed.AddClassHandler<Rate>((control, args) => control.UpdatePlacementPseudo(args.GetNewValue<RateTextPlacement>()));
    }

    /// <summary>
    /// 初始化 <see cref="Rate" /> 的新实例。
    /// </summary>
    public Rate()
    {
        UpdateCurrentText();
        UpdatePlacementPseudo(Placement);
    }

    /// <summary>
    /// 当评分值因用户交互变化时触发。
    /// </summary>
    public event EventHandler<RateValueChangedEventArgs>? ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    /// <summary>
    /// 获取或设置图标总数量。
    /// </summary>
    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    /// <summary>
    /// 获取或设置当前评分值。
    /// </summary>
    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 获取或设置是否允许半选。
    /// </summary>
    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    /// <summary>
    /// 获取或设置点击当前值时是否允许清空。
    /// </summary>
    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    /// <summary>
    /// 获取或设置是否只读。
    /// </summary>
    public bool ReadOnly
    {
        get => GetValue(ReadOnlyProperty);
        set => SetValue(ReadOnlyProperty, value);
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
    /// 获取或设置图标之间的间距。
    /// </summary>
    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    /// <summary>
    /// 获取或设置选中状态的画刷。
    /// </summary>
    public IBrush? ActiveBrush
    {
        get => GetValue(ActiveBrushProperty);
        set => SetValue(ActiveBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置未选中状态的画刷。
    /// </summary>
    public IBrush? InactiveBrush
    {
        get => GetValue(InactiveBrushProperty);
        set => SetValue(InactiveBrushProperty, value);
    }

    /// <summary>
    /// 获取或设置自定义评分图标几何。
    /// </summary>
    public Geometry? IconGeometry
    {
        get => GetValue(IconGeometryProperty);
        set => SetValue(IconGeometryProperty, value);
    }

    /// <summary>
    /// 获取或设置评分文本映射列表。
    /// </summary>
    public IReadOnlyList<string>? Texts
    {
        get => GetValue(TextsProperty);
        set => SetValue(TextsProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示评分文本。
    /// </summary>
    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    /// <summary>
    /// 获取或设置评分文本显示位置。
    /// </summary>
    public RateTextPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// 获取当前展示的评分文本。
    /// </summary>
    public string? CurrentText
    {
        get => _currentText;
        private set => SetAndRaise(CurrentTextProperty, ref _currentText, value);
    }

    /// <summary>
    /// 获取当前是否存在可显示的评分文本。
    /// </summary>
    public bool HasText
    {
        get => _hasText;
        private set => SetAndRaise(HasTextProperty, ref _hasText, value);
    }

    /// <inheritdoc />
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (!IsEnabled || ReadOnly)
        {
            return;
        }

        _isDragging = true;
        e.Pointer.Capture(this);
        UpdateValueFromPointer(e.GetPosition(this), true);
        e.Handled = true;
    }

    /// <inheritdoc />
    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (!_isDragging || !IsEnabled || ReadOnly)
        {
            return;
        }

        UpdateValueFromPointer(e.GetPosition(this), false);
        e.Handled = true;
    }

    /// <inheritdoc />
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        if (!_isDragging)
        {
            return;
        }

        _isDragging = false;
        e.Pointer.Capture(null);
        e.Handled = true;
    }

    private void UpdateValueFromPointer(Point p, bool allowClearOnSame)
    {
        var count = Math.Max(0, Count);
        if (count == 0)
        {
            return;
        }

        var size = Math.Max(0, Size);
        var spacing = Math.Max(0, Spacing);
        var total = count * size + (count - 1) * spacing;
        var x = Math.Clamp(p.X, 0, total);
        var raw = x / (size + spacing);
        var index = (int)Math.Floor(raw);
        var local = x - index * (size + spacing);

        var next = 0.0;
        if (index >= count)
        {
            next = count;
        }
        else
        {
            if (AllowHalf)
            {
                next = index + (local <= size / 2 ? 0.5 : 1.0);
            }
            else
            {
                next = index + 1.0;
            }
        }

        next = Math.Clamp(next, 0, count);
        if (AllowClear && allowClearOnSame && Math.Abs(next - Value) < 0.0001)
        {
            next = 0;
        }

        Value = next;
    }

    private void UpdateCurrentText()
    {
        if (!ShowText)
        {
            CurrentText = null;
            HasText = false;
            return;
        }

        var v = (int)Math.Round(Math.Clamp(Value, 0, Count));
        if (v <= 0)
        {
            CurrentText = null;
            HasText = false;
            return;
        }

        var texts = Texts;
        if (texts is null || texts.Count == 0)
        {
            CurrentText = v.ToString();
            HasText = true;
            return;
        }

        var idx = Math.Clamp(v - 1, 0, texts.Count - 1);
        CurrentText = texts[idx];
        HasText = true;
    }

    private void UpdatePlacementPseudo(RateTextPlacement placement)
    {
        PseudoClasses.Set(":placement-right", placement == RateTextPlacement.Right);
        PseudoClasses.Set(":placement-top", placement == RateTextPlacement.Top);
        PseudoClasses.Set(":placement-bottom", placement == RateTextPlacement.Bottom);
        PseudoClasses.Set(":placement-none", placement == RateTextPlacement.None);
    }
}
