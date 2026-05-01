using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Luna.Mobile.Controls;

/// <summary>
/// <see cref="ActionSheetHost"/> 的布局模式。
/// </summary>
public enum ActionSheetTheme
{
    /// <summary>
    /// 列表模式。
    /// </summary>
    List,

    /// <summary>
    /// 宫格模式。
    /// </summary>
    Grid,
}

/// <summary>
/// 列表模式下的文本对齐预设。
/// </summary>
public enum ActionSheetAlign
{
    /// <summary>
    /// 文本居中对齐。
    /// </summary>
    Center,

    /// <summary>
    /// 文本左对齐。
    /// </summary>
    Left,
}

/// <summary>
/// <see cref="ActionSheetHost"/> 的关闭原因。
/// </summary>
public enum ActionSheetCloseReason
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
    /// 选中某一项后关闭。
    /// </summary>
    Selected,

    /// <summary>
    /// 通过代码主动关闭。
    /// </summary>
    Programmatic,
}

/// <summary>
/// <see cref="ActionSheetHost"/> 中显示的选项项。
/// </summary>
public sealed class ActionSheetItem
{
    /// <summary>
    /// 获取显示给用户的文本。
    /// </summary>
    public required string Label { get; init; }

    /// <summary>
    /// 获取可选的图标内容。
    /// </summary>
    public object? Icon { get; init; }

    /// <summary>
    /// 获取该项是否禁用。
    /// </summary>
    public bool IsDisabled { get; init; }

    /// <summary>
    /// 获取可选的文本前景色。
    /// </summary>
    public IBrush? Foreground { get; init; }

    /// <summary>
    /// 获取是否显示点状徽标。
    /// </summary>
    public bool BadgeDot { get; init; }

    /// <summary>
    /// 获取徽标数量文本（例如：8 / 99 / 1000）。
    /// </summary>
    public string? BadgeCount { get; init; }

    /// <summary>
    /// 获取当前是否包含图标。
    /// </summary>
    public bool HasIcon => Icon is not null;

    /// <summary>
    /// 获取当前是否包含任意徽标。
    /// </summary>
    public bool HasBadge => BadgeDot || !string.IsNullOrWhiteSpace(BadgeCount);

    /// <summary>
    /// 获取当前是否不包含徽标。
    /// </summary>
    public bool HasNoBadge => !HasBadge;

    /// <summary>
    /// 获取当前是否使用自定义前景色。
    /// </summary>
    public bool HasCustomForeground => Foreground is not null;

    /// <summary>
    /// 获取当前是否使用默认前景色。
    /// </summary>
    public bool HasDefaultForeground => Foreground is null;

    /// <summary>
    /// 获取当前项是否可点击。
    /// </summary>
    public bool IsEnabled => !IsDisabled;
}

/// <summary>
/// <see cref="ActionSheetHost.Show(ActionSheetOptions)"/> 的配置参数。
/// </summary>
public sealed class ActionSheetOptions
{
    /// <summary>
    /// 获取要展示的选项列表。
    /// </summary>
    public IReadOnlyList<ActionSheetItem> Items { get; init; } = Array.Empty<ActionSheetItem>();

    /// <summary>
    /// 获取可选的描述文本（显示在选项上方）。
    /// </summary>
    public string? Description { get; init; }

    /// <summary>
    /// 获取取消按钮文案。
    /// </summary>
    public string CancelText { get; init; } = "取消";

    /// <summary>
    /// 获取是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay { get; init; } = true;

    /// <summary>
    /// 获取布局模式（列表/宫格）。
    /// </summary>
    public ActionSheetTheme Theme { get; init; } = ActionSheetTheme.List;

    /// <summary>
    /// 获取列表模式下的对齐预设。
    /// </summary>
    public ActionSheetAlign Align { get; init; } = ActionSheetAlign.Center;

    /// <summary>
    /// 获取点击选项后是否自动关闭。
    /// </summary>
    public bool CloseOnSelect { get; init; } = true;
}

/// <summary>
/// 选项被选中时触发的事件参数。
/// </summary>
public sealed class ActionSheetItemSelectedEventArgs : EventArgs
{
    /// <summary>
    /// 使用被选中的项初始化事件参数。
    /// </summary>
    /// <param name="item">被选中的项。</param>
    /// <param name="index">被选中的索引。</param>
    public ActionSheetItemSelectedEventArgs(ActionSheetItem item, int index)
    {
        Item = item;
        Index = index;
    }

    /// <summary>
    /// 获取被选中的项。
    /// </summary>
    public ActionSheetItem Item { get; }

    /// <summary>
    /// 获取被选中项的索引。
    /// </summary>
    public int Index { get; }
}

