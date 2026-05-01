using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace Luna.Mobile.Controls;

/// <summary>
/// DateTimePicker 的粒度模式。
/// </summary>
public enum DateTimePickerMode
{
    /// <summary>
    /// 仅选择年份。
    /// </summary>
    Year,

    /// <summary>
    /// 选择年份和月份。
    /// </summary>
    Month,

    /// <summary>
    /// 选择年月日。
    /// </summary>
    Date,

    /// <summary>
    /// 选择到小时。
    /// </summary>
    Hour,

    /// <summary>
    /// 选择到分钟。
    /// </summary>
    Minute,

    /// <summary>
    /// 选择到秒。
    /// </summary>
    Second,
}

/// <summary>
/// DateTimePicker 的关闭原因。
/// </summary>
public enum DateTimePickerCloseReason
{
    /// <summary>
    /// 未知原因关闭。
    /// </summary>
    Unknown,

    /// <summary>
    /// 点击遮罩关闭。
    /// </summary>
    Overlay,

    /// <summary>
    /// 点击取消按钮关闭。
    /// </summary>
    Cancel,

    /// <summary>
    /// 点击确认按钮关闭。
    /// </summary>
    Confirm,

    /// <summary>
    /// 通过代码主动关闭。
    /// </summary>
    Programmatic,
}

/// <summary>
/// DateTimePicker 的时间步进配置。
/// </summary>
public sealed class DateTimePickerStepOptions
{
    /// <summary>
    /// 获取或设置小时步进，最小为 1。
    /// </summary>
    public int Hour { get; init; } = 1;

    /// <summary>
    /// 获取或设置分钟步进，最小为 1。
    /// </summary>
    public int Minute { get; init; } = 1;

    /// <summary>
    /// 获取或设置秒步进，最小为 1。
    /// </summary>
    public int Second { get; init; } = 1;
}

/// <summary>
/// DateTimePicker 显示参数。
/// </summary>
public sealed class DateTimePickerOptions
{
    /// <summary>
    /// 获取或设置标题文本。
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 获取或设置取消按钮文本。
    /// </summary>
    public string CancelText { get; init; } = "取消";

    /// <summary>
    /// 获取或设置确认按钮文本。
    /// </summary>
    public string ConfirmText { get; init; } = "确认";

    /// <summary>
    /// 获取或设置最小可选时间；为空时默认当前时间前 10 年。
    /// </summary>
    public DateTime? Start { get; init; }

    /// <summary>
    /// 获取或设置最大可选时间；为空时默认当前时间后 10 年。
    /// </summary>
    public DateTime? End { get; init; }

    /// <summary>
    /// 获取或设置当前选中值。
    /// </summary>
    public DateTime? Value { get; init; }

    /// <summary>
    /// 获取或设置格式化字符串，默认使用 TDesign 风格格式。
    /// </summary>
    public string Format { get; init; } = "YYYY-MM-DD HH:mm:ss";

    /// <summary>
    /// 获取或设置选择粒度模式。
    /// </summary>
    public DateTimePickerMode Mode { get; init; } = DateTimePickerMode.Date;

    /// <summary>
    /// 获取或设置是否在日期标签旁边显示星期。
    /// </summary>
    public bool ShowWeek { get; init; }

    /// <summary>
    /// 获取或设置时间列步进配置。
    /// </summary>
    public DateTimePickerStepOptions Steps { get; init; } = new();

    /// <summary>
    /// 获取或设置弹层高度。
    /// </summary>
    public double SheetHeight { get; init; } = 320;

    /// <summary>
    /// 获取或设置是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick { get; init; } = true;
}

/// <summary>
/// DateTimePicker 单个列项。
/// </summary>
public sealed class DateTimePickerColumnItem
{
    /// <summary>
    /// 获取展示文本。
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// 获取实际数值。
    /// </summary>
    public int Value { get; init; }
}

