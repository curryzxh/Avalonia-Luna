using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Data;
using Avalonia.Input;
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Luna.Mobile.Controls;

/// <summary>
/// 步骤条方向。
/// </summary>
public enum StepsLayout
{
    /// <summary>
    /// 横向。
    /// </summary>
    Horizontal,

    /// <summary>
    /// 纵向。
    /// </summary>
    Vertical,
}

/// <summary>
/// 步骤条主题。
/// </summary>
public enum StepsTheme
{
    /// <summary>
    /// 默认主题。
    /// </summary>
    Default,

    /// <summary>
    /// 点状主题。
    /// </summary>
    Dot,
}

/// <summary>
/// 步骤状态。
/// </summary>
public enum StepStatus
{
    /// <summary>
    /// 未开始。
    /// </summary>
    Default,

    /// <summary>
    /// 进行中。
    /// </summary>
    Process,

    /// <summary>
    /// 已完成。
    /// </summary>
    Finish,

    /// <summary>
    /// 错误。
    /// </summary>
    Error,
}

/// <summary>
/// 当前步骤变化事件参数。
/// </summary>
/// <param name="current">新的当前步骤值。</param>
/// <param name="previous">旧的当前步骤值。</param>
/// <param name="item">触发变化的步骤项。</param>
/// <param name="index">触发变化的逻辑索引。</param>
public sealed class StepsCurrentChangedEventArgs(string? current, string? previous, StepItem? item, int index) : EventArgs
{
    /// <summary>
    /// 获取新的当前步骤值。
    /// </summary>
    public string? Current { get; } = current;

    /// <summary>
    /// 获取旧的当前步骤值。
    /// </summary>
    public string? Previous { get; } = previous;

    /// <summary>
    /// 获取触发变化的步骤项。
    /// </summary>
    public StepItem? Item { get; } = item;

    /// <summary>
    /// 获取触发变化的逻辑索引。
    /// </summary>
    public int Index { get; } = index;
}

/// <summary>
/// 步骤条控件。
/// </summary>
public sealed class Steps : ItemsControl
{
    private INotifyCollectionChanged? _inlineItemsNotifier;
    private INotifyCollectionChanged? _itemsSourceNotifier;
    private readonly List<StepItem> _attachedItems = [];
    private int _resolvedItemCount = 1;

    /// <inheritdoc cref="Current" />
    public static readonly StyledProperty<string?> CurrentProperty =
        AvaloniaProperty.Register<Steps, string?>(
            nameof(Current),
            "0",
            defaultBindingMode: BindingMode.TwoWay);

    /// <inheritdoc cref="CurrentStatus" />
    public static readonly StyledProperty<StepStatus> CurrentStatusProperty =
        AvaloniaProperty.Register<Steps, StepStatus>(nameof(CurrentStatus), StepStatus.Process);

    /// <inheritdoc cref="Layout" />
    public static readonly StyledProperty<StepsLayout> LayoutProperty =
        AvaloniaProperty.Register<Steps, StepsLayout>(nameof(Layout), StepsLayout.Horizontal);

    /// <inheritdoc cref="StepTheme" />
    public static readonly StyledProperty<StepsTheme> StepThemeProperty =
        AvaloniaProperty.Register<Steps, StepsTheme>(nameof(StepTheme), StepsTheme.Default);

    /// <inheritdoc cref="IsReadOnly" />
    public static readonly StyledProperty<bool> IsReadOnlyProperty =
        AvaloniaProperty.Register<Steps, bool>(nameof(IsReadOnly));

    /// <inheritdoc cref="ResolvedItemCount" />
    public static readonly DirectProperty<Steps, int> ResolvedItemCountProperty =
        AvaloniaProperty.RegisterDirect<Steps, int>(
            nameof(ResolvedItemCount),
            o => o.ResolvedItemCount);

