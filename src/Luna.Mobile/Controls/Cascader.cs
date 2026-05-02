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
using System.Linq;
using System.Threading.Tasks;

namespace Luna.Mobile.Controls;

/// <summary>
/// Cascader 的展示风格。
/// </summary>
public enum CascaderTheme
{
    /// <summary>
    /// 逐级步骤式展示。
    /// </summary>
    Step,

    /// <summary>
    /// 标签页式展示。
    /// </summary>
    Tab,
}

/// <summary>
/// Cascader 的关闭原因。
/// </summary>
public enum CascaderCloseReason
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
    /// 点击关闭按钮关闭。
    /// </summary>
    CloseButton,

    /// <summary>
    /// 完成选择后关闭。
    /// </summary>
    Finish,

    /// <summary>
    /// 通过代码主动关闭。
    /// </summary>
    Programmatic,
}

/// <summary>
/// Cascader 的单个节点定义。
/// </summary>
public sealed class CascaderOption
{
    /// <summary>
    /// 获取节点显示文本。
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// 获取节点值。
    /// </summary>
    public string Value { get; init; } = string.Empty;

    /// <summary>
    /// 获取是否禁用该节点。
    /// </summary>
    public bool IsDisabled { get; init; }

    /// <summary>
    /// 获取子节点集合。
    /// </summary>
    public IReadOnlyList<CascaderOption> Children { get; init; } = Array.Empty<CascaderOption>();
}

/// <summary>
/// Cascader 的显示参数。
/// </summary>
public sealed class CascaderOptions
{
    /// <summary>
    /// 获取可选项数据源。
    /// </summary>
    public IReadOnlyList<CascaderOption> Options { get; init; } = Array.Empty<CascaderOption>();

    /// <summary>
    /// 获取标题文本。
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 获取关闭按钮文本。
    /// </summary>
    public string CloseText { get; init; } = "关闭";

    /// <summary>
    /// 获取未选择时的占位文案。
    /// </summary>
    public string Placeholder { get; init; } = "选择选项";

    /// <summary>
    /// 获取每级的辅助标题。
    /// </summary>
    public IReadOnlyList<string> SubTitles { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 获取展示风格。
    /// </summary>
    public CascaderTheme Theme { get; init; } = CascaderTheme.Step;

    /// <summary>
    /// 获取初始选中值。
    /// </summary>
    public string? Value { get; init; }

    /// <summary>
    /// 获取是否允许父节点独立选中。
    /// </summary>
    public bool CheckStrictly { get; init; }

    /// <summary>
    /// 获取是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick { get; init; } = true;

    /// <summary>
    /// 获取面板高度。
    /// </summary>
    public double SheetHeight { get; init; } = 360;
}

/// <summary>
/// Cascader 选项点击事件参数。
/// </summary>
public sealed class CascaderPickedEventArgs : EventArgs
{
    public CascaderPickedEventArgs(string value, string label, int index, int level, IReadOnlyList<CascaderOption> selectedOptions)
    {
        Value = value;
        Label = label;
        Index = index;
        Level = level;
        SelectedOptions = selectedOptions;
    }

    /// <summary>
    /// 获取当前节点值。
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 获取当前节点文本。
    /// </summary>
    public string Label { get; }

    /// <summary>
    /// 获取当前层中的索引。
    /// </summary>
    public int Index { get; }

    /// <summary>
    /// 获取当前层级。
    /// </summary>
    public int Level { get; }

    /// <summary>
    /// 获取当前路径的已选节点集合。
    /// </summary>
    public IReadOnlyList<CascaderOption> SelectedOptions { get; }
}

/// <summary>
/// Cascader 完成选择事件参数。
/// </summary>
public sealed class CascaderChangedEventArgs : EventArgs
{
    public CascaderChangedEventArgs(string value, IReadOnlyList<CascaderOption> selectedOptions)
    {
        Value = value;
        SelectedOptions = selectedOptions;
    }

    /// <summary>
    /// 获取最终选中的节点值。
    /// </summary>
    public string Value { get; }