/// <summary>
/// 面板关闭时触发的事件参数。
/// </summary>
public sealed class ActionSheetClosedEventArgs : EventArgs
{
    /// <summary>
    /// 使用关闭原因初始化事件参数。
    /// </summary>
    /// <param name="reason">关闭原因。</param>
    public ActionSheetClosedEventArgs(ActionSheetCloseReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// 获取关闭原因。
    /// </summary>
    public ActionSheetCloseReason Reason { get; }
}

/// <summary>
/// 动作面板宿主控件，渲染遮罩层与底部面板。
/// </summary>
/// <remarks>
/// 通常每个页面放置一个实例，并调用 <see cref="Show(ActionSheetOptions)"/> 打开。
/// 静态帮助类 <see cref="ActionSheet"/> 会使用最近一次附加到可视树的宿主作为 <see cref="Current"/>。
/// </remarks>
[TemplatePart(OverlayPartName, typeof(Border))]
[TemplatePart(SheetPartName, typeof(Border))]
[TemplatePart(CancelButtonPartName, typeof(Button))]
public sealed class ActionSheetHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string SheetPartName = "PART_Sheet";
    private const string CancelButtonPartName = "PART_CancelButton";

    private static ActionSheetHost? _current;

    private Border? _overlay;
    private Border? _sheet;
    private Button? _cancelButton;
    private ActionSheetCloseReason _closeReason = ActionSheetCloseReason.Unknown;