/// <summary>
/// DateTimePicker 的单列模型。
/// </summary>
public sealed class DateTimePickerColumn : INotifyPropertyChanged
{
    private int _selectedIndex;

    /// <summary>
    /// 获取或设置列类型标识。
    /// </summary>
    public string Type { get; init; } = string.Empty;

    /// <summary>
    /// 获取或设置当前列的选项列表。
    /// </summary>
    public IReadOnlyList<DateTimePickerColumnItem> Items { get; init; } = Array.Empty<DateTimePickerColumnItem>();

    /// <summary>
    /// 获取或设置当前选中索引。
    /// </summary>
    public int SelectedIndex
    {
        get => _selectedIndex;
        set
        {
            if (_selectedIndex == value)
            {
                return;
            }

            _selectedIndex = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedIndex)));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedItem)));
        }
    }

    /// <summary>
    /// 获取当前选中项；当索引无效时返回 null。
    /// </summary>
    public DateTimePickerColumnItem? SelectedItem
    {
        get
        {
            if (SelectedIndex < 0 || SelectedIndex >= Items.Count)
            {
                return null;
            }

            return Items[SelectedIndex];
        }
    }

    /// <summary>
    /// 当列的选中状态发生变化时触发。
    /// </summary>
    public event PropertyChangedEventHandler? PropertyChanged;
}

/// <summary>
/// DateTimePicker 确认事件参数。
/// </summary>
public sealed class DateTimePickerConfirmedEventArgs : EventArgs
{
    /// <summary>
    /// 使用确认结果初始化事件参数。
    /// </summary>
    /// <param name="value">确认后的时间值。</param>
    /// <param name="formattedValue">格式化后的文本。</param>
    public DateTimePickerConfirmedEventArgs(DateTime value, string formattedValue)
    {
        Value = value;
        FormattedValue = formattedValue;
    }

    /// <summary>
    /// 获取确认后的时间值。
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// 获取格式化后的文本。
    /// </summary>
    public string FormattedValue { get; }
}

/// <summary>
/// DateTimePicker 选中中间态变化事件参数。
/// </summary>
public sealed class DateTimePickerPickedEventArgs : EventArgs
{
    /// <summary>
    /// 使用当前选中结果初始化事件参数。
    /// </summary>
    /// <param name="value">当前选中的时间值。</param>
    /// <param name="formattedValue">格式化后的文本。</param>
    public DateTimePickerPickedEventArgs(DateTime value, string formattedValue)
    {
        Value = value;
        FormattedValue = formattedValue;
    }

    /// <summary>
    /// 获取当前选中的时间值。
    /// </summary>
    public DateTime Value { get; }

    /// <summary>
    /// 获取格式化后的文本。
    /// </summary>
    public string FormattedValue { get; }
}

/// <summary>
/// DateTimePicker 关闭事件参数。
/// </summary>
public sealed class DateTimePickerClosedEventArgs : EventArgs
{
    /// <summary>
    /// 使用关闭原因初始化事件参数。
    /// </summary>
    /// <param name="reason">关闭原因。</param>
    public DateTimePickerClosedEventArgs(DateTimePickerCloseReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// 获取关闭原因。
    /// </summary>
    public DateTimePickerCloseReason Reason { get; }
}

/// <summary>
/// DateTimePicker 的静态入口，依赖页面中的 <see cref="DateTimePickerHost"/>。
/// </summary>
public static class DateTimePicker
{
    /// <summary>
    /// 使用指定参数显示时间选择器。
    /// </summary>
    public static void Show(DateTimePickerOptions options) => DateTimePickerHost.Current?.Show(options);

