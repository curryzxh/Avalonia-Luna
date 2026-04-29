using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 定义 <see cref="SearchBar"/> 的形状。
/// </summary>
public enum SearchBarShape
{
    Default,
    Round,
}

/// <summary>
/// 移动端搜索输入控件。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_TextBox：<see cref="TextBox"/></description></item>
/// <item><description>PART_ClearButton：<see cref="Button"/></description></item>
/// <item><description>PART_CancelButton：<see cref="Button"/></description></item>
/// </list>
/// 伪类：:round/:default、:center、:focused、:has-text。
/// </remarks>
public sealed class SearchBar : TemplatedControl
{
    private const string TextBoxPartName = "PART_TextBox";
    private const string ClearButtonPartName = "PART_ClearButton";
    private const string CancelButtonPartName = "PART_CancelButton";

    private TextBox? _textBox;
    private Button? _clearButton;
    private Button? _cancelButton;

    private bool _isClearVisible;
    private bool _isCancelVisible;
    private bool _isInputFocused;
    private TextAlignment _textAlignment = TextAlignment.Left;
    private HorizontalAlignment _textHorizontalAlignment = HorizontalAlignment.Left;

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<SearchBar, string?>(nameof(Text));

    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<SearchBar, string?>(nameof(Placeholder));

    public static readonly StyledProperty<bool> IsClearableProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(IsClearable), true);

    public static readonly StyledProperty<bool> ShowCancelButtonProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(ShowCancelButton));

    public static readonly StyledProperty<bool> ShowCancelButtonOnFocusProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(ShowCancelButtonOnFocus), true);

    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<SearchBar, string>(nameof(CancelText), "取消");

    public static readonly StyledProperty<bool> CenterProperty =
        AvaloniaProperty.Register<SearchBar, bool>(nameof(Center));

    public static readonly StyledProperty<SearchBarShape> ShapeProperty =
        AvaloniaProperty.Register<SearchBar, SearchBarShape>(nameof(Shape), SearchBarShape.Default);

    public static readonly DirectProperty<SearchBar, bool> IsClearVisibleProperty =
        AvaloniaProperty.RegisterDirect<SearchBar, bool>(
            nameof(IsClearVisible),
            o => o.IsClearVisible);

    public static readonly DirectProperty<SearchBar, bool> IsCancelVisibleProperty =
        AvaloniaProperty.RegisterDirect<SearchBar, bool>(
            nameof(IsCancelVisible),
            o => o.IsCancelVisible);

    public static readonly DirectProperty<SearchBar, bool> IsInputFocusedProperty =
        AvaloniaProperty.RegisterDirect<SearchBar, bool>(
            nameof(IsInputFocused),
            o => o.IsInputFocused);

    public static readonly DirectProperty<SearchBar, TextAlignment> TextAlignmentProperty =
        AvaloniaProperty.RegisterDirect<SearchBar, TextAlignment>(
            nameof(TextAlignment),
            o => o.TextAlignment);

    public static readonly DirectProperty<SearchBar, HorizontalAlignment> TextHorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<SearchBar, HorizontalAlignment>(
            nameof(TextHorizontalAlignment),
            o => o.TextHorizontalAlignment);

    static SearchBar()
    {
        ClipToBoundsProperty.OverrideDefaultValue<SearchBar>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<SearchBar>(HorizontalAlignment.Stretch);

        TextProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
        IsClearableProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
        ShowCancelButtonProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
        ShowCancelButtonOnFocusProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
        CenterProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
        ShapeProperty.Changed.AddClassHandler<SearchBar>((o, _) => o.UpdateState());
    }

    /// <summary>
    /// 用户在输入框按下 Enter 时触发。
    /// </summary>
    public event EventHandler? SearchRequested;

    /// <summary>
    /// 用户点击取消按钮时触发。
    /// </summary>
    public event EventHandler? CancelRequested;

    /// <summary>
    /// 用户点击清除按钮时触发。
    /// </summary>
    public event EventHandler? Cleared;

    /// <summary>
    /// 获取或设置当前输入文本。
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 获取或设置占位文案。
    /// </summary>
    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用清除按钮能力。
    /// </summary>
    public bool IsClearable
    {
        get => GetValue(IsClearableProperty);
        set => SetValue(IsClearableProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用取消按钮能力。
    /// </summary>
    public bool ShowCancelButton
    {
        get => GetValue(ShowCancelButtonProperty);
        set => SetValue(ShowCancelButtonProperty, value);
    }

    /// <summary>
    /// 获取或设置取消按钮是否受焦点/文本状态控制显示与隐藏。
    /// </summary>
    public bool ShowCancelButtonOnFocus
    {
        get => GetValue(ShowCancelButtonOnFocusProperty);
        set => SetValue(ShowCancelButtonOnFocusProperty, value);
    }

    /// <summary>
    /// 获取或设置取消按钮文案。
    /// </summary>
    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    /// <summary>
    /// 获取或设置文本是否居中显示。
    /// </summary>
    public bool Center
    {
        get => GetValue(CenterProperty);
        set => SetValue(CenterProperty, value);
    }

    /// <summary>
    /// 获取或设置形状预设。
    /// </summary>
    public SearchBarShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    /// <summary>
    /// 获取当前状态下清除按钮是否可见。
    /// </summary>
    public bool IsClearVisible
    {
        get => _isClearVisible;
        private set => SetAndRaise(IsClearVisibleProperty, ref _isClearVisible, value);
    }

    /// <summary>
    /// 获取当前状态下取消按钮是否可见。
    /// </summary>
    public bool IsCancelVisible
    {
        get => _isCancelVisible;
        private set => SetAndRaise(IsCancelVisibleProperty, ref _isCancelVisible, value);
    }

    /// <summary>
    /// 获取内部 <see cref="TextBox"/> 是否处于焦点状态。
    /// </summary>
    public bool IsInputFocused
    {
        get => _isInputFocused;
        private set => SetAndRaise(IsInputFocusedProperty, ref _isInputFocused, value);
    }

    /// <summary>
    /// 获取当前 <see cref="Center"/> 设置对应的文本对齐方式（自动计算）。
    /// </summary>
    public TextAlignment TextAlignment
    {
        get => _textAlignment;
        private set => SetAndRaise(TextAlignmentProperty, ref _textAlignment, value);
    }

    /// <summary>
    /// 获取当前 <see cref="Center"/> 设置对应的水平对齐方式（自动计算）。
    /// </summary>
    public HorizontalAlignment TextHorizontalAlignment
    {
        get => _textHorizontalAlignment;
        private set => SetAndRaise(TextHorizontalAlignmentProperty, ref _textHorizontalAlignment, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachHandlers();

        _textBox = e.NameScope.Find<TextBox>(TextBoxPartName);
        _clearButton = e.NameScope.Find<Button>(ClearButtonPartName);
        _cancelButton = e.NameScope.Find<Button>(CancelButtonPartName);

        AttachHandlers();
        UpdateState();
    }

    private void AttachHandlers()
    {
        if (_textBox is not null)
        {
            _textBox.GotFocus += OnTextBoxGotFocus;
            _textBox.LostFocus += OnTextBoxLostFocus;
            _textBox.KeyDown += OnTextBoxKeyDown;
        }

        if (_clearButton is not null)
        {
            _clearButton.Click += OnClearClick;
        }

        if (_cancelButton is not null)
        {
            _cancelButton.Click += OnCancelClick;
        }
    }

    private void DetachHandlers()
    {
        if (_textBox is not null)
        {
            _textBox.GotFocus -= OnTextBoxGotFocus;
            _textBox.LostFocus -= OnTextBoxLostFocus;
            _textBox.KeyDown -= OnTextBoxKeyDown;
        }

        if (_clearButton is not null)
        {
            _clearButton.Click -= OnClearClick;
        }

        if (_cancelButton is not null)
        {
            _cancelButton.Click -= OnCancelClick;
        }
    }

    private void OnTextBoxGotFocus(object? sender, RoutedEventArgs e)
    {
        IsInputFocused = true;
        UpdateState();
    }

    private void OnTextBoxLostFocus(object? sender, RoutedEventArgs e)
    {
        IsInputFocused = false;
        UpdateState();
    }

    private void OnTextBoxKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Enter)
        {
            SearchRequested?.Invoke(this, EventArgs.Empty);
            e.Handled = true;
        }
    }

    private void OnClearClick(object? sender, EventArgs e)
    {
        Text = string.Empty;
        Cleared?.Invoke(this, EventArgs.Empty);
        _textBox?.Focus();
        UpdateState();
    }

    private void OnCancelClick(object? sender, EventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
        Text = string.Empty;
        Focus();
        IsInputFocused = false;
        UpdateState();
    }

    private void UpdateState()
    {
        IsClearVisible = IsClearable && !string.IsNullOrEmpty(Text);

        var shouldShowCancel = ShowCancelButton;
        if (ShowCancelButtonOnFocus)
        {
            shouldShowCancel &= IsInputFocused || !string.IsNullOrEmpty(Text);
        }

        IsCancelVisible = shouldShowCancel;

        TextAlignment = Center ? TextAlignment.Center : TextAlignment.Left;
        TextHorizontalAlignment = Center ? HorizontalAlignment.Center : HorizontalAlignment.Left;

        PseudoClasses.Set(":round", Shape == SearchBarShape.Round);
        PseudoClasses.Set(":default", Shape == SearchBarShape.Default);
        PseudoClasses.Set(":center", Center);
        PseudoClasses.Set(":focused", IsInputFocused);
        PseudoClasses.Set(":has-text", !string.IsNullOrEmpty(Text));
    }
}
