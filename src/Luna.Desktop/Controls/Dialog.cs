using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;

namespace Luna.Desktop.Controls;

public enum DialogResult
{
    None,
    Confirmed,
    Canceled,
}

public sealed record DialogOptions
{
    public string? Title { get; init; }
    public object? Content { get; init; }
    public string? ConfirmText { get; init; } = "确认";
    public string? CancelText { get; init; } = "取消";
    public bool ShowCloseButton { get; init; } = true;
    public bool ShowCancelButton { get; init; } = true;
    public bool CloseOnOverlayClick { get; init; }
    public bool CloseOnEsc { get; init; } = true;
    public double Width { get; init; } = 480;
}

[PseudoClasses(PC_Open, PC_ConfirmLoading)]
public sealed class DialogHost : TemplatedControl
{
    private const string PC_Open = ":open";
    private const string PC_ConfirmLoading = ":confirm-loading";

    private static DialogHost? _current;
    private Border? _overlay;
    private DialogOptions? _options;

    public static DialogHost? Current => _current;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(Title));

    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<DialogHost, object?>(nameof(Content));

    public static readonly StyledProperty<string?> ConfirmTextProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(ConfirmText));

    public static readonly StyledProperty<string?> CancelTextProperty =
        AvaloniaProperty.Register<DialogHost, string?>(nameof(CancelText));

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(ShowCloseButton), true);

    public static readonly StyledProperty<bool> ShowCancelButtonProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(ShowCancelButton), true);

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseOnOverlayClick));

    public static readonly StyledProperty<bool> CloseOnEscProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(CloseOnEsc), true);

    public static readonly StyledProperty<double> DialogWidthProperty =
        AvaloniaProperty.Register<DialogHost, double>(nameof(DialogWidth), 480);

    public static readonly StyledProperty<bool> ConfirmLoadingProperty =
        AvaloniaProperty.Register<DialogHost, bool>(nameof(ConfirmLoading));

    static DialogHost()
    {
        IsOpenProperty.Changed.AddClassHandler<DialogHost>((control, args) =>
            control.PseudoClasses.Set(PC_Open, args.GetNewValue<bool>()));
        ConfirmLoadingProperty.Changed.AddClassHandler<DialogHost>((control, args) =>
            control.PseudoClasses.Set(PC_ConfirmLoading, args.GetNewValue<bool>()));
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

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
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

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public bool ShowCancelButton
    {
        get => GetValue(ShowCancelButtonProperty);
        set => SetValue(ShowCancelButtonProperty, value);
    }

    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    public bool CloseOnEsc
    {
        get => GetValue(CloseOnEscProperty);
        set => SetValue(CloseOnEscProperty, value);
    }

    public double DialogWidth
    {
        get => GetValue(DialogWidthProperty);
        set => SetValue(DialogWidthProperty, value);
    }

    public bool ConfirmLoading
    {
        get => GetValue(ConfirmLoadingProperty);
        set => SetValue(ConfirmLoadingProperty, value);
    }

    public event EventHandler<DialogResult>? Result;

    public void Show(DialogOptions options)
    {
        _options = options;
        Title = options.Title;
        Content = options.Content;
        ConfirmText = options.ConfirmText;
        CancelText = options.CancelText;
        ShowCloseButton = options.ShowCloseButton;
        ShowCancelButton = options.ShowCancelButton;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        CloseOnEsc = options.CloseOnEsc;
        DialogWidth = options.Width;
        ConfirmLoading = false;
        IsOpen = true;
    }

    public void Close(DialogResult result = DialogResult.None)
    {
        _options = null;
        ConfirmLoading = false;
        IsOpen = false;
        Result?.Invoke(this, result);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
        KeyDown += OnKeyDown;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        KeyDown -= OnKeyDown;
        if (ReferenceEquals(_current, this))
            _current = null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
            _overlay.PointerPressed -= OnOverlayPressed;

        _overlay = e.NameScope.Find<Border>("PART_Overlay");

        var confirmBtn = e.NameScope.Find<Button>("PART_ConfirmButton");
        var cancelBtn = e.NameScope.Find<Button>("PART_CancelButton");
        var closeBtn = e.NameScope.Find<Button>("PART_CloseButton");

        if (confirmBtn is not null)
            confirmBtn.Click += (_, _) => Close(DialogResult.Confirmed);
        if (cancelBtn is not null)
            cancelBtn.Click += (_, _) => Close(DialogResult.Canceled);
        if (closeBtn is not null)
            closeBtn.Click += (_, _) => Close(DialogResult.Canceled);

        if (_overlay is not null)
            _overlay.PointerPressed += OnOverlayPressed;
    }

    private void OnKeyDown(object? sender, KeyEventArgs e)
    {
        if (e.Key == Key.Escape && IsOpen && CloseOnEsc)
        {
            e.Handled = true;
            Close(DialogResult.Canceled);
        }
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (IsOpen && CloseOnOverlayClick)
        {
            e.Handled = true;
            Close(DialogResult.Canceled);
        }
    }
}

public static class LunaDialog
{
    public static void Show(DialogOptions options) => DialogHost.Current?.Show(options);
    public static void Close() => DialogHost.Current?.Close();
}