    /// <summary>
    /// 以编程方式关闭当前时间选择器。
    /// </summary>
    public static void Close() => DateTimePickerHost.Current?.Close(DateTimePickerCloseReason.Programmatic);
}

/// <summary>
/// DateTimePicker 宿主控件，负责渲染弹层并管理日期时间列联动。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_Overlay：遮罩层 <see cref="Border"/></description></item>
/// <item><description>PART_CancelButton：取消按钮 <see cref="Button"/></description></item>
/// <item><description>PART_ConfirmButton：确认按钮 <see cref="Button"/></description></item>
/// </list>
/// </remarks>
[TemplatePart(OverlayPartName, typeof(Border))]
[TemplatePart(SheetPartName, typeof(Border))]
[TemplatePart(CancelButtonPartName, typeof(Button))]
[TemplatePart(ConfirmButtonPartName, typeof(Button))]
public sealed class DateTimePickerHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string SheetPartName = "PART_Sheet";
    private const string CancelButtonPartName = "PART_CancelButton";
    private const string ConfirmButtonPartName = "PART_ConfirmButton";

    private static DateTimePickerHost? _current;

    private Border? _overlay;
    private Border? _sheet;
    private Button? _cancelButton;
    private Button? _confirmButton;
    private DateTime _start = DateTime.Now.AddYears(-10);
    private DateTime _end = DateTime.Now.AddYears(10);
    private DateTime _currentValue = DateTime.Now;
    private DateTimePickerMode _mode = DateTimePickerMode.Date;
    private string _format = "YYYY-MM-DD HH:mm:ss";
    private bool _showWeek;
    private bool _isSyncingColumns;
    private bool _isOverlayVisible;
    private bool _isSheetVisible;
    private bool _hasTitle;
    private IReadOnlyList<DateTimePickerColumn> _columns = Array.Empty<DateTimePickerColumn>();
    private DateTimePickerCloseReason _closeReason = DateTimePickerCloseReason.Unknown;
    private DateTimePickerStepOptions _steps = new();
    private int _animationVersion;

    /// <summary>
    /// 获取当前附加到可视树的时间选择器宿主实例。
    /// </summary>
    public static DateTimePickerHost? Current => _current;

    /// <summary>
    /// 点击取消按钮后触发。
    /// </summary>
    public event EventHandler? CancelRequested;

    /// <summary>
    /// 当前选中值发生变化时触发。
    /// </summary>
    public event EventHandler<DateTimePickerPickedEventArgs>? Picked;

    /// <summary>
    /// 点击确认按钮后触发。
    /// </summary>
    public event EventHandler<DateTimePickerConfirmedEventArgs>? Confirmed;

    /// <summary>
    /// 关闭后触发。
    /// </summary>
    public event EventHandler<DateTimePickerClosedEventArgs>? Closed;

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DateTimePickerHost, bool>(nameof(IsOpen));

    /// <inheritdoc cref="CloseOnOverlayClick" />
    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DateTimePickerHost, bool>(nameof(CloseOnOverlayClick), true);

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DateTimePickerHost, string?>(nameof(Title));

    /// <inheritdoc cref="CancelText" />
    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<DateTimePickerHost, string>(nameof(CancelText), "取消");

    /// <inheritdoc cref="ConfirmText" />
    public static readonly StyledProperty<string> ConfirmTextProperty =
        AvaloniaProperty.Register<DateTimePickerHost, string>(nameof(ConfirmText), "确认");

    /// <inheritdoc cref="SheetHeight" />
    public static readonly StyledProperty<double> SheetHeightProperty =
        AvaloniaProperty.Register<DateTimePickerHost, double>(nameof(SheetHeight), 320);

    /// <inheritdoc cref="IsOverlayVisible" />
    public static readonly DirectProperty<DateTimePickerHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<DateTimePickerHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    /// <inheritdoc cref="IsSheetVisible" />
    public static readonly DirectProperty<DateTimePickerHost, bool> IsSheetVisibleProperty =
        AvaloniaProperty.RegisterDirect<DateTimePickerHost, bool>(
            nameof(IsSheetVisible),
            o => o.IsSheetVisible);