    /// <summary>
    /// 获取最终选中的路径节点集合。
    /// </summary>
    public IReadOnlyList<CascaderOption> SelectedOptions { get; }
}

/// <summary>
/// Cascader 关闭事件参数。
/// </summary>
public sealed class CascaderClosedEventArgs : EventArgs
{
    public CascaderClosedEventArgs(CascaderCloseReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// 获取关闭原因。
    /// </summary>
    public CascaderCloseReason Reason { get; }
}

/// <summary>
/// Cascader 顶部层级标签项。
/// </summary>
public sealed class CascaderTabItem
{
    /// <summary>
    /// 获取层级索引。
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// 获取主标题文本。
    /// </summary>
    public string Title { get; init; } = string.Empty;

    /// <summary>
    /// 获取辅助标题文本。
    /// </summary>
    public string? Subtitle { get; init; }

    /// <summary>
    /// 获取当前是否存在辅助标题。
    /// </summary>
    public bool HasSubtitle { get; init; }

    /// <summary>
    /// 获取当前是否为激活层。
    /// </summary>
    public bool IsActive { get; init; }

    /// <summary>
    /// 获取当前是否为占位层。
    /// </summary>
    public bool IsPlaceholder { get; init; }
}

/// <summary>
/// Cascader 当前层的可选项包装。
/// </summary>
public sealed class CascaderItem
{
    /// <summary>
    /// 获取所在层级。
    /// </summary>
    public int Level { get; init; }

    /// <summary>
    /// 获取该项在当前层的索引。
    /// </summary>
    public int Index { get; init; }

    /// <summary>
    /// 获取显示文本。
    /// </summary>
    public string Label { get; init; } = string.Empty;

    /// <summary>
    /// 获取是否禁用。
    /// </summary>
    public bool IsEnabled { get; init; } = true;

    /// <summary>
    /// 获取是否为当前选中项。
    /// </summary>
    public bool IsSelected { get; init; }

    /// <summary>
    /// 获取是否存在子节点。
    /// </summary>
    public bool HasChildren { get; init; }

    internal CascaderOption Option { get; init; } = new();
}

/// <summary>
/// Cascader 的静态入口，依赖页面中的 <see cref="CascaderHost"/>。
/// </summary>
public static class Cascader
{
    /// <summary>
    /// 使用指定参数显示级联选择器。
    /// </summary>
    public static void Show(CascaderOptions options) => CascaderHost.Current?.Show(options);

    /// <summary>
    /// 以编程方式关闭当前级联选择器。
    /// </summary>
    public static void Close() => CascaderHost.Current?.Close(CascaderCloseReason.Programmatic);
}

/// <summary>
/// Cascader 宿主控件，负责渲染底部弹层并处理树形层级联动。
/// </summary>
[TemplatePart(OverlayPartName, typeof(Border))]
[TemplatePart(SheetPartName, typeof(Border))]
[TemplatePart(CloseButtonPartName, typeof(Button))]
[TemplatePart(TabsPartName, typeof(ItemsControl))]
[TemplatePart(OptionsPartName, typeof(ItemsControl))]
public sealed class CascaderHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string SheetPartName = "PART_Sheet";
    private const string CloseButtonPartName = "PART_CloseButton";
    private const string TabsPartName = "PART_Tabs";
    private const string OptionsPartName = "PART_Options";

    private static CascaderHost? _current;

    private Border? _overlay;
    private Border? _sheet;
    private Button? _closeButton;
    private ItemsControl? _tabsControl;
    private ItemsControl? _optionsControl;
    private IReadOnlyList<CascaderOption> _options = Array.Empty<CascaderOption>();
    private readonly List<CascaderOption> _selectedPath = [];
    private IReadOnlyList<CascaderTabItem> _tabs = Array.Empty<CascaderTabItem>();
    private IReadOnlyList<CascaderItem> _currentItems = Array.Empty<CascaderItem>();
    private IReadOnlyList<string> _subTitles = Array.Empty<string>();
    private CascaderTheme _theme = CascaderTheme.Step;
    private string _placeholder = "选择选项";
    private bool _checkStrictly;
    private bool _isOverlayVisible;
    private bool _isSheetVisible;
    private bool _hasTitle;
    private int _activeLevel;
    private int _animationVersion;
    private CascaderCloseReason _closeReason = CascaderCloseReason.Unknown;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<CascaderHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<CascaderHost, bool>(nameof(CloseOnOverlayClick), true);

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<CascaderHost, string?>(nameof(Title));

    public static readonly StyledProperty<string> CloseTextProperty =
        AvaloniaProperty.Register<CascaderHost, string>(nameof(CloseText), "关闭");

