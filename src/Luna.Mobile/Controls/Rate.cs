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
    Right,
    Top,
    Bottom,
    None,
}

/// <summary>
/// 评分值变更时的路由事件参数。
/// </summary>
public sealed class RateValueChangedEventArgs : RoutedEventArgs
{
    public RateValueChangedEventArgs(RoutedEvent routedEvent, double oldValue, double newValue) : base(routedEvent)
    {
        OldValue = oldValue;
        NewValue = newValue;
    }

    public double OldValue { get; }
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

    public static readonly RoutedEvent<RateValueChangedEventArgs> ValueChangedEvent =
        RoutedEvent.Register<Rate, RateValueChangedEventArgs>(nameof(ValueChanged), RoutingStrategies.Bubble);

    public static readonly StyledProperty<int> CountProperty =
        AvaloniaProperty.Register<Rate, int>(nameof(Count), 5);

    public static readonly StyledProperty<double> ValueProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Value));

    public static readonly StyledProperty<bool> AllowHalfProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowHalf));

    public static readonly StyledProperty<bool> AllowClearProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(AllowClear), true);

    public static readonly StyledProperty<bool> ReadOnlyProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(ReadOnly));

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Size), 24);

    public static readonly StyledProperty<double> SpacingProperty =
        AvaloniaProperty.Register<Rate, double>(nameof(Spacing), 8);

    public static readonly StyledProperty<IBrush?> ActiveBrushProperty =
        AvaloniaProperty.Register<Rate, IBrush?>(nameof(ActiveBrush));

    public static readonly StyledProperty<IBrush?> InactiveBrushProperty =
        AvaloniaProperty.Register<Rate, IBrush?>(nameof(InactiveBrush));

    public static readonly StyledProperty<Geometry?> IconGeometryProperty =
        AvaloniaProperty.Register<Rate, Geometry?>(nameof(IconGeometry));

    public static readonly StyledProperty<IReadOnlyList<string>?> TextsProperty =
        AvaloniaProperty.Register<Rate, IReadOnlyList<string>?>(nameof(Texts));

    public static readonly StyledProperty<bool> ShowTextProperty =
        AvaloniaProperty.Register<Rate, bool>(nameof(ShowText));

    public static readonly StyledProperty<RateTextPlacement> PlacementProperty =
        AvaloniaProperty.Register<Rate, RateTextPlacement>(nameof(Placement), RateTextPlacement.Right);

    public static readonly DirectProperty<Rate, string?> CurrentTextProperty =
        AvaloniaProperty.RegisterDirect<Rate, string?>(
            nameof(CurrentText),
            o => o.CurrentText);

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

    public Rate()
    {
        UpdateCurrentText();
        UpdatePlacementPseudo(Placement);
    }

    public event EventHandler<RateValueChangedEventArgs>? ValueChanged
    {
        add => AddHandler(ValueChangedEvent, value);
        remove => RemoveHandler(ValueChangedEvent, value);
    }

    public int Count
    {
        get => GetValue(CountProperty);
        set => SetValue(CountProperty, value);
    }

    public double Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public bool AllowHalf
    {
        get => GetValue(AllowHalfProperty);
        set => SetValue(AllowHalfProperty, value);
    }

    public bool AllowClear
    {
        get => GetValue(AllowClearProperty);
        set => SetValue(AllowClearProperty, value);
    }

    public bool ReadOnly
    {
        get => GetValue(ReadOnlyProperty);
        set => SetValue(ReadOnlyProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public double Spacing
    {
        get => GetValue(SpacingProperty);
        set => SetValue(SpacingProperty, value);
    }

    public IBrush? ActiveBrush
    {
        get => GetValue(ActiveBrushProperty);
        set => SetValue(ActiveBrushProperty, value);
    }

    public IBrush? InactiveBrush
    {
        get => GetValue(InactiveBrushProperty);
        set => SetValue(InactiveBrushProperty, value);
    }

    public Geometry? IconGeometry
    {
        get => GetValue(IconGeometryProperty);
        set => SetValue(IconGeometryProperty, value);
    }

    public IReadOnlyList<string>? Texts
    {
        get => GetValue(TextsProperty);
        set => SetValue(TextsProperty, value);
    }

    public bool ShowText
    {
        get => GetValue(ShowTextProperty);
        set => SetValue(ShowTextProperty, value);
    }

    public RateTextPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public string? CurrentText
    {
        get => _currentText;
        private set => SetAndRaise(CurrentTextProperty, ref _currentText, value);
    }

    public bool HasText
    {
        get => _hasText;
        private set => SetAndRaise(HasTextProperty, ref _hasText, value);
    }

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
