using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// <see cref="Stepper"/> 的视觉风格预设。
/// </summary>
public enum StepperTheme
{
    /// <summary>
    /// 默认样式。
    /// </summary>
    Default,

    /// <summary>
    /// 填充背景样式。
    /// </summary>
    Filled,

    /// <summary>
    /// 描边样式。
    /// </summary>
    Outline,
}

/// <summary>
/// <see cref="Stepper"/> 的尺寸预设。
/// </summary>
public enum StepperSize
{
    /// <summary>
    /// 小尺寸。
    /// </summary>
    Small,

    /// <summary>
    /// 中尺寸。
    /// </summary>
    Medium,

    /// <summary>
    /// 大尺寸。
    /// </summary>
    Large,
}

/// <summary>
/// 当用户尝试超出 <see cref="Stepper.Minimum"/> 或 <see cref="Stepper.Maximum"/> 时触发的事件参数。
/// </summary>
public sealed class StepperOverlimitEventArgs : EventArgs
{
    /// <summary>
    /// 使用边界命中信息初始化事件参数。
    /// </summary>
    /// <param name="isMin">是否命中最小值边界。</param>
    public StepperOverlimitEventArgs(bool isMin)
    {
        IsMin = isMin;
    }

    /// <summary>
    /// 获取是否命中了最小值边界。
    /// </summary>
    public bool IsMin { get; }
}

/// <summary>
/// 整数步进器控件，用于数值增减。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_DecreaseButton：<see cref="Button"/></description></item>
/// <item><description>PART_IncreaseButton：<see cref="Button"/></description></item>
/// <item><description>PART_TextBox：<see cref="TextBox"/></description></item>
/// </list>
/// 伪类：:filled/:outline/:default、:small/:medium/:large、:min/:max、:disabled。
/// </remarks>
[TemplatePart(DecreaseButtonPartName, typeof(Button))]
[TemplatePart(IncreaseButtonPartName, typeof(Button))]
[TemplatePart(TextBoxPartName, typeof(TextBox))]
public sealed class Stepper : TemplatedControl
{
    private const string DecreaseButtonPartName = "PART_DecreaseButton";
    private const string IncreaseButtonPartName = "PART_IncreaseButton";
    private const string TextBoxPartName = "PART_TextBox";

    private Button? _decreaseButton;
    private Button? _increaseButton;
    private TextBox? _textBox;

    private bool _canDecrease;
    private bool _canIncrease;
    private bool _isTextBoxReadOnly;

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<int> ValueProperty =
        AvaloniaProperty.Register<Stepper, int>(nameof(Value), defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    /// <inheritdoc cref="Minimum" />
    public static readonly StyledProperty<int> MinimumProperty =
        AvaloniaProperty.Register<Stepper, int>(nameof(Minimum), 0);

    /// <inheritdoc cref="Maximum" />
    public static readonly StyledProperty<int> MaximumProperty =
        AvaloniaProperty.Register<Stepper, int>(nameof(Maximum), 999);

    /// <inheritdoc cref="Step" />
    public static readonly StyledProperty<int> StepProperty =
        AvaloniaProperty.Register<Stepper, int>(nameof(Step), 1);

    /// <inheritdoc cref="Theme" />
    public static readonly StyledProperty<StepperTheme> ThemeProperty =
        AvaloniaProperty.Register<Stepper, StepperTheme>(nameof(Theme), StepperTheme.Default);

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<StepperSize> SizeProperty =
        AvaloniaProperty.Register<Stepper, StepperSize>(nameof(Size), StepperSize.Medium);

    /// <inheritdoc cref="IsEditable" />
    public static readonly StyledProperty<bool> IsEditableProperty =
        AvaloniaProperty.Register<Stepper, bool>(nameof(IsEditable), true);

    /// <inheritdoc cref="IsDisabled" />
    public static readonly StyledProperty<bool> IsDisabledProperty =
        AvaloniaProperty.Register<Stepper, bool>(nameof(IsDisabled));

    /// <inheritdoc cref="CanDecrease" />
    public static readonly DirectProperty<Stepper, bool> CanDecreaseProperty =
        AvaloniaProperty.RegisterDirect<Stepper, bool>(
            nameof(CanDecrease),
            o => o.CanDecrease);

    /// <inheritdoc cref="CanIncrease" />
    public static readonly DirectProperty<Stepper, bool> CanIncreaseProperty =
        AvaloniaProperty.RegisterDirect<Stepper, bool>(
            nameof(CanIncrease),
            o => o.CanIncrease);

    /// <inheritdoc cref="IsTextBoxReadOnly" />
    public static readonly DirectProperty<Stepper, bool> IsTextBoxReadOnlyProperty =
        AvaloniaProperty.RegisterDirect<Stepper, bool>(
            nameof(IsTextBoxReadOnly),
            o => o.IsTextBoxReadOnly);

    static Stepper()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Stepper>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<Stepper>(HorizontalAlignment.Left);

        ValueProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        MinimumProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        MaximumProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        StepProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        ThemeProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        SizeProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        IsEditableProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
        IsDisabledProperty.Changed.AddClassHandler<Stepper>((o, _) => o.UpdateState());
    }