    static Steps()
    {
        CurrentProperty.Changed.AddClassHandler<Steps>((control, args) =>
        {
            control.UpdateStepsState();
            control.CurrentChanged?.Invoke(
                control,
                new StepsCurrentChangedEventArgs(
                    args.GetNewValue<string?>(),
                    args.GetOldValue<string?>(),
                    null,
                    control.ResolveCurrentIndex()));
        });

        CurrentStatusProperty.Changed.AddClassHandler<Steps>((control, _) => control.UpdateStepsState());
        LayoutProperty.Changed.AddClassHandler<Steps>((control, args) =>
        {
            control.UpdateLayoutPseudo(args.GetNewValue<StepsLayout>());
            control.UpdateStepsState();
        });
        StepThemeProperty.Changed.AddClassHandler<Steps>((control, args) =>
        {
            control.UpdateThemePseudo(args.GetNewValue<StepsTheme>());
            control.UpdateStepsState();
        });
        IsReadOnlyProperty.Changed.AddClassHandler<Steps>((control, _) => control.UpdateStepsState());
        ItemsSourceProperty.Changed.AddClassHandler<Steps>((control, _) => control.AttachItemsSourceNotifier());
    }

    /// <summary>
    /// 当前步骤变化时触发。
    /// </summary>
    public event EventHandler<StepsCurrentChangedEventArgs>? CurrentChanged;

    /// <summary>
    /// 初始化步骤条控件。
    /// </summary>
    public Steps()
    {
        if (Items is INotifyCollectionChanged notifier)
        {
            _inlineItemsNotifier = notifier;
            _inlineItemsNotifier.CollectionChanged += OnItemsCollectionChanged;
        }

        UpdateLayoutPseudo(Layout);
        UpdateThemePseudo(StepTheme);
        PseudoClasses.Set(":readonly", IsReadOnly);
    }

    /// <summary>
    /// 获取或设置当前步骤。
    /// 当步骤项未设置 <see cref="StepItem.Value"/> 时，按 0 基索引匹配；
    /// 若传入 <c>FINISH</c> 或超出范围的索引，则视为全部完成。
    /// </summary>
    public string? Current
    {
        get => GetValue(CurrentProperty);
        set => SetValue(CurrentProperty, value);
    }

    /// <summary>
    /// 获取或设置当前步骤的状态。
    /// </summary>
    public StepStatus CurrentStatus
    {
        get => GetValue(CurrentStatusProperty);
        set => SetValue(CurrentStatusProperty, value);
    }

