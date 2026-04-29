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

public enum SearchBarShape
{
    Default,
    Round,
}

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

    public event EventHandler? SearchRequested;
    public event EventHandler? CancelRequested;
    public event EventHandler? Cleared;

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsClearable
    {
        get => GetValue(IsClearableProperty);
        set => SetValue(IsClearableProperty, value);
    }

    public bool ShowCancelButton
    {
        get => GetValue(ShowCancelButtonProperty);
        set => SetValue(ShowCancelButtonProperty, value);
    }

    public bool ShowCancelButtonOnFocus
    {
        get => GetValue(ShowCancelButtonOnFocusProperty);
        set => SetValue(ShowCancelButtonOnFocusProperty, value);
    }

    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public bool Center
    {
        get => GetValue(CenterProperty);
        set => SetValue(CenterProperty, value);
    }

    public SearchBarShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public bool IsClearVisible
    {
        get => _isClearVisible;
        private set => SetAndRaise(IsClearVisibleProperty, ref _isClearVisible, value);
    }

    public bool IsCancelVisible
    {
        get => _isCancelVisible;
        private set => SetAndRaise(IsCancelVisibleProperty, ref _isCancelVisible, value);
    }

    public bool IsInputFocused
    {
        get => _isInputFocused;
        private set => SetAndRaise(IsInputFocusedProperty, ref _isInputFocused, value);
    }

    public TextAlignment TextAlignment
    {
        get => _textAlignment;
        private set => SetAndRaise(TextAlignmentProperty, ref _textAlignment, value);
    }

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