    public static readonly StyledProperty<double> SheetHeightProperty =
        AvaloniaProperty.Register<CascaderHost, double>(nameof(SheetHeight), 360);

    public static readonly DirectProperty<CascaderHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<CascaderHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    public static readonly DirectProperty<CascaderHost, bool> IsSheetVisibleProperty =
        AvaloniaProperty.RegisterDirect<CascaderHost, bool>(
            nameof(IsSheetVisible),
            o => o.IsSheetVisible);

    public static readonly DirectProperty<CascaderHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<CascaderHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    public static readonly DirectProperty<CascaderHost, IReadOnlyList<CascaderTabItem>> TabsProperty =
        AvaloniaProperty.RegisterDirect<CascaderHost, IReadOnlyList<CascaderTabItem>>(
            nameof(Tabs),
            o => o.Tabs);

    public static readonly DirectProperty<CascaderHost, IReadOnlyList<CascaderItem>> CurrentItemsProperty =
        AvaloniaProperty.RegisterDirect<CascaderHost, IReadOnlyList<CascaderItem>>(
            nameof(CurrentItems),
            o => o.CurrentItems);

    /// <summary>
    /// 获取当前附加到可视树的级联选择器宿主实例。
    /// </summary>
    public static CascaderHost? Current => _current;

    /// <summary>
    /// 当前点击某一层选项时触发。
    /// </summary>
    public event EventHandler<CascaderPickedEventArgs>? Picked;

    /// <summary>
    /// 完成选择后触发。
    /// </summary>
    public event EventHandler<CascaderChangedEventArgs>? Changed;

    /// <summary>
    /// 关闭后触发。
    /// </summary>
    public event EventHandler<CascaderClosedEventArgs>? Closed;

    static CascaderHost()
    {
        IsOpenProperty.Changed.AddClassHandler<CascaderHost>((control, args) =>
        {
            control.HandleIsOpenChanged(args.GetNewValue<bool>());
        });
        TitleProperty.Changed.AddClassHandler<CascaderHost>((control, args) =>
        {
            control.HasTitle = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });
    }

    public CascaderHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
        IsSheetVisible = IsOpen;
        UpdateOverlayVisible();
        UpdateThemeState();
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
    /// 获取或设置关闭按钮文本。
    /// </summary>
    public string CloseText
    {
        get => GetValue(CloseTextProperty);
        set => SetValue(CloseTextProperty, value);
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
    /// 获取当前面板是否仍保持渲染。
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
    /// 获取当前顶部层级标签集合。
    /// </summary>
    public IReadOnlyList<CascaderTabItem> Tabs
    {
        get => _tabs;
        private set => SetAndRaise(TabsProperty, ref _tabs, value);
    }

    /// <summary>
    /// 获取当前激活层的选项集合。
    /// </summary>
    public IReadOnlyList<CascaderItem> CurrentItems
    {
        get => _currentItems;
        private set => SetAndRaise(CurrentItemsProperty, ref _currentItems, value);
    }

    /// <summary>
    /// 使用指定参数打开级联选择器。
    /// </summary>
    public void Show(CascaderOptions options)
    {
        _closeReason = CascaderCloseReason.Unknown;
        _options = options.Options;
        _subTitles = options.SubTitles;
        _placeholder = string.IsNullOrWhiteSpace(options.Placeholder) ? "选择选项" : options.Placeholder;
        _checkStrictly = options.CheckStrictly;
        _theme = options.Theme;

        Title = options.Title;
        CloseText = options.CloseText;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        SheetHeight = options.SheetHeight;

        _selectedPath.Clear();
        if (!string.IsNullOrWhiteSpace(options.Value) &&
            TryFindPathByValue(_options, options.Value, out var selectedPath))
        {
            _selectedPath.AddRange(selectedPath);
        }

        _activeLevel = ResolveInitialActiveLevel();
        RebuildState();
        IsOpen = true;
    }

    /// <summary>
    /// 使用指定原因关闭级联选择器。
    /// </summary>
    public void Close(CascaderCloseReason reason)
    {
        _closeReason = reason;
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

        DetachHandlers();
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachHandlers();

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        _sheet = e.NameScope.Find<Border>(SheetPartName);
        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);
        _tabsControl = e.NameScope.Find<ItemsControl>(TabsPartName);
        _optionsControl = e.NameScope.Find<ItemsControl>(OptionsPartName);

        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPressed;
        }

        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseClick;
        }

