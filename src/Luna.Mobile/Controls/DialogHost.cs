using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 对话框宿主控件，负责渲染遮罩与对话框面板，并处理按钮/关闭逻辑。
/// </summary>
/// <remarks>
/// 通常每个页面放置一个实例；静态入口 <see cref="Dialog"/> 会使用最近附加到可视树的 <see cref="Current"/>。
/// </remarks>
public sealed class DialogHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string DialogPartName = "PART_Dialog";
    private const string ConfirmButtonPartName = "PART_ConfirmButton";
    private const string CancelButtonPartName = "PART_CancelButton";
    private const string CloseButtonPartName = "PART_CloseButton";

    private static DialogHost? _current;

    private Border? _overlay;
    private Control? _dialog;
    private Button? _confirmButton;
    private Button? _cancelButton;
    private Button? _confirmButtonVertical;
    private Button? _cancelButtonVertical;
    private Button? _closeButton;
    private DialogOptions? _options;
    private DialogButtonTheme _confirmTheme;
    private DialogButtonTheme _cancelTheme;
    private bool _hasButtons;

    public static DialogHost? Current => _current;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(Title));

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(Content));

    public static readonly StyledProperty<object?> TopProperty =
        AvaloniaProperty.Register<DialogHost, object?>(nameof(Top));

    public static readonly StyledProperty<object?> MiddleProperty =
        AvaloniaProperty.Register<DialogHost, object?>(nameof(Middle));

    public static readonly StyledProperty<string?> ConfirmTextProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(ConfirmText));

    public static readonly StyledProperty<string?> CancelTextProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(CancelText));

    public static readonly StyledProperty<DialogButtonLayout> ButtonLayoutProperty =
        AvaloniaProperty.Register<DialogHost, DialogButtonLayout>(nameof(ButtonLayout), DialogButtonLayout.Horizontal);

    public static readonly DirectProperty<DialogHost, DialogButtonTheme> ConfirmThemeProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, DialogButtonTheme>(
            nameof(ConfirmTheme),
            o => o.ConfirmTheme);

    public static readonly DirectProperty<DialogHost, DialogButtonTheme> CancelThemeProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, DialogButtonTheme>(
            nameof(CancelTheme),
            o => o.CancelTheme);

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseOnOverlayClick));

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(ShowCloseButton));

    public static readonly StyledProperty<double> MaxContentHeightProperty =
        AvaloniaProperty.Register<DialogHost, double>(nameof(MaxContentHeight), 120);

    public static readonly DirectProperty<DialogHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    public static readonly DirectProperty<DialogHost, bool> HasContentProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasContent),
            o => o.HasContent);

    public static readonly DirectProperty<DialogHost, bool> HasTopProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasTop),
            o => o.HasTop);

    public static readonly DirectProperty<DialogHost, bool> HasMiddleProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasMiddle),
            o => o.HasMiddle);

    public static readonly DirectProperty<DialogHost, bool> HasCancelProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasCancel),
            o => o.HasCancel);

    public static readonly DirectProperty<DialogHost, bool> HasConfirmProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasConfirm),
            o => o.HasConfirm);

    public static readonly DirectProperty<DialogHost, bool> HasButtonsProperty =
        AvaloniaProperty.RegisterDirect<DialogHost, bool>(
            nameof(HasButtons),
            o => o.HasButtons);

    private bool _hasTitle;
    private bool _hasContent;
    private bool _hasTop;
    private bool _hasMiddle;
    private bool _hasCancel;
    private bool _hasConfirm;

    static DialogHost()
    {
        IsOpenProperty.Changed.AddClassHandler<DialogHost>((control, args) =>
        {
            control.PseudoClasses.Set(":open", args.GetNewValue<bool>());
        });

        ButtonLayoutProperty.Changed.AddClassHandler<DialogHost>((control, args) =>
        {
            var layout = args.GetNewValue<DialogButtonLayout>();
            control.PseudoClasses.Set(":buttons-horizontal", layout == DialogButtonLayout.Horizontal);
            control.PseudoClasses.Set(":buttons-vertical", layout == DialogButtonLayout.Vertical);
        });
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public object? Top
    {
        get => GetValue(TopProperty);
        set => SetValue(TopProperty, value);
    }

    public object? Middle
    {
        get => GetValue(MiddleProperty);
        set => SetValue(MiddleProperty, value);
    }

    public string? ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public string? CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public DialogButtonLayout ButtonLayout
    {
        get => GetValue(ButtonLayoutProperty);
        set => SetValue(ButtonLayoutProperty, value);
    }

    public DialogButtonTheme ConfirmTheme
    {
        get => _confirmTheme;
        private set => SetAndRaise(ConfirmThemeProperty, ref _confirmTheme, value);
    }

    public DialogButtonTheme CancelTheme
    {
        get => _cancelTheme;
        private set => SetAndRaise(CancelThemeProperty, ref _cancelTheme, value);
    }

    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public double MaxContentHeight
    {
        get => GetValue(MaxContentHeightProperty);
        set => SetValue(MaxContentHeightProperty, value);
    }

    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    public bool HasContent
    {
        get => _hasContent;
        private set => SetAndRaise(HasContentProperty, ref _hasContent, value);
    }

    public bool HasTop
    {
        get => _hasTop;
        private set => SetAndRaise(HasTopProperty, ref _hasTop, value);
    }

    public bool HasMiddle
    {
        get => _hasMiddle;
        private set => SetAndRaise(HasMiddleProperty, ref _hasMiddle, value);
    }

    public bool HasCancel
    {
        get => _hasCancel;
        private set => SetAndRaise(HasCancelProperty, ref _hasCancel, value);
    }

    public bool HasConfirm
    {
        get => _hasConfirm;
        private set => SetAndRaise(HasConfirmProperty, ref _hasConfirm, value);
    }

    public bool HasButtons
    {
        get => _hasButtons;
        private set => SetAndRaise(HasButtonsProperty, ref _hasButtons, value);
    }

    public event EventHandler? Confirmed;
    public event EventHandler? Canceled;
    public event EventHandler? Closed;

    public void Show(DialogOptions options)
    {
        _options = options;

        Title = options.Title;
        Content = options.Content;
        Top = options.Top;
        Middle = options.Middle;
        ConfirmText = options.ConfirmButton?.Content;
        CancelText = options.CancelButton?.Content;
        ButtonLayout = options.ButtonLayout;
        ConfirmTheme = options.ConfirmButton?.Theme ?? DialogButtonTheme.Default;
        CancelTheme = options.CancelButton?.Theme ?? DialogButtonTheme.Default;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        ShowCloseButton = options.ShowCloseButton;
        MaxContentHeight = options.MaxContentHeight;

        HasTitle = !string.IsNullOrWhiteSpace(Title);
        HasContent = !string.IsNullOrWhiteSpace(Content);
        HasTop = Top is not null;
        HasMiddle = Middle is not null;
        HasCancel = !string.IsNullOrWhiteSpace(CancelText);
        HasConfirm = !string.IsNullOrWhiteSpace(ConfirmText);
        HasButtons = HasCancel || HasConfirm;

        PseudoClasses.Set(":confirm-primary", ConfirmTheme == DialogButtonTheme.Primary);
        PseudoClasses.Set(":confirm-danger", ConfirmTheme == DialogButtonTheme.Danger);

        IsOpen = true;
    }

    public void Close()
    {
        _options = null;
        IsOpen = false;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
        {
            _overlay.PointerPressed -= OnOverlayPressed;
        }
        if (_confirmButton is not null)
        {
            _confirmButton.Click -= OnConfirmClick;
        }
        if (_cancelButton is not null)
        {
            _cancelButton.Click -= OnCancelClick;
        }
        if (_confirmButtonVertical is not null)
        {
            _confirmButtonVertical.Click -= OnConfirmClick;
        }
        if (_cancelButtonVertical is not null)
        {
            _cancelButtonVertical.Click -= OnCancelClick;
        }
        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseClick;
        }

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        _dialog = e.NameScope.Find<Control>(DialogPartName);
        _confirmButton = e.NameScope.Find<Button>(ConfirmButtonPartName);
        _cancelButton = e.NameScope.Find<Button>(CancelButtonPartName);
        _confirmButtonVertical = e.NameScope.Find<Button>($"{ConfirmButtonPartName}_Vertical");
        _cancelButtonVertical = e.NameScope.Find<Button>($"{CancelButtonPartName}_Vertical");
        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);

        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPressed;
        }
        if (_confirmButton is not null)
        {
            _confirmButton.Click += OnConfirmClick;
        }
        if (_cancelButton is not null)
        {
            _cancelButton.Click += OnCancelClick;
        }
        if (_confirmButtonVertical is not null)
        {
            _confirmButtonVertical.Click += OnConfirmClick;
        }
        if (_cancelButtonVertical is not null)
        {
            _cancelButtonVertical.Click += OnCancelClick;
        }
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseClick;
        }
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsOpen || !CloseOnOverlayClick)
        {
            return;
        }

        e.Handled = true;
        Close();
        Closed?.Invoke(this, EventArgs.Empty);
    }

    private void OnConfirmClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        Close();
        Confirmed?.Invoke(this, EventArgs.Empty);
    }

    private void OnCancelClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        Close();
        Canceled?.Invoke(this, EventArgs.Empty);
    }

    private void OnCloseClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        Close();
        Closed?.Invoke(this, EventArgs.Empty);
    }
}