    /// <summary>
    /// 获取或设置步骤条方向。
    /// </summary>
    public StepsLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    /// <summary>
    /// 获取或设置步骤条主题。
    /// </summary>
    public StepsTheme StepTheme
    {
        get => GetValue(StepThemeProperty);
        set => SetValue(StepThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置是否只读。
    /// 只读时点击步骤项不会修改 <see cref="Current"/>。
    /// </summary>
    public bool IsReadOnly
    {
        get => GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    /// <summary>
    /// 获取当前步骤项数量。
    /// </summary>
    public int ResolvedItemCount
    {
        get => _resolvedItemCount;
        private set => SetAndRaise(ResolvedItemCountProperty, ref _resolvedItemCount, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        AttachItemsSourceNotifier();
        UpdateStepsState();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        DetachItemsSourceNotifier();

        if (_inlineItemsNotifier is not null)
        {
            _inlineItemsNotifier.CollectionChanged -= OnItemsCollectionChanged;
            _inlineItemsNotifier = null;
        }

        ClearAttachedOwners();
        base.OnDetachedFromVisualTree(e);
    }

    internal void NotifyStepInvoked(StepItem item)
    {
        if (IsReadOnly || !IsEnabled)
        {
            return;
        }

        var steps = GetStepItems();
        var index = steps.IndexOf(item);
        if (index < 0)
        {
            return;
        }

        var previous = Current;
        var next = !string.IsNullOrWhiteSpace(item.Value)
            ? item.Value
            : index.ToString(CultureInfo.InvariantCulture);

        if (string.Equals(previous, next, StringComparison.Ordinal))
        {
            CurrentChanged?.Invoke(this, new StepsCurrentChangedEventArgs(next, previous, item, index));
            return;
        }

        SetCurrentValue(CurrentProperty, next);
    }

    internal void RefreshFromItems()
    {
        UpdateStepsState();
    }

    private void OnItemsCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateStepsState();
    }

    private void AttachItemsSourceNotifier()
    {
        DetachItemsSourceNotifier();

        if (ItemsSource is INotifyCollectionChanged notifier)
        {
            _itemsSourceNotifier = notifier;
            _itemsSourceNotifier.CollectionChanged += OnItemsCollectionChanged;
        }

        UpdateStepsState();
    }

    private void DetachItemsSourceNotifier()
    {
        if (_itemsSourceNotifier is null)
        {
            return;
        }

        _itemsSourceNotifier.CollectionChanged -= OnItemsCollectionChanged;
        _itemsSourceNotifier = null;
    }

    private void ClearAttachedOwners()
    {
        foreach (var item in _attachedItems)
        {
            item.AttachOwner(null);
        }

        _attachedItems.Clear();
        ResolvedItemCount = 1;
    }

    private void UpdateStepsState()
    {
        PseudoClasses.Set(":readonly", IsReadOnly);

        var steps = GetStepItems();
        var count = steps.Count;
        ResolvedItemCount = Math.Max(count, 1);

        foreach (var previousItem in _attachedItems)
        {
            if (!steps.Contains(previousItem))
            {
                previousItem.AttachOwner(null);
            }
        }

        _attachedItems.Clear();
        _attachedItems.AddRange(steps);

        var currentIndex = ResolveCurrentIndex(steps);
        var markAllAsFinished = ShouldMarkAllAsFinished(steps, currentIndex);

        for (var index = 0; index < count; index++)
        {
            var item = steps[index];
            item.AttachOwner(this);
            item.ApplyResolvedState(
                index: index,
                isLast: index == count - 1,
                layout: Layout,
                theme: StepTheme,
                status: ResolveItemStatus(item, index, currentIndex, markAllAsFinished));
        }
    }

    private StepStatus ResolveItemStatus(StepItem item, int index, int currentIndex, bool markAllAsFinished)
    {
        if (item.Status.HasValue)
        {
            return item.Status.Value;
        }

        if (markAllAsFinished)
        {
            return StepStatus.Finish;
        }

        if (currentIndex < 0)
        {
            return StepStatus.Default;
        }

        if (index < currentIndex)
        {
            return StepStatus.Finish;
        }

        if (index == currentIndex)
        {
            return CurrentStatus;
        }

        return StepStatus.Default;
    }

    private bool ShouldMarkAllAsFinished(IReadOnlyList<StepItem> steps, int currentIndex)
    {
        if (string.Equals(Current, "FINISH", StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        return currentIndex >= steps.Count && steps.Count > 0;
    }

    private int ResolveCurrentIndex()
    {
        return ResolveCurrentIndex(GetStepItems());
    }

    private int ResolveCurrentIndex(IReadOnlyList<StepItem> steps)
    {
        if (steps.Count == 0 || string.IsNullOrWhiteSpace(Current))
        {
            return -1;
        }

        for (var index = 0; index < steps.Count; index++)
        {
            if (!string.IsNullOrWhiteSpace(steps[index].Value) &&
                string.Equals(steps[index].Value, Current, StringComparison.Ordinal))
            {
                return index;
            }
        }

        return int.TryParse(Current, NumberStyles.Integer, CultureInfo.InvariantCulture, out var currentIndex)
            ? currentIndex
            : -1;
    }

    private List<StepItem> GetStepItems()
    {
        if (ItemsSource is IEnumerable itemsSource)
        {
            return itemsSource.OfType<StepItem>().ToList();
        }

        return Items.OfType<StepItem>().ToList();
    }

    private void UpdateLayoutPseudo(StepsLayout layout)
    {
        PseudoClasses.Set(":horizontal", layout == StepsLayout.Horizontal);
        PseudoClasses.Set(":vertical", layout == StepsLayout.Vertical);
    }

    private void UpdateThemePseudo(StepsTheme theme)
    {
        PseudoClasses.Set(":default", theme == StepsTheme.Default);
        PseudoClasses.Set(":dot", theme == StepsTheme.Dot);
    }
}

/// <summary>
/// 步骤项。
/// </summary>
public sealed class StepItem : TemplatedControl
{
    private Steps? _owner;
    private int _resolvedIndex;
    private string _displayIndex = "1";
    private StepStatus _resolvedStatus;
    private bool _showConnector = true;
    private bool _showContent;
    private bool _showExtra;
    private bool _showTitleRight;
    private bool _hasCustomIcon;
    private bool _showIndexText = true;
    private bool _showCheckIcon;
    private bool _showErrorIcon;
    private bool _showDotIndicator;

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<StepItem, string?>(nameof(Title));

    /// <inheritdoc cref="Content" />
    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<StepItem, string?>(nameof(Content));

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<string?> ValueProperty =
        AvaloniaProperty.Register<StepItem, string?>(nameof(Value));

    /// <inheritdoc cref="TitleRight" />
    public static readonly StyledProperty<string?> TitleRightProperty =
        AvaloniaProperty.Register<StepItem, string?>(nameof(TitleRight));

    /// <inheritdoc cref="Extra" />
    public static readonly StyledProperty<object?> ExtraProperty =
        AvaloniaProperty.Register<StepItem, object?>(nameof(Extra));

    /// <inheritdoc cref="Icon" />
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<StepItem, object?>(nameof(Icon));

    /// <inheritdoc cref="Status" />
    public static readonly StyledProperty<StepStatus?> StatusProperty =
        AvaloniaProperty.Register<StepItem, StepStatus?>(nameof(Status));

    /// <inheritdoc cref="ResolvedIndex" />
    public static readonly DirectProperty<StepItem, int> ResolvedIndexProperty =
        AvaloniaProperty.RegisterDirect<StepItem, int>(
            nameof(ResolvedIndex),
            o => o.ResolvedIndex);

    /// <inheritdoc cref="DisplayIndex" />
    public static readonly DirectProperty<StepItem, string> DisplayIndexProperty =
        AvaloniaProperty.RegisterDirect<StepItem, string>(
            nameof(DisplayIndex),
            o => o.DisplayIndex);

    /// <inheritdoc cref="ResolvedStatus" />
    public static readonly DirectProperty<StepItem, StepStatus> ResolvedStatusProperty =
        AvaloniaProperty.RegisterDirect<StepItem, StepStatus>(
            nameof(ResolvedStatus),
            o => o.ResolvedStatus);

    /// <inheritdoc cref="ShowConnector" />
    public static readonly DirectProperty<StepItem, bool> ShowConnectorProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowConnector),
            o => o.ShowConnector);

    /// <inheritdoc cref="ShowContent" />
    public static readonly DirectProperty<StepItem, bool> ShowContentProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowContent),
            o => o.ShowContent);

    /// <inheritdoc cref="ShowExtra" />
    public static readonly DirectProperty<StepItem, bool> ShowExtraProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowExtra),
            o => o.ShowExtra);

    /// <inheritdoc cref="ShowTitleRight" />
    public static readonly DirectProperty<StepItem, bool> ShowTitleRightProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowTitleRight),
            o => o.ShowTitleRight);

    /// <inheritdoc cref="HasCustomIcon" />
    public static readonly DirectProperty<StepItem, bool> HasCustomIconProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(HasCustomIcon),
            o => o.HasCustomIcon);

    /// <inheritdoc cref="ShowIndexText" />
    public static readonly DirectProperty<StepItem, bool> ShowIndexTextProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowIndexText),
            o => o.ShowIndexText);

    /// <inheritdoc cref="ShowCheckIcon" />
    public static readonly DirectProperty<StepItem, bool> ShowCheckIconProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowCheckIcon),
            o => o.ShowCheckIcon);

    /// <inheritdoc cref="ShowErrorIcon" />
    public static readonly DirectProperty<StepItem, bool> ShowErrorIconProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowErrorIcon),
            o => o.ShowErrorIcon);

    /// <inheritdoc cref="ShowDotIndicator" />
    public static readonly DirectProperty<StepItem, bool> ShowDotIndicatorProperty =
        AvaloniaProperty.RegisterDirect<StepItem, bool>(
            nameof(ShowDotIndicator),
            o => o.ShowDotIndicator);

    static StepItem()
    {
        TitleProperty.Changed.AddClassHandler<StepItem>((control, _) => control.UpdateFlags());
        ContentProperty.Changed.AddClassHandler<StepItem>((control, _) => control.UpdateFlags());
        TitleRightProperty.Changed.AddClassHandler<StepItem>((control, _) => control.UpdateFlags());
        ExtraProperty.Changed.AddClassHandler<StepItem>((control, _) => control.UpdateFlags());
        IconProperty.Changed.AddClassHandler<StepItem>((control, _) => control.UpdateFlags());
        StatusProperty.Changed.AddClassHandler<StepItem>((control, _) => control._owner?.RefreshFromItems());
    }

    /// <summary>
    /// 获取或设置步骤标题。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 获取或设置步骤说明文本。
    /// </summary>
    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// 获取或设置步骤值。
    /// 当父控件 <see cref="Steps.Current"/> 为非数字时，可通过该值匹配当前步骤。
    /// </summary>
    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 获取或设置纵向布局时标题右侧的辅助文本。
    /// </summary>
    public string? TitleRight
    {
        get => GetValue(TitleRightProperty);
        set => SetValue(TitleRightProperty, value);
    }

    /// <summary>
    /// 获取或设置额外内容。
    /// </summary>
    public object? Extra
    {
        get => GetValue(ExtraProperty);
        set => SetValue(ExtraProperty, value);
    }

    /// <summary>
    /// 获取或设置自定义图标。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取或设置显式状态；若未设置，则由父级步骤条自动推导。
    /// </summary>
    public StepStatus? Status
    {
        get => GetValue(StatusProperty);
        set => SetValue(StatusProperty, value);
    }

    /// <summary>
    /// 获取当前逻辑索引。
    /// </summary>
    public int ResolvedIndex
    {
        get => _resolvedIndex;
        private set => SetAndRaise(ResolvedIndexProperty, ref _resolvedIndex, value);
    }

    /// <summary>
    /// 获取默认展示的序号文本。
    /// </summary>
    public string DisplayIndex
    {
        get => _displayIndex;
        private set => SetAndRaise(DisplayIndexProperty, ref _displayIndex, value);
    }

    /// <summary>
    /// 获取实际渲染状态。
    /// </summary>
    public StepStatus ResolvedStatus
    {
        get => _resolvedStatus;
        private set => SetAndRaise(ResolvedStatusProperty, ref _resolvedStatus, value);
    }

    /// <summary>
    /// 获取是否显示连接线。
    /// </summary>
    public bool ShowConnector
    {
        get => _showConnector;
        private set => SetAndRaise(ShowConnectorProperty, ref _showConnector, value);
    }

    /// <summary>
    /// 获取是否显示说明文本。
    /// </summary>
    public bool ShowContent
    {
        get => _showContent;
        private set => SetAndRaise(ShowContentProperty, ref _showContent, value);
    }

    /// <summary>
    /// 获取是否显示额外内容。
    /// </summary>
    public bool ShowExtra
    {
        get => _showExtra;
        private set => SetAndRaise(ShowExtraProperty, ref _showExtra, value);
    }

    /// <summary>
    /// 获取是否显示标题右侧文本。
    /// </summary>
    public bool ShowTitleRight
    {
        get => _showTitleRight;
        private set => SetAndRaise(ShowTitleRightProperty, ref _showTitleRight, value);
    }

    /// <summary>
    /// 获取是否存在自定义图标。
    /// </summary>
    public bool HasCustomIcon
    {
        get => _hasCustomIcon;
        private set => SetAndRaise(HasCustomIconProperty, ref _hasCustomIcon, value);
    }

    /// <summary>
    /// 获取是否显示默认序号文本。
    /// </summary>
    public bool ShowIndexText
    {
        get => _showIndexText;
        private set => SetAndRaise(ShowIndexTextProperty, ref _showIndexText, value);
    }

    /// <summary>
    /// 获取是否显示完成图标。
    /// </summary>
    public bool ShowCheckIcon
    {
        get => _showCheckIcon;
        private set => SetAndRaise(ShowCheckIconProperty, ref _showCheckIcon, value);
    }

    /// <summary>
    /// 获取是否显示错误图标。
    /// </summary>
    public bool ShowErrorIcon
    {
        get => _showErrorIcon;
        private set => SetAndRaise(ShowErrorIconProperty, ref _showErrorIcon, value);
    }

    /// <summary>
    /// 获取是否显示点状指示器。
    /// </summary>
    public bool ShowDotIndicator
    {
        get => _showDotIndicator;
        private set => SetAndRaise(ShowDotIndicatorProperty, ref _showDotIndicator, value);
    }

    /// <inheritdoc />
    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);

        if (IsEnabled)
        {
            _owner?.NotifyStepInvoked(this);
        }
    }

    internal void AttachOwner(Steps? owner)
    {
        _owner = owner;
        UpdateFlags();
    }

    internal void ApplyResolvedState(int index, bool isLast, StepsLayout layout, StepsTheme theme, StepStatus status)
    {
        ResolvedIndex = index;
        DisplayIndex = (index + 1).ToString(CultureInfo.InvariantCulture);
        ShowConnector = !isLast;
        ResolvedStatus = status;

        var isVertical = layout == StepsLayout.Vertical;
        var isDotTheme = theme == StepsTheme.Dot;

        PseudoClasses.Set(":horizontal", !isVertical);
        PseudoClasses.Set(":vertical", isVertical);
        PseudoClasses.Set(":dot", isDotTheme);
        PseudoClasses.Set(":last", isLast);
        PseudoClasses.Set(":default", status == StepStatus.Default);
        PseudoClasses.Set(":process", status == StepStatus.Process);
        PseudoClasses.Set(":finish", status == StepStatus.Finish);
        PseudoClasses.Set(":error", status == StepStatus.Error);
        PseudoClasses.Set(":clickable", _owner is { IsReadOnly: false, IsEnabled: true });

        UpdateFlags(isDotTheme);
    }

    private void UpdateFlags(bool? dotThemeOverride = null)
    {
        ShowContent = !string.IsNullOrWhiteSpace(Content);
        ShowExtra = Extra is not null;
        ShowTitleRight = !string.IsNullOrWhiteSpace(TitleRight);
        HasCustomIcon = Icon is not null;

        var isDotTheme = dotThemeOverride ?? PseudoClasses.Contains(":dot");
        ShowDotIndicator = !HasCustomIcon && isDotTheme;
        ShowCheckIcon = !HasCustomIcon && !isDotTheme && ResolvedStatus == StepStatus.Finish;
        ShowErrorIcon = !HasCustomIcon && !isDotTheme && ResolvedStatus == StepStatus.Error;
        ShowIndexText = !HasCustomIcon && !isDotTheme && !ShowCheckIcon && !ShowErrorIcon;
    }
}