        ScheduleInteractionWireup();
    }

    private void DetachHandlers()
    {
        if (_overlay is not null)
        {
            _overlay.PointerPressed -= OnOverlayPressed;
        }

        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseClick;
        }

        DetachTabHandlers();
        DetachOptionHandlers();
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!IsOpen || !CloseOnOverlayClick)
        {
            return;
        }

        e.Handled = true;
        Close(CascaderCloseReason.Overlay);
    }

    private void OnCloseClick(object? sender, EventArgs e)
    {
        if (!IsOpen)
        {
            return;
        }

        Close(CascaderCloseReason.CloseButton);
    }

    private void ScheduleInteractionWireup()
    {
        Dispatcher.UIThread.Post(() =>
        {
            AttachTabHandlers();
            AttachOptionHandlers();
        }, DispatcherPriority.Loaded);
    }

    private void AttachTabHandlers()
    {
        if (_tabsControl is null)
        {
            return;
        }

        DetachTabHandlers();

        foreach (var button in _tabsControl.GetVisualDescendants().OfType<Button>())
        {
            button.Click += OnTabClick;
        }

        UpdateTabButtonStates();
    }

    private void DetachTabHandlers()
    {
        if (_tabsControl is null)
        {
            return;
        }

        foreach (var button in _tabsControl.GetVisualDescendants().OfType<Button>())
        {
            button.Click -= OnTabClick;
        }
    }

    private void AttachOptionHandlers()
    {
        if (_optionsControl is null)
        {
            return;
        }

        DetachOptionHandlers();

        foreach (var button in _optionsControl.GetVisualDescendants().OfType<Button>())
        {
            button.Click += OnOptionClick;
        }

        UpdateOptionButtonStates();
    }

    private void DetachOptionHandlers()
    {
        if (_optionsControl is null)
        {
            return;
        }

        foreach (var button in _optionsControl.GetVisualDescendants().OfType<Button>())
        {
            button.Click -= OnOptionClick;
        }
    }

    private void OnTabClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button { Tag: CascaderTabItem tab })
        {
            return;
        }

        _activeLevel = tab.Level;
        RebuildState();
    }

    private void OnOptionClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button { Tag: CascaderItem item } || !item.IsEnabled)
        {
            return;
        }

        HandleItemSelected(item);
    }

    private void HandleItemSelected(CascaderItem item)
    {
        TrimPathToLevel(item.Level);
        _selectedPath.Add(item.Option);

        var pickedArgs = new CascaderPickedEventArgs(
            item.Option.Value,
            item.Option.Label,
            item.Index,
            item.Level,
            _selectedPath.ToArray());
        Picked?.Invoke(this, pickedArgs);

        var shouldFinish = _checkStrictly || item.Option.Children.Count == 0;
        if (shouldFinish)
        {
            Changed?.Invoke(this, new CascaderChangedEventArgs(item.Option.Value, _selectedPath.ToArray()));
            Close(CascaderCloseReason.Finish);
            return;
        }

        _activeLevel = item.Level + 1;
        RebuildState();
    }

    private void TrimPathToLevel(int level)
    {
        while (_selectedPath.Count > level)
        {
            _selectedPath.RemoveAt(_selectedPath.Count - 1);
        }
    }

    private int ResolveInitialActiveLevel()
    {
        if (_selectedPath.Count == 0)
        {
            return 0;
        }

        var last = _selectedPath[^1];
        if (!_checkStrictly && last.Children.Count > 0)
        {
            return _selectedPath.Count;
        }

        return _selectedPath.Count - 1;
    }

    private void RebuildState()
    {
        var totalLevels = ResolveVisibleLevelCount();
        if (totalLevels <= 0)
        {
            totalLevels = 1;
        }

        _activeLevel = Math.Clamp(_activeLevel, 0, totalLevels - 1);

        var tabs = new List<CascaderTabItem>(totalLevels);
        for (var i = 0; i < totalLevels; i++)
        {
            var selected = i < _selectedPath.Count ? _selectedPath[i] : null;
            tabs.Add(new CascaderTabItem
            {
                Level = i,
                Title = selected?.Label ?? _placeholder,
                Subtitle = i < _subTitles.Count ? _subTitles[i] : null,
                HasSubtitle = i < _subTitles.Count && !string.IsNullOrWhiteSpace(_subTitles[i]),
                IsActive = i == _activeLevel,
                IsPlaceholder = selected is null,
            });
        }

        Tabs = tabs;
        CurrentItems = BuildCurrentItems();
        UpdateThemeState();
        ScheduleInteractionWireup();
    }

    private int ResolveVisibleLevelCount()
    {
        if (_selectedPath.Count == 0)
        {
            return 1;
        }

        var count = _selectedPath.Count;
        if (!_checkStrictly && _selectedPath[^1].Children.Count > 0)
        {
            count++;
        }

        return count;
    }

    private IReadOnlyList<CascaderItem> BuildCurrentItems()
    {
        var options = GetOptionsForLevel(_activeLevel);
        var selectedValue = _activeLevel < _selectedPath.Count ? _selectedPath[_activeLevel].Value : null;
        var items = new CascaderItem[options.Count];

        for (var i = 0; i < options.Count; i++)
        {
            var option = options[i];
            items[i] = new CascaderItem
            {
                Level = _activeLevel,
                Index = i,
                Label = option.Label,
                IsEnabled = !option.IsDisabled,
                IsSelected = string.Equals(option.Value, selectedValue, StringComparison.Ordinal),
                HasChildren = option.Children.Count > 0,
                Option = option,
            };
        }

        return items;
    }

    private IReadOnlyList<CascaderOption> GetOptionsForLevel(int level)
    {
        if (level <= 0)
        {
            return _options;
        }

        if (level - 1 >= _selectedPath.Count)
        {
            return Array.Empty<CascaderOption>();
        }

        return _selectedPath[level - 1].Children;
    }

    private void UpdateThemeState()
    {
        PseudoClasses.Set(":step", _theme == CascaderTheme.Step);
        PseudoClasses.Set(":tab", _theme == CascaderTheme.Tab);
    }

    private void UpdateTabButtonStates()
    {
        if (_tabsControl is null)
        {
            return;
        }

        foreach (var button in _tabsControl.GetVisualDescendants().OfType<Button>())
        {
            if (button.Tag is not CascaderTabItem item)
            {
                continue;
            }

            SetButtonClass(button, "active", item.IsActive);
            SetButtonClass(button, "placeholder", item.IsPlaceholder);
        }
    }

    private void UpdateOptionButtonStates()
    {
        if (_optionsControl is null)
        {
            return;
        }

        foreach (var button in _optionsControl.GetVisualDescendants().OfType<Button>())
        {
            if (button.Tag is not CascaderItem item)
            {
                continue;
            }

            SetButtonClass(button, "selected", item.IsSelected);
        }
    }

    private static void SetButtonClass(Button button, string className, bool enabled)
    {
        if (enabled)
        {
            if (!button.Classes.Contains(className))
            {
                button.Classes.Add(className);
            }
        }
        else
        {
            button.Classes.Remove(className);
        }
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

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen || IsSheetVisible;
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
            Closed?.Invoke(this, new CascaderClosedEventArgs(_closeReason));
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
        Closed?.Invoke(this, new CascaderClosedEventArgs(_closeReason));
    }

    private double GetClosedOffset()
    {
        return OverlayHostAnimationHelper.ResolveDistance(SheetHeight, 360);
    }

    private void RefreshSheetLayout()
    {
        if (_sheet is null)
        {
            return;
        }

        _sheet.InvalidateMeasure();
        _sheet.InvalidateArrange();

        foreach (var scrollViewer in _sheet.GetVisualDescendants().OfType<ScrollViewer>())
        {
            scrollViewer.InvalidateMeasure();
            scrollViewer.InvalidateArrange();
        }
    }

    private static bool TryFindPathByValue(
        IReadOnlyList<CascaderOption> options,
        string targetValue,
        out List<CascaderOption> path)
    {
        for (var i = 0; i < options.Count; i++)
        {
            var option = options[i];
            if (string.Equals(option.Value, targetValue, StringComparison.Ordinal))
            {
                path = [option];
                return true;
            }

            if (option.Children.Count > 0 &&
                TryFindPathByValue(option.Children, targetValue, out path))
            {
                path.Insert(0, option);
                return true;
            }
        }

        path = [];
        return false;
    }
}
