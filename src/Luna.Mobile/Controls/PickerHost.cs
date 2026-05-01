using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// Picker 宿主控件，负责渲染遮罩与底部选择面板，并处理取消/确认/关闭事件。
/// </summary>
/// <remarks>
/// 通常每个页面放置一个实例；静态入口 <see cref="Picker"/> 会使用最近附加到可视树的 <see cref="Current"/>。
/// </remarks>
[TemplatePart(OverlayPartName, typeof(Border))]
[TemplatePart(CancelButtonPartName, typeof(Button))]
[TemplatePart(ConfirmButtonPartName, typeof(Button))]
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

    /// <summary>
    /// 获取当前附加到可视树的选择器宿主实例。
    /// </summary>
    public static PickerHost? Current => _current;

    /// <summary>
    /// 点击取消按钮后触发。
    /// </summary>
    public event EventHandler? CancelRequested;

    /// <summary>
    /// 点击确认按钮后触发。
    /// </summary>
    public event EventHandler<PickerConfirmedEventArgs>? Confirmed;

    /// <summary>
    /// 选择器关闭后触发。
    /// </summary>
    public event EventHandler<PickerClosedEventArgs>? Closed;

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<PickerHost, bool>(nameof(IsOpen));

    /// <inheritdoc cref="CloseOnOverlayClick" />
    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<PickerHost, bool>(nameof(CloseOnOverlayClick), true);

    /// <inheritdoc cref="IsOverlayVisible" />
    public static readonly DirectProperty<PickerHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<PickerHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<PickerHost, string?>(nameof(Title));

    /// <inheritdoc cref="HasTitle" />
    public static readonly DirectProperty<PickerHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<PickerHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    /// <inheritdoc cref="CancelText" />
    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<PickerHost, string>(nameof(CancelText), "取消");

    /// <inheritdoc cref="ConfirmText" />
    public static readonly StyledProperty<string> ConfirmTextProperty =
        AvaloniaProperty.Register<PickerHost, string>(nameof(ConfirmText), "确认");

    /// <inheritdoc cref="SheetHeight" />
    public static readonly StyledProperty<double> SheetHeightProperty =
        AvaloniaProperty.Register<PickerHost, double>(nameof(SheetHeight), 320);

    /// <inheritdoc cref="Columns" />
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

    /// <summary>
    /// 初始化 <see cref="PickerHost" /> 的新实例。
    /// </summary>
    public PickerHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
    }

    /// <summary>
    /// 获取或设置选择器当前是否打开。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    /// <summary>
    /// 获取当前遮罩层是否可见。
    /// </summary>
    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
    }

    /// <summary>
    /// 获取或设置标题文本。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在标题。
    /// </summary>
    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    /// <summary>
    /// 获取或设置取消按钮文本。
    /// </summary>
    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    /// <summary>
    /// 获取或设置确认按钮文本。
    /// </summary>
    public string ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    /// <summary>
    /// 获取或设置底部面板高度。
    /// </summary>
    public double SheetHeight
    {
        get => GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
    }

    /// <summary>
    /// 获取或设置当前展示的列集合。
    /// </summary>
    public IReadOnlyList<PickerColumn> Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    /// <summary>
    /// 使用指定参数打开选择器。
    /// </summary>
    /// <param name="options">选择器配置参数。</param>
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

    /// <summary>
    /// 使用指定原因关闭选择器。
    /// </summary>
    /// <param name="reason">关闭原因。</param>
    public void Close(PickerCloseReason reason)
    {
        _closeReason = reason;
        IsOpen = false;
        UpdateOverlayVisible();
        Closed?.Invoke(this, new PickerClosedEventArgs(reason));
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    /// <inheritdoc />
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