    private bool _isOverlayVisible;
    private bool _isSheetVisible;
    private bool _isDescriptionVisible;
    private bool _isListVisible = true;
    private bool _isGridVisible;
    private bool _isAlignLeft;
    private TextAlignment _itemTextAlignment = TextAlignment.Center;
    private HorizontalAlignment _itemHorizontalAlignment = HorizontalAlignment.Center;
    private int _animationVersion;

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(IsOpen));

    /// <inheritdoc cref="Description" />
    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<ActionSheetHost, string?>(nameof(Description));

    /// <inheritdoc cref="IsDescriptionVisible" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsDescriptionVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsDescriptionVisible),
            o => o.IsDescriptionVisible);

    /// <inheritdoc cref="CancelText" />
    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<ActionSheetHost, string>(nameof(CancelText), "取消");

    /// <inheritdoc cref="ShowOverlay" />
    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(ShowOverlay), true);

    /// <inheritdoc cref="IsOverlayVisible" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    /// <inheritdoc cref="IsSheetVisible" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsSheetVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsSheetVisible),
            o => o.IsSheetVisible);

    /// <inheritdoc cref="Items" />
    public static readonly StyledProperty<IReadOnlyList<ActionSheetItem>> ItemsProperty =
        AvaloniaProperty.Register<ActionSheetHost, IReadOnlyList<ActionSheetItem>>(nameof(Items), Array.Empty<ActionSheetItem>());

    /// <inheritdoc cref="Theme" />
    public static readonly StyledProperty<ActionSheetTheme> ThemeProperty =
        AvaloniaProperty.Register<ActionSheetHost, ActionSheetTheme>(nameof(Theme), ActionSheetTheme.List);

    /// <inheritdoc cref="Align" />
    public static readonly StyledProperty<ActionSheetAlign> AlignProperty =
        AvaloniaProperty.Register<ActionSheetHost, ActionSheetAlign>(nameof(Align), ActionSheetAlign.Center);

    /// <inheritdoc cref="IsListVisible" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsListVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsListVisible),
            o => o.IsListVisible);

    /// <inheritdoc cref="IsGridVisible" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsGridVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsGridVisible),
            o => o.IsGridVisible);

    /// <inheritdoc cref="IsAlignLeft" />
    public static readonly DirectProperty<ActionSheetHost, bool> IsAlignLeftProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsAlignLeft),
            o => o.IsAlignLeft);

    /// <inheritdoc cref="ItemTextAlignment" />
    public static readonly DirectProperty<ActionSheetHost, TextAlignment> ItemTextAlignmentProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, TextAlignment>(
            nameof(ItemTextAlignment),
            o => o.ItemTextAlignment);

    /// <inheritdoc cref="ItemHorizontalAlignment" />
    public static readonly DirectProperty<ActionSheetHost, HorizontalAlignment> ItemHorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, HorizontalAlignment>(
            nameof(ItemHorizontalAlignment),
            o => o.ItemHorizontalAlignment);

    /// <inheritdoc cref="ItemClickCommand" />
    public static readonly DirectProperty<ActionSheetHost, ICommand> ItemClickCommandProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, ICommand>(
            nameof(ItemClickCommand),
            o => o.ItemClickCommand);

    /// <inheritdoc cref="CloseOnSelect" />
    public static readonly StyledProperty<bool> CloseOnSelectProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(CloseOnSelect), true);

    static ActionSheetHost()
    {
        IsOpenProperty.Changed.AddClassHandler<ActionSheetHost>((control, args) =>
        {
            control.HandleIsOpenChanged(args.GetNewValue<bool>());
        });
        ShowOverlayProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdateOverlayVisible());

        DescriptionProperty.Changed.AddClassHandler<ActionSheetHost>((control, args) =>
        {
            control.IsDescriptionVisible = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });

        ThemeProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdatePseudoClasses());
        AlignProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdatePseudoClasses());
    }

    /// <summary>
    /// 初始化 <see cref="ActionSheetHost" /> 的新实例。
    /// </summary>
    public ActionSheetHost()
    {
        ItemClickCommand = new ItemClickCommandImpl(this);
        IsDescriptionVisible = !string.IsNullOrWhiteSpace(Description);
        IsSheetVisible = IsOpen;
        UpdateOverlayVisible();
        UpdatePseudoClasses();
    }

    /// <summary>
    /// 获取当前附加到可视树的动作面板宿主实例。
    /// </summary>
    public static ActionSheetHost? Current => _current;

    /// <summary>
    /// 选中某个动作项后触发。
    /// </summary>
    public event EventHandler<ActionSheetItemSelectedEventArgs>? Selected;

    /// <summary>
    /// 点击取消按钮后触发。
    /// </summary>
    public event EventHandler? CancelRequested;

    /// <summary>
    /// 动作面板关闭后触发。
    /// </summary>
    public event EventHandler<ActionSheetClosedEventArgs>? Closed;

    /// <summary>
    /// 获取或设置当前是否打开动作面板。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置描述文本。
    /// </summary>
    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// 获取当前是否显示描述文本。
    /// </summary>
    public bool IsDescriptionVisible
    {
        get => _isDescriptionVisible;
        private set => SetAndRaise(IsDescriptionVisibleProperty, ref _isDescriptionVisible, value);
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
    /// 获取或设置是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
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
    /// 获取或设置动作项集合。
    /// </summary>
    public IReadOnlyList<ActionSheetItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    /// <summary>
    /// 获取或设置展示模式。
    /// </summary>
    public ActionSheetTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置列表文本对齐方式。
    /// </summary>
    public ActionSheetAlign Align
    {
        get => GetValue(AlignProperty);
        set => SetValue(AlignProperty, value);
    }

    /// <summary>
    /// 获取当前是否显示列表布局。
    /// </summary>
    public bool IsListVisible
    {
        get => _isListVisible;
        private set => SetAndRaise(IsListVisibleProperty, ref _isListVisible, value);
    }

    /// <summary>
    /// 获取当前是否显示宫格布局。
    /// </summary>
    public bool IsGridVisible
    {
        get => _isGridVisible;
        private set => SetAndRaise(IsGridVisibleProperty, ref _isGridVisible, value);
    }

    /// <summary>
    /// 获取当前是否为左对齐。
    /// </summary>
    public bool IsAlignLeft
    {
        get => _isAlignLeft;
        private set => SetAndRaise(IsAlignLeftProperty, ref _isAlignLeft, value);
    }

    /// <summary>
    /// 获取当前动作项文本对齐方式。
    /// </summary>
    public TextAlignment ItemTextAlignment
    {
        get => _itemTextAlignment;
        private set => SetAndRaise(ItemTextAlignmentProperty, ref _itemTextAlignment, value);
    }

    /// <summary>
    /// 获取当前动作项水平对齐方式。
    /// </summary>
    public HorizontalAlignment ItemHorizontalAlignment
    {
        get => _itemHorizontalAlignment;
        private set => SetAndRaise(ItemHorizontalAlignmentProperty, ref _itemHorizontalAlignment, value);
    }

    /// <summary>
    /// 获取动作项点击命令。
    /// </summary>
    public ICommand ItemClickCommand { get; }

    /// <summary>
    /// 获取或设置点击动作项后是否自动关闭。
    /// </summary>
    public bool CloseOnSelect
    {
        get => GetValue(CloseOnSelectProperty);
        set => SetValue(CloseOnSelectProperty, value);
    }

    /// <summary>
    /// 使用指定参数打开动作面板。
    /// </summary>
    /// <param name="options">动作面板配置参数。</param>
    public void Show(ActionSheetOptions options)
    {
        Items = options.Items;
        Description = options.Description;
        CancelText = options.CancelText;
        ShowOverlay = options.ShowOverlay;
        Theme = options.Theme;
        Align = options.Align;
        CloseOnSelect = options.CloseOnSelect;

        _closeReason = ActionSheetCloseReason.Unknown;
        IsOpen = true;
    }

    /// <summary>
    /// 使用指定原因关闭动作面板。
    /// </summary>
    /// <param name="reason">关闭原因。</param>
    public void Close(ActionSheetCloseReason reason = ActionSheetCloseReason.Programmatic)
    {
        if (!IsOpen)
        {
            return;
        }

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
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
        {
            _overlay.PointerPressed -= OnOverlayPointerPressed;
        }

        _sheet = null;
        if (_cancelButton is not null)
        {
            _cancelButton.Click -= OnCancelButtonClick;
        }

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPointerPressed;
        }

        _sheet = e.NameScope.Find<Border>(SheetPartName);

        _cancelButton = e.NameScope.Find<Button>(CancelButtonPartName);
        if (_cancelButton is not null)
        {
            _cancelButton.Click += OnCancelButtonClick;
        }
    }

    private void OnOverlayPointerPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsOpen || !ShowOverlay)
        {
            return;
        }

        Close(ActionSheetCloseReason.Overlay);
    }

    private void OnCancelButtonClick(object? sender, EventArgs e)
    {
        CancelRequested?.Invoke(this, EventArgs.Empty);
        Close(ActionSheetCloseReason.Cancel);
    }

    private async void HandleIsOpenChanged(bool isOpen)
    {
        var version = ++_animationVersion;

        if (isOpen)
        {
            IsSheetVisible = true;
            UpdateOverlayVisible();
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

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = ShowOverlay && (IsOpen || IsSheetVisible);
    }

    private async Task RunOpenAnimationAsync(int version)
    {
        if (_sheet is null && _overlay is null)
        {
            return;
        }

        var tasks = new List<Task>(2);

        if (_overlay is not null && ShowOverlay)
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
            Closed?.Invoke(this, new ActionSheetClosedEventArgs(_closeReason));
            return;
        }

        var tasks = new List<Task>(2);

        if (_overlay is not null && ShowOverlay)
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
        Closed?.Invoke(this, new ActionSheetClosedEventArgs(_closeReason));
    }

    private double GetClosedOffset()
    {
        var height = _sheet?.Bounds.Height ?? double.NaN;
        return OverlayHostAnimationHelper.ResolveDistance(height, 320);
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":list", Theme == ActionSheetTheme.List);
        PseudoClasses.Set(":grid", Theme == ActionSheetTheme.Grid);
        PseudoClasses.Set(":align-center", Align == ActionSheetAlign.Center);
        PseudoClasses.Set(":align-left", Align == ActionSheetAlign.Left);

        IsListVisible = Theme == ActionSheetTheme.List;
        IsGridVisible = Theme == ActionSheetTheme.Grid;
        IsAlignLeft = Align == ActionSheetAlign.Left;

        ItemTextAlignment = IsAlignLeft ? TextAlignment.Left : TextAlignment.Center;
        ItemHorizontalAlignment = IsAlignLeft ? HorizontalAlignment.Left : HorizontalAlignment.Center;
    }

    private void OnItemClicked(ActionSheetItem item)
    {
        if (item.IsDisabled)
        {
            return;
        }

        var index = Items.IndexOf(item);
        Selected?.Invoke(this, new ActionSheetItemSelectedEventArgs(item, index));

        if (CloseOnSelect)
        {
            Close(ActionSheetCloseReason.Selected);
        }
    }

    private sealed class ItemClickCommandImpl : ICommand
    {
        private readonly ActionSheetHost _host;

        public ItemClickCommandImpl(ActionSheetHost host)
        {
            _host = host;
        }

        public bool CanExecute(object? parameter)
        {
            return parameter is ActionSheetItem { IsDisabled: false };
        }

        public void Execute(object? parameter)
        {
            if (parameter is not ActionSheetItem item)
            {
                return;
            }

            _host.OnItemClicked(item);
        }

        public event EventHandler? CanExecuteChanged;
    }
}

/// <summary>
/// ActionSheet 的静态入口，依赖页面中的 <see cref="ActionSheetHost" />。
/// </summary>
public static class ActionSheet
{
    /// <summary>
    /// 使用指定参数显示动作面板。
    /// </summary>
    /// <param name="options">动作面板配置参数。</param>
    public static void Show(ActionSheetOptions options)
    {
        ActionSheetHost.Current?.Show(options);
    }

    /// <summary>
    /// 以编程方式关闭当前动作面板。
    /// </summary>
    public static void Close()
    {
        ActionSheetHost.Current?.Close(ActionSheetCloseReason.Programmatic);
    }
}

internal static class ActionSheetListExtensions
{
    public static int IndexOf<T>(this IReadOnlyList<T> list, T item)
    {
        for (var i = 0; i < list.Count; i++)
        {
            if (Equals(list[i], item))
            {
                return i;
            }
        }

        return -1;
    }
}