    /// <summary>
    /// <see cref="Value"/> 因用户交互发生变化时触发。
    /// </summary>
    public event EventHandler? ValueChanged;

    /// <summary>
    /// 用户交互触达最小/最大边界时触发。
    /// </summary>
    public event EventHandler<StepperOverlimitEventArgs>? Overlimit;

    /// <summary>
    /// 获取或设置当前值。
    /// </summary>
    public int Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, CoerceValue(value));
    }

    /// <summary>
    /// 获取或设置允许的最小值。
    /// </summary>
    public int Minimum
    {
        get => GetValue(MinimumProperty);
        set => SetValue(MinimumProperty, value);
    }

    /// <summary>
    /// 获取或设置允许的最大值。
    /// </summary>
    public int Maximum
    {
        get => GetValue(MaximumProperty);
        set => SetValue(MaximumProperty, value);
    }

    /// <summary>
    /// 获取或设置每次增减的步长。
    /// </summary>
    public int Step
    {
        get => GetValue(StepProperty);
        set => SetValue(StepProperty, value);
    }

    /// <summary>
    /// 获取或设置风格预设。
    /// </summary>
    public StepperTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置尺寸预设。
    /// </summary>
    public StepperSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置输入框是否允许直接编辑。
    /// </summary>
    public bool IsEditable
    {
        get => GetValue(IsEditableProperty);
        set => SetValue(IsEditableProperty, value);
    }

    /// <summary>
    /// 获取或设置控件是否禁用。
    /// </summary>
    public bool IsDisabled
    {
        get => GetValue(IsDisabledProperty);
        set => SetValue(IsDisabledProperty, value);
    }

    /// <summary>
    /// 获取当前是否允许递减。
    /// </summary>
    public bool CanDecrease
    {
        get => _canDecrease;
        private set => SetAndRaise(CanDecreaseProperty, ref _canDecrease, value);
    }

    /// <summary>
    /// 获取当前是否允许递增。
    /// </summary>
    public bool CanIncrease
    {
        get => _canIncrease;
        private set => SetAndRaise(CanIncreaseProperty, ref _canIncrease, value);
    }

    /// <summary>
    /// 获取当前输入框是否为只读。
    /// </summary>
    public bool IsTextBoxReadOnly
    {
        get => _isTextBoxReadOnly;
        private set => SetAndRaise(IsTextBoxReadOnlyProperty, ref _isTextBoxReadOnly, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachHandlers();

        _decreaseButton = e.NameScope.Find<Button>(DecreaseButtonPartName);
        _increaseButton = e.NameScope.Find<Button>(IncreaseButtonPartName);
        _textBox = e.NameScope.Find<TextBox>(TextBoxPartName);

        AttachHandlers();
        UpdateState();
    }

    private void AttachHandlers()
    {
        if (_decreaseButton is not null)
        {
            _decreaseButton.Click += OnDecreaseClick;
        }

        if (_increaseButton is not null)
        {
            _increaseButton.Click += OnIncreaseClick;
        }

        if (_textBox is not null)
        {
            _textBox.LostFocus += OnTextBoxLostFocus;
            _textBox.KeyDown += OnTextBoxKeyDown;
        }
    }

    private void DetachHandlers()
    {
        if (_decreaseButton is not null)
        {
            _decreaseButton.Click -= OnDecreaseClick;
        }

        if (_increaseButton is not null)
        {
            _increaseButton.Click -= OnIncreaseClick;
        }

        if (_textBox is not null)
        {
            _textBox.LostFocus -= OnTextBoxLostFocus;
            _textBox.KeyDown -= OnTextBoxKeyDown;
        }
    }

    private void OnDecreaseClick(object? sender, EventArgs e)
    {
        if (IsDisabled || !CanDecrease)
        {
            if (Value <= Minimum)
            {
                Overlimit?.Invoke(this, new StepperOverlimitEventArgs(true));
            }
            return;
        }

        SetValueInternal(Value - Math.Max(1, Step));
    }

    private void OnIncreaseClick(object? sender, EventArgs e)
    {
        if (IsDisabled || !CanIncrease)
        {
            if (Value >= Maximum)
            {
                Overlimit?.Invoke(this, new StepperOverlimitEventArgs(false));
            }
            return;
        }

        SetValueInternal(Value + Math.Max(1, Step));
    }

    private void OnTextBoxLostFocus(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        CommitTextBoxValue();
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key != Key.Enter)
        {
            return;
        }

        CommitTextBoxValue();
        e.Handled = true;
    }

    private void CommitTextBoxValue()
    {
        if (_textBox is null || IsTextBoxReadOnly || IsDisabled)
        {
            return;
        }

        if (!int.TryParse(_textBox.Text, out var parsed))
        {
            _textBox.Text = Value.ToString();
            return;
        }

        SetValueInternal(parsed);
    }

    private void SetValueInternal(int value)
    {
        var coerced = CoerceValue(value);
        if (coerced == Value)
        {
            UpdateState();
            return;
        }

        SetValue(ValueProperty, coerced);
        ValueChanged?.Invoke(this, EventArgs.Empty);
        UpdateState();
    }

    private int CoerceValue(int value)
    {
        if (Maximum < Minimum)
        {
            return Minimum;
        }

        if (value < Minimum)
        {
            return Minimum;
        }

        if (value > Maximum)
        {
            return Maximum;
        }

        return value;
    }

    private void UpdateState()
    {
        var min = Minimum;
        var max = Maximum;
        if (max < min)
        {
            max = min;
        }

        if (Value < min)
        {
            SetValue(ValueProperty, min);
        }
        else if (Value > max)
        {
            SetValue(ValueProperty, max);
        }

        CanDecrease = !IsDisabled && Value > min;
        CanIncrease = !IsDisabled && Value < max;
        IsTextBoxReadOnly = !IsEditable || IsDisabled;

        PseudoClasses.Set(":filled", Theme == StepperTheme.Filled);
        PseudoClasses.Set(":outline", Theme == StepperTheme.Outline);
        PseudoClasses.Set(":default", Theme == StepperTheme.Default);

        PseudoClasses.Set(":small", Size == StepperSize.Small);
        PseudoClasses.Set(":medium", Size == StepperSize.Medium);
        PseudoClasses.Set(":large", Size == StepperSize.Large);

        PseudoClasses.Set(":min", Value <= min);
        PseudoClasses.Set(":max", Value >= max);
        PseudoClasses.Set(":disabled", IsDisabled);
    }
}