    /// <inheritdoc cref="HasTitle" />
    public static readonly DirectProperty<DateTimePickerHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<DateTimePickerHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    /// <inheritdoc cref="Columns" />
    public static readonly DirectProperty<DateTimePickerHost, IReadOnlyList<DateTimePickerColumn>> ColumnsProperty =
        AvaloniaProperty.RegisterDirect<DateTimePickerHost, IReadOnlyList<DateTimePickerColumn>>(
            nameof(Columns),
            o => o.Columns);

    static DateTimePickerHost()
    {
        IsOpenProperty.Changed.AddClassHandler<DateTimePickerHost>((control, args) =>
        {
            control.HandleIsOpenChanged(args.GetNewValue<bool>());
        });
        TitleProperty.Changed.AddClassHandler<DateTimePickerHost>((control, args) =>
        {
            control.HasTitle = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });
    }

    /// <summary>
    /// 初始化 <see cref="DateTimePickerHost" /> 的新实例。
    /// </summary>
    public DateTimePickerHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
        IsSheetVisible = IsOpen;
        UpdateOverlayVisible();
    }

    /// <summary>
    /// 获取或设置当前是否打开。
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
    /// 获取或设置标题文本。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
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
    /// 获取或设置面板高度。
    /// </summary>
    public double SheetHeight
    {
        get => GetValue(SheetHeightProperty);
        set => SetValue(SheetHeightProperty, value);
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
    /// 获取当前底部面板是否仍需保持渲染，用于承载开关动画。
    /// </summary>
    public bool IsSheetVisible
    {
        get => _isSheetVisible;
        private set => SetAndRaise(IsSheetVisibleProperty, ref _isSheetVisible, value);
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
    /// 获取当前展示的列集合。
    /// </summary>
    public IReadOnlyList<DateTimePickerColumn> Columns
    {
        get => _columns;
        private set => SetAndRaise(ColumnsProperty, ref _columns, value);
    }

    /// <summary>
    /// 使用指定参数打开时间选择器。
    /// </summary>
    public void Show(DateTimePickerOptions options)
    {
        _closeReason = DateTimePickerCloseReason.Unknown;
        _mode = options.Mode;
        _format = string.IsNullOrWhiteSpace(options.Format) ? "YYYY-MM-DD HH:mm:ss" : options.Format;
        _showWeek = options.ShowWeek;
        _steps = options.Steps ?? new DateTimePickerStepOptions();

        var now = DateTime.Now;
        _start = options.Start ?? now.AddYears(-10);
        _end = options.End ?? now.AddYears(10);
        if (_end < _start)
        {
            _end = _start;
        }

        Title = options.Title;
        CancelText = options.CancelText;
        ConfirmText = options.ConfirmText;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        SheetHeight = options.SheetHeight;

        _currentValue = Clamp(options.Value ?? now);
        RebuildColumns(_currentValue, raisePicked: false);
        IsOpen = true;
    }

    /// <summary>
    /// 使用指定原因关闭时间选择器。
    /// </summary>
    public void Close(DateTimePickerCloseReason reason)
    {
        _closeReason = reason;
        IsOpen = false;
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

        DetachTemplateHandlers();
        DetachColumnHandlers(_columns);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachTemplateHandlers();

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        _sheet = e.NameScope.Find<Border>(SheetPartName);
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

    private void DetachTemplateHandlers()
    {
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
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsOpen || !CloseOnOverlayClick)
        {
            return;
        }

        e.Handled = true;
        Close(DateTimePickerCloseReason.Overlay);
    }

    private void OnCancelClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        CancelRequested?.Invoke(this, EventArgs.Empty);
        Close(DateTimePickerCloseReason.Cancel);
    }

    private void OnConfirmClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        Confirmed?.Invoke(this, new DateTimePickerConfirmedEventArgs(_currentValue, FormatValue(_currentValue)));
        Close(DateTimePickerCloseReason.Confirm);
    }

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen || IsSheetVisible;
    }

    private async void HandleIsOpenChanged(bool isOpen)
    {
        var version = ++_animationVersion;

        if (isOpen)
        {
            IsSheetVisible = true;
            UpdateOverlayVisible();
            await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);
            RefreshSheetLayout();
            await Dispatcher.UIThread.InvokeAsync(() => { }, DispatcherPriority.Render);
            await RunOpenAnimationAsync(version);
            return;
        }

        if (!IsSheetVisible && !IsOverlayVisible)
        {
            return;
        }

        await RunCloseAnimationAsync(version);
    }

    private async Task RunOpenAnimationAsync(int version)
    {
        if (_sheet is null && _overlay is null)
        {
            return;
        }

        var tasks = new List<Task>(2);

        if (_overlay is not null)
        {
            _overlay.Opacity = 0;
            tasks.Add(OverlayHostAnimationHelper.CreateOpacityAnimation(true, 0.6d).RunAsync(_overlay));
        }

        if (_sheet is not null)
        {
            var transform = OverlayHostAnimationHelper.EnsureTranslateTransform(_sheet);
            _sheet.Opacity = 0;
            transform.Y = GetClosedOffset();
            tasks.Add(OverlayHostAnimationHelper.CreateSlideAnimation(true, TranslateTransform.YProperty, GetClosedOffset()).RunAsync(_sheet));
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks);
        }

        if (version != _animationVersion || !IsOpen)
        {
            return;
        }
    }

    private async Task RunCloseAnimationAsync(int version)
    {
        if (_sheet is null && _overlay is null)
        {
            IsSheetVisible = false;
            UpdateOverlayVisible();
            Closed?.Invoke(this, new DateTimePickerClosedEventArgs(_closeReason));
            return;
        }

        var tasks = new List<Task>(2);

        if (_overlay is not null)
        {
            tasks.Add(OverlayHostAnimationHelper.CreateOpacityAnimation(false, 0.6d).RunAsync(_overlay));
        }

        if (_sheet is not null)
        {
            OverlayHostAnimationHelper.EnsureTranslateTransform(_sheet);
            tasks.Add(OverlayHostAnimationHelper.CreateSlideAnimation(false, TranslateTransform.YProperty, GetClosedOffset()).RunAsync(_sheet));
        }

        if (tasks.Count > 0)
        {
            await Task.WhenAll(tasks);
        }

        if (version != _animationVersion || IsOpen)
        {
            return;
        }

        IsSheetVisible = false;
        UpdateOverlayVisible();
        Closed?.Invoke(this, new DateTimePickerClosedEventArgs(_closeReason));
    }

    private double GetClosedOffset()
    {
        return OverlayHostAnimationHelper.ResolveDistance(SheetHeight, 320);
    }

    private void RefreshSheetLayout()
    {
        if (_sheet is null)
        {
            return;
        }

        _sheet.InvalidateMeasure();
        _sheet.InvalidateArrange();

        foreach (var listBox in _sheet.GetVisualDescendants().OfType<ListBox>())
        {
            listBox.InvalidateMeasure();
            listBox.InvalidateArrange();

            if (listBox.SelectedItem is not null)
            {
                listBox.ScrollIntoView(listBox.SelectedItem);
            }
        }
    }

    private void RebuildColumns(DateTime value, bool raisePicked)
    {
        _isSyncingColumns = true;

        var columns = new List<DateTimePickerColumn>();

        columns.Add(BuildColumn("year", CreateYearItems(), value.Year));

        if (Includes(DateTimePickerMode.Month))
        {
            columns.Add(BuildColumn("month", CreateMonthItems(value.Year), value.Month));
        }

        if (Includes(DateTimePickerMode.Date))
        {
            columns.Add(BuildColumn("date", CreateDayItems(value.Year, value.Month), value.Day));
        }

        if (Includes(DateTimePickerMode.Hour))
        {
            columns.Add(BuildColumn("hour", CreateHourItems(value.Year, value.Month, value.Day), value.Hour));
        }

        if (Includes(DateTimePickerMode.Minute))
        {
            columns.Add(BuildColumn("minute", CreateMinuteItems(value.Year, value.Month, value.Day, value.Hour), value.Minute));
        }

        if (Includes(DateTimePickerMode.Second))
        {
            columns.Add(BuildColumn("second", CreateSecondItems(value.Year, value.Month, value.Day, value.Hour, value.Minute), value.Second));
        }

        DetachColumnHandlers(_columns);
        Columns = columns;
        AttachColumnHandlers(columns);

        _currentValue = ReadValueFromColumns();
        _isSyncingColumns = false;

        if (raisePicked)
        {
            Picked?.Invoke(this, new DateTimePickerPickedEventArgs(_currentValue, FormatValue(_currentValue)));
        }
    }

    private DateTimePickerColumn BuildColumn(string type, IReadOnlyList<DateTimePickerColumnItem> items, int targetValue)
    {
        return new DateTimePickerColumn
        {
            Type = type,
            Items = items,
            SelectedIndex = ResolveSelectedIndex(items, targetValue)
        };
    }

    private void AttachColumnHandlers(IReadOnlyList<DateTimePickerColumn> columns)
    {
        foreach (var column in columns)
        {
            column.PropertyChanged += OnColumnPropertyChanged;
        }
    }

    private void DetachColumnHandlers(IReadOnlyList<DateTimePickerColumn> columns)
    {
        foreach (var column in columns)
        {
            column.PropertyChanged -= OnColumnPropertyChanged;
        }
    }

    private void OnColumnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (_isSyncingColumns || e.PropertyName != nameof(DateTimePickerColumn.SelectedIndex))
        {
            return;
        }

        _currentValue = Clamp(ReadValueFromColumns());
        RebuildColumns(_currentValue, raisePicked: true);
    }

    private DateTime ReadValueFromColumns()
    {
        var year = GetColumnValue("year", _currentValue.Year);
        var month = Includes(DateTimePickerMode.Month) ? GetColumnValue("month", _currentValue.Month) : 1;
        var day = Includes(DateTimePickerMode.Date) ? GetColumnValue("date", _currentValue.Day) : 1;
        var hour = Includes(DateTimePickerMode.Hour) ? GetColumnValue("hour", _currentValue.Hour) : 0;
        var minute = Includes(DateTimePickerMode.Minute) ? GetColumnValue("minute", _currentValue.Minute) : 0;
        var second = Includes(DateTimePickerMode.Second) ? GetColumnValue("second", _currentValue.Second) : 0;

        var maxDay = DateTime.DaysInMonth(year, month);
        day = Math.Min(day, maxDay);

        return Clamp(new DateTime(year, month, day, hour, minute, second));
    }

    private int GetColumnValue(string type, int fallback)
    {
        var column = Columns.FirstOrDefault(x => string.Equals(x.Type, type, StringComparison.Ordinal));
        return column?.SelectedItem?.Value ?? fallback;
    }

    private int ResolveSelectedIndex(IReadOnlyList<DateTimePickerColumnItem> items, int value)
    {
        for (var i = 0; i < items.Count; i++)
        {
            if (items[i].Value == value)
            {
                return i;
            }
        }

        for (var i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].Value <= value)
            {
                return i;
            }
        }

        return 0;
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateYearItems()
    {
        var items = new List<DateTimePickerColumnItem>();
        for (var year = _start.Year; year <= _end.Year; year++)
        {
            items.Add(new DateTimePickerColumnItem
            {
                Value = year,
                Label = $"{year}年"
            });
        }

        return items;
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateMonthItems(int year)
    {
        var startMonth = year == _start.Year ? _start.Month : 1;
        var endMonth = year == _end.Year ? _end.Month : 12;
        var items = new List<DateTimePickerColumnItem>();
        for (var month = startMonth; month <= endMonth; month++)
        {
            items.Add(new DateTimePickerColumnItem
            {
                Value = month,
                Label = $"{month:00}月"
            });
        }

        return items;
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateDayItems(int year, int month)
    {
        var startDay = year == _start.Year && month == _start.Month ? _start.Day : 1;
        var endDay = year == _end.Year && month == _end.Month ? _end.Day : DateTime.DaysInMonth(year, month);
        var items = new List<DateTimePickerColumnItem>();
        for (var day = startDay; day <= endDay; day++)
        {
            var date = new DateTime(year, month, day);
            var label = $"{day:00}日";
            if (_showWeek)
            {
                label = $"{label} {GetWeekText(date.DayOfWeek)}";
            }

            items.Add(new DateTimePickerColumnItem
            {
                Value = day,
                Label = label
            });
        }

        return items;
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateHourItems(int year, int month, int day)
    {
        var startHour = year == _start.Year && month == _start.Month && day == _start.Day ? _start.Hour : 0;
        var endHour = year == _end.Year && month == _end.Month && day == _end.Day ? _end.Hour : 23;
        return CreateNumberItems(startHour, endHour, Math.Max(1, _steps.Hour), "时");
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateMinuteItems(int year, int month, int day, int hour)
    {
        var startMinute = year == _start.Year && month == _start.Month && day == _start.Day && hour == _start.Hour ? _start.Minute : 0;
        var endMinute = year == _end.Year && month == _end.Month && day == _end.Day && hour == _end.Hour ? _end.Minute : 59;
        return CreateNumberItems(startMinute, endMinute, Math.Max(1, _steps.Minute), "分");
    }

    private IReadOnlyList<DateTimePickerColumnItem> CreateSecondItems(int year, int month, int day, int hour, int minute)
    {
        var startSecond = year == _start.Year && month == _start.Month && day == _start.Day && hour == _start.Hour && minute == _start.Minute ? _start.Second : 0;
        var endSecond = year == _end.Year && month == _end.Month && day == _end.Day && hour == _end.Hour && minute == _end.Minute ? _end.Second : 59;
        return CreateNumberItems(startSecond, endSecond, Math.Max(1, _steps.Second), "秒");
    }

    private static IReadOnlyList<DateTimePickerColumnItem> CreateNumberItems(int start, int end, int step, string suffix)
    {
        var items = new List<DateTimePickerColumnItem>();
        for (var value = start; value <= end; value += step)
        {
            items.Add(new DateTimePickerColumnItem
            {
                Value = value,
                Label = $"{value:00}{suffix}"
            });
        }

        if (items.Count == 0)
        {
            items.Add(new DateTimePickerColumnItem
            {
                Value = start,
                Label = $"{start:00}{suffix}"
            });
        }

        return items;
    }

    private bool Includes(DateTimePickerMode mode) => _mode >= mode;

    private DateTime Clamp(DateTime value)
    {
        if (value < _start)
        {
            return _start;
        }

        if (value > _end)
        {
            return _end;
        }

        return value;
    }

    private string FormatValue(DateTime value)
    {
        var format = _format
            .Replace("YYYY", "yyyy", StringComparison.Ordinal)
            .Replace("DD", "dd", StringComparison.Ordinal);

        return value.ToString(format, CultureInfo.InvariantCulture);
    }

    private static string GetWeekText(DayOfWeek dayOfWeek)
    {
        return dayOfWeek switch
        {
            DayOfWeek.Monday => "周一",
            DayOfWeek.Tuesday => "周二",
            DayOfWeek.Wednesday => "周三",
            DayOfWeek.Thursday => "周四",
            DayOfWeek.Friday => "周五",
            DayOfWeek.Saturday => "周六",
            _ => "周日",
        };
    }
}
