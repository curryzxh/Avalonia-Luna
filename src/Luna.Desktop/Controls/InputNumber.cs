using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Luna.Desktop.Controls;

[TemplatePart(PART_TextBox, typeof(TextBox))]
[TemplatePart(PART_SpinnerUp, typeof(Button))]
[TemplatePart(PART_SpinnerDown, typeof(Button))]
public class InputNumber : TemplatedControl
{
    public const string PART_TextBox = "PART_TextBox";
    public const string PART_SpinnerUp = "PART_SpinnerUp";
    public const string PART_SpinnerDown = "PART_SpinnerDown";

    private TextBox? _textBox;
    private Button? _spinnerUp;
    private Button? _spinnerDown;

    public static readonly StyledProperty<double?> ValueProperty =
        AvaloniaProperty.Register<InputNumber, double?>(nameof(Value), defaultBindingMode: BindingMode.TwoWay);

    public double? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public static readonly StyledProperty<double> MinimumProperty =
        AvaloniaProperty.Register<InputNumber, double>(nameof(Minimum), double.MinValue);

    public double Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    public static readonly StyledProperty<double> MaximumProperty =
        AvaloniaProperty.Register<InputNumber, double>(nameof(Maximum), double.MaxValue);

    public double Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    public static readonly StyledProperty<double> StepProperty =
        AvaloniaProperty.Register<InputNumber, double>(nameof(Step), 1.0);

    public double Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    public static readonly StyledProperty<string?> PlaceholderTextProperty =
        AvaloniaProperty.Register<InputNumber, string?>(nameof(PlaceholderText));

    public string? PlaceholderText
    {
        get => GetValue(PlaceholderTextProperty);
        set => SetValue(PlaceholderTextProperty, value);
    }

    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<InputNumber, bool>(nameof(IsReadOnly));

    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly StyledProperty<int> DecimalPlacesProperty =
        AvaloniaProperty.Register<InputNumber, int>(nameof(DecimalPlaces));

    public int DecimalPlaces
    {
        get => GetValue(DecimalPlacesProperty);
        set => SetValue(DecimalPlacesProperty, value);
    }

    static InputNumber()
    {
        FocusableProperty.OverrideDefaultValue<InputNumber>(true);
        ValueProperty.Changed.AddClassHandler<InputNumber>((x, _) => x.OnValueChanged());
        MaximumProperty.Changed.AddClassHandler<InputNumber>((x, _) => x.ClampValue());
        MinimumProperty.Changed.AddClassHandler<InputNumber>((x, _) => x.ClampValue());
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _textBox = e.NameScope.Find<TextBox>(PART_TextBox);
        _spinnerUp = e.NameScope.Find<Button>(PART_SpinnerUp);
        _spinnerDown = e.NameScope.Find<Button>(PART_SpinnerDown);

        if (_spinnerUp != null)
            _spinnerUp.Click += OnSpinnerUpClick;
        if (_spinnerDown != null)
            _spinnerDown.Click += OnSpinnerDownClick;
        if (_textBox != null)
        {
            _textBox.LostFocus += OnTextBoxLostFocus;
            _textBox.KeyDown += OnTextBoxKeyDown;
        }
        SyncTextBox();
    }

    private void OnSpinnerUpClick(object? sender, RoutedEventArgs e)
    {
        if (IsReadOnly) return;
        var newVal = (Value ?? Minimum) + Step;
        if (newVal > Maximum) newVal = Maximum;
        SetCurrentValue(ValueProperty, newVal);
    }

    private void OnSpinnerDownClick(object? sender, RoutedEventArgs e)
    {
        if (IsReadOnly) return;
        var newVal = (Value ?? Maximum) - Step;
        if (newVal < Minimum) newVal = Minimum;
        SetCurrentValue(ValueProperty, newVal);
    }

    private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        CommitInput();
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            CommitInput();
            e.Handled = true;
        }
        else if (e.Key == Key.Up)
        {
            OnSpinnerUpClick(null, null!);
            e.Handled = true;
        }
        else if (e.Key == Key.Down)
        {
            OnSpinnerDownClick(null, null!);
            e.Handled = true;
        }
    }

    private void CommitInput()
    {
        if (_textBox == null) return;
        var text = _textBox.Text?.Trim();
        if (string.IsNullOrEmpty(text))
        {
            SetCurrentValue(ValueProperty, null);
            return;
        }
        if (double.TryParse(text, NumberStyles.Any, CultureInfo.CurrentCulture, out var parsed))
        {
            parsed = Math.Clamp(parsed, Minimum, Maximum);
            SetCurrentValue(ValueProperty, parsed);
        }
        else
        {
            SyncTextBox();
        }
    }

    private void OnValueChanged()
    {
        SyncTextBox();
        UpdateSpinStates();
    }

    private void SyncTextBox()
    {
        if (_textBox == null) return;
        var text = Value.HasValue
            ? (DecimalPlaces > 0
                ? Math.Round(Value.Value, DecimalPlaces).ToString($"F{DecimalPlaces}", CultureInfo.CurrentCulture)
                : Value.Value.ToString(CultureInfo.CurrentCulture))
            : string.Empty;
        if (_textBox.Text != text)
        {
            _textBox.Text = text;
            _textBox.CaretIndex = text?.Length ?? 0;
        }
    }

    private void ClampValue()
    {
        if (Value.HasValue)
        {
            var clamped = Math.Clamp(Value.Value, Minimum, Maximum);
            if (Math.Abs(clamped - Value.Value) > double.Epsilon)
                SetCurrentValue(ValueProperty, clamped);
        }
    }

    private void UpdateSpinStates()
    {
        if (_spinnerUp != null)
            _spinnerUp.IsEnabled = !IsReadOnly && (!Value.HasValue || Value.Value < Maximum);
        if (_spinnerDown != null)
            _spinnerDown.IsEnabled = !IsReadOnly && (!Value.HasValue || Value.Value > Minimum);
    }
}
