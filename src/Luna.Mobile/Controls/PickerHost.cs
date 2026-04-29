using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

public sealed class PickerHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string CancelButtonPartName = "PART_CancelButton";
    private const string ConfirmButtonPartName = "PART_ConfirmButton";

    private static PickerHost? _current;

    private Border? _overlay;
    private Button? _cancelButton;
    private Button? _confirmButton;
    private PickerCloseReason _closeReason = PickerCloseReason.Unknown;
    private bool _isOverlayVisible;
    private bool _hasTitle;

    public static PickerHost? Current => _current;

    public event EventHandler? CancelRequested;
    public event EventHandler<PickerConfirmedEventArgs>? Confirmed;
    public event EventHandler<PickerClosedEventArgs>? Closed;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<PickerHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<PickerHost, bool>(nameof(CloseOnOverlayClick), true);

    public static readonly DirectProperty<PickerHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<PickerHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<PickerHost, string?>(nameof(Title));

    public static readonly DirectProperty<PickerHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<PickerHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<PickerHost, string>(nameof(CancelText), "取消");

    public static readonly StyledProperty<string> ConfirmTextProperty =
        AvaloniaProperty.Register<PickerHost, string>(nameof(ConfirmText), "确认");

    public static readonly StyledProperty<double> SheetHeightProperty =
        AvaloniaProperty.Register<PickerHost, double>(nameof(SheetHeight), 320);

    public static readonly StyledProperty<IReadOnlyList<PickerColumn>> ColumnsProperty =
        AvaloniaProperty.Register<PickerHost, IReadOnlyList<PickerColumn>>(nameof(Columns), Array.Empty<PickerColumn>());

    static PickerHost()
    {
        IsOpenProperty.Changed.AddClassHandler<PickerHost>((control, _) => control.UpdateOverlayVisible());
        TitleProperty.Changed.AddClassHandler<PickerHost>((control, args) =>
        {
            control.HasTitle = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });
    }

    public PickerHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public string ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public double SheetHeight
    {
        get => GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    public IReadOnlyList<PickerColumn> Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public void Show(PickerOptions options)
    {
        _closeReason = PickerCloseReason.Unknown;
        Columns = options.Columns;
        Title = options.Title;
        CancelText = options.CancelText;
        ConfirmText = options.ConfirmText;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        SheetHeight = options.SheetHeight;
        IsOpen = true;
        UpdateOverlayVisible();
    }

    public void Close(PickerCloseReason reason)
    {
        _closeReason = reason;
        IsOpen = false;
        UpdateOverlayVisible();
        Closed?.Invoke(this, new PickerClosedEventArgs(reason));
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
        if (_cancelButton is not null)
        {
            _cancelButton.Click -= OnCancelClick;
        }
        if (_confirmButton is not null)
        {
            _confirmButton.Click -= OnConfirmClick;
        }

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        _cancelButton = e.NameScope.Find<Button>(CancelButtonPartName);
        _confirmButton = e.NameScope.Find<Button>(ConfirmButtonPartName);

        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPressed;
        }
        if (_cancelButton is not null)
        {
            _cancelButton.Click += OnCancelClick;
        }
        if (_confirmButton is not null)
        {
            _confirmButton.Click += OnConfirmClick;
        }
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsOpen || !CloseOnOverlayClick)
        {
            return;
        }

        e.Handled = true;
        Close(PickerCloseReason.Overlay);
    }

    private void OnCancelClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        CancelRequested?.Invoke(this, EventArgs.Empty);
        Close(PickerCloseReason.Cancel);
    }

    private void OnConfirmClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        var indices = new int[Columns.Count];
        var values = new string?[Columns.Count];
        for (var i = 0; i < Columns.Count; i++)
        {
            var column = Columns[i];
            indices[i] = column.SelectedIndex;
            values[i] = column.SelectedValue;
        }

        Confirmed?.Invoke(this, new PickerConfirmedEventArgs(indices, values));
        Close(PickerCloseReason.Confirm);
    }

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen;
    }
}

