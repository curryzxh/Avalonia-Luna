using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Luna.Mobile.Controls;

/// <summary>
/// `Indexes` 控件索引变化时的事件参数。
/// </summary>
public sealed class IndexesChangedEventArgs : EventArgs
{
    /// <summary>
    /// 使用指定索引初始化事件参数。
    /// </summary>
    /// <param name="index">当前索引文本。</param>
    public IndexesChangedEventArgs(string index)
    {
        Index = index;
    }

    /// <summary>
    /// 获取当前索引值。
    /// </summary>
    public string Index { get; }
}

/// <summary>
/// `Indexes` 内容中的锚点标题。
/// </summary>
/// <remarks>
/// 常与 <see cref="CellGroup"/>、<see cref="Cell"/> 配合使用，用于标记某个索引分组的起始位置。
/// 伪类：:current。
/// </remarks>
public sealed class IndexesAnchor : TemplatedControl
{
    private bool _isCurrent;

    /// <inheritdoc cref="Index" />
    public static readonly StyledProperty<string> IndexProperty =
        AvaloniaProperty.Register<IndexesAnchor, string>(nameof(Index), string.Empty);

    /// <inheritdoc cref="IsCurrent" />
    public static readonly DirectProperty<IndexesAnchor, bool> IsCurrentProperty =
        AvaloniaProperty.RegisterDirect<IndexesAnchor, bool>(
            nameof(IsCurrent),
            o => o.IsCurrent);

    static IndexesAnchor()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<IndexesAnchor>(Avalonia.Layout.HorizontalAlignment.Stretch);
        IndexProperty.Changed.AddClassHandler<IndexesAnchor>((o, _) => o.UpdateState());
    }

    /// <summary>
    /// 获取或设置锚点索引文本。
    /// </summary>
    public string Index
    {
        get => GetValue(IndexProperty);
        set => SetValue(IndexProperty, value);
    }

    /// <summary>
    /// 获取当前锚点是否为激活状态。
    /// </summary>
    public bool IsCurrent
    {
        get => _isCurrent;
        private set => SetAndRaise(IsCurrentProperty, ref _isCurrent, value);
    }

    internal void SetCurrent(bool value)
    {
        IsCurrent = value;
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    private void UpdateState()
    {
        PseudoClasses.Set(":current", IsCurrent);
    }
}

/// <summary>
/// 移动端索引导航控件，支持右侧索引栏、滚动联动和吸顶标题。
/// </summary>
/// <remarks>
/// 模板契约：
/// <list type="bullet">
/// <item><description>PART_ScrollViewer：<see cref="ScrollViewer"/></description></item>
/// <item><description>PART_Sidebar：右侧索引按钮容器 <see cref="ItemsControl"/></description></item>
/// </list>
/// 伪类：:sticky。
/// </remarks>
[TemplatePart(ScrollViewerPartName, typeof(ScrollViewer))]
[TemplatePart(SidebarPartName, typeof(ItemsControl))]
public sealed class Indexes : ContentControl
{
    private const string ScrollViewerPartName = "PART_ScrollViewer";
    private const string SidebarPartName = "PART_Sidebar";

    private ScrollViewer? _scrollViewer;
    private ItemsControl? _sidebar;
    private IReadOnlyList<IndexesAnchor> _anchors = Array.Empty<IndexesAnchor>();
    private IReadOnlyList<string> _resolvedIndexes = Array.Empty<string>();
    private string? _currentIndex;
    private string? _stickyHeaderText;
    private bool _showStickyHeader;
    private bool _pendingRefresh;

    /// <inheritdoc cref="IndexList" />
    public static readonly StyledProperty<IEnumerable<string>?> IndexListProperty =
        AvaloniaProperty.Register<Indexes, IEnumerable<string>?>(nameof(IndexList));

    /// <inheritdoc cref="Sticky" />
    public static readonly StyledProperty<bool> StickyProperty =
        AvaloniaProperty.Register<Indexes, bool>(nameof(Sticky), true);

    /// <inheritdoc cref="StickyOffset" />
    public static readonly StyledProperty<double> StickyOffsetProperty =
        AvaloniaProperty.Register<Indexes, double>(nameof(StickyOffset));

    /// <inheritdoc cref="ResolvedIndexes" />
    public static readonly DirectProperty<Indexes, IReadOnlyList<string>> ResolvedIndexesProperty =
        AvaloniaProperty.RegisterDirect<Indexes, IReadOnlyList<string>>(
            nameof(ResolvedIndexes),
            o => o.ResolvedIndexes);

    /// <inheritdoc cref="CurrentIndex" />
    public static readonly DirectProperty<Indexes, string?> CurrentIndexProperty =
        AvaloniaProperty.RegisterDirect<Indexes, string?>(
            nameof(CurrentIndex),
            o => o.CurrentIndex);

    /// <inheritdoc cref="StickyHeaderText" />
    public static readonly DirectProperty<Indexes, string?> StickyHeaderTextProperty =
        AvaloniaProperty.RegisterDirect<Indexes, string?>(
            nameof(StickyHeaderText),
            o => o.StickyHeaderText);

    /// <inheritdoc cref="ShowStickyHeader" />
    public static readonly DirectProperty<Indexes, bool> ShowStickyHeaderProperty =
        AvaloniaProperty.RegisterDirect<Indexes, bool>(
            nameof(ShowStickyHeader),
            o => o.ShowStickyHeader);

    static Indexes()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Indexes>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<Indexes>(Avalonia.Layout.HorizontalAlignment.Stretch);

        ContentProperty.Changed.AddClassHandler<Indexes>((o, _) => o.ScheduleRefreshAnchors());
        IndexListProperty.Changed.AddClassHandler<Indexes>((o, _) => o.UpdateResolvedIndexes());
        StickyProperty.Changed.AddClassHandler<Indexes>((o, _) => o.UpdateStickyState());
        StickyOffsetProperty.Changed.AddClassHandler<Indexes>((o, _) => o.UpdateCurrentIndex());
    }

    /// <summary>
    /// 当当前索引因滚动变化而更新时触发。
    /// </summary>
    public event EventHandler<IndexesChangedEventArgs>? CurrentIndexChanged;

    /// <summary>
    /// 当用户点击右侧索引栏时触发。
    /// </summary>
    public event EventHandler<IndexesChangedEventArgs>? IndexSelected;

    /// <summary>
    /// 获取或设置右侧索引栏显示顺序；为空时自动从内容中的 <see cref="IndexesAnchor"/> 推导。
    /// </summary>
    public IEnumerable<string>? IndexList
    {
        get => GetValue(IndexListProperty);
        set => SetValue(IndexListProperty, value);
    }

    /// <summary>
    /// 获取或设置是否启用吸顶标题。
    /// </summary>
    public bool Sticky
    {
        get => GetValue(StickyProperty);
        set => SetValue(StickyProperty, value);
    }

    /// <summary>
    /// 获取或设置吸顶标题距离顶部的偏移量。
    /// </summary>
    public double StickyOffset
    {
        get => GetValue(StickyOffsetProperty);
        set => SetValue(StickyOffsetProperty, value);
    }

    /// <summary>
    /// 获取当前解析后的索引列表。
    /// </summary>
    public IReadOnlyList<string> ResolvedIndexes
    {
        get => _resolvedIndexes;
        private set => SetAndRaise(ResolvedIndexesProperty, ref _resolvedIndexes, value);
    }

    /// <summary>
    /// 获取当前激活的索引。
    /// </summary>
    public string? CurrentIndex
    {
        get => _currentIndex;
        private set => SetAndRaise(CurrentIndexProperty, ref _currentIndex, value);
    }

    /// <summary>
    /// 获取吸顶标题显示的文本。
    /// </summary>
    public string? StickyHeaderText
    {
        get => _stickyHeaderText;
        private set => SetAndRaise(StickyHeaderTextProperty, ref _stickyHeaderText, value);
    }

    /// <summary>
    /// 获取当前是否显示吸顶标题。
    /// </summary>
    public bool ShowStickyHeader
    {
        get => _showStickyHeader;
        private set => SetAndRaise(ShowStickyHeaderProperty, ref _showStickyHeader, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        ScheduleRefreshAnchors();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        DetachScrollViewer();
        DetachSidebarHandlers();
        _sidebar = null;
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        DetachScrollViewer();
        DetachSidebarHandlers();

        _scrollViewer = e.NameScope.Find<ScrollViewer>(ScrollViewerPartName);
        _sidebar = e.NameScope.Find<ItemsControl>(SidebarPartName);

        AttachScrollViewer();
        ScheduleRefreshAnchors();
        ScheduleSidebarWireup();
    }

    private void AttachScrollViewer()
    {
        if (_scrollViewer is null)
        {
            return;
        }

        _scrollViewer.PropertyChanged += OnScrollViewerPropertyChanged;
    }

    private void DetachScrollViewer()
    {
        if (_scrollViewer is not null)
        {
            _scrollViewer.PropertyChanged -= OnScrollViewerPropertyChanged;
        }

        _scrollViewer = null;
    }

    private void OnScrollViewerPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == ScrollViewer.OffsetProperty)
        {
            UpdateCurrentIndex();
        }
    }

    private void ScheduleRefreshAnchors()
    {
        if (_pendingRefresh)
        {
            return;
        }

        _pendingRefresh = true;
        Dispatcher.UIThread.Post(() =>
        {
            _pendingRefresh = false;
            RefreshAnchors();
        }, DispatcherPriority.Loaded);
    }

    private void RefreshAnchors()
    {
        var anchors = this
            .GetVisualDescendants()
            .OfType<IndexesAnchor>()
            .Where(anchor => !string.IsNullOrWhiteSpace(anchor.Index))
            .ToArray();

        _anchors = anchors;
        UpdateResolvedIndexes();
        ScheduleSidebarWireup();
        UpdateCurrentIndex();
    }

    private void UpdateResolvedIndexes()
    {
        IReadOnlyList<string> indexes;
        var explicitIndexes = IndexList?
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();

        if (explicitIndexes is { Length: > 0 })
        {
            indexes = explicitIndexes;
        }
        else
        {
            indexes = _anchors
                .Select(anchor => anchor.Index)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray();
        }

        ResolvedIndexes = indexes;
        ScheduleSidebarWireup();
        UpdateSidebarState();
        UpdateStickyState();
    }

    private void UpdateCurrentIndex()
    {
        var nextIndex = ResolveCurrentIndex();
        var changed = !string.Equals(CurrentIndex, nextIndex, StringComparison.Ordinal);

        CurrentIndex = nextIndex;

        foreach (var anchor in _anchors)
        {
            anchor.SetCurrent(string.Equals(anchor.Index, CurrentIndex, StringComparison.Ordinal));
        }

        UpdateSidebarState();
        UpdateStickyState();

        if (changed && !string.IsNullOrWhiteSpace(CurrentIndex))
        {
            CurrentIndexChanged?.Invoke(this, new IndexesChangedEventArgs(CurrentIndex));
        }
    }

    private string? ResolveCurrentIndex()
    {
        if (_anchors.Count == 0)
        {
            return ResolvedIndexes.FirstOrDefault();
        }

        if (_scrollViewer is null)
        {
            return _anchors[0].Index;
        }

        var threshold = Math.Max(0, StickyOffset);
        string? current = _anchors[0].Index;

        foreach (var anchor in _anchors)
        {
            var position = anchor.TranslatePoint(new Point(0, 0), _scrollViewer);
            if (!position.HasValue)
            {
                continue;
            }

            if (position.Value.Y <= threshold + 1)
            {
                current = anchor.Index;
                continue;
            }

            break;
        }

        return current;
    }

    private void UpdateStickyState()
    {
        var showSticky = Sticky && !string.IsNullOrWhiteSpace(CurrentIndex);
        ShowStickyHeader = showSticky;
        StickyHeaderText = showSticky ? CurrentIndex : null;
        PseudoClasses.Set(":sticky", Sticky);
    }

    private void ScheduleSidebarWireup()
    {
        Dispatcher.UIThread.Post(AttachSidebarHandlers, DispatcherPriority.Loaded);
    }

    private void AttachSidebarHandlers()
    {
        if (_sidebar is null)
        {
            return;
        }

        DetachSidebarHandlers();

        foreach (var button in _sidebar.GetVisualDescendants().OfType<Button>())
        {
            button.Click += OnSidebarButtonClick;
        }

        UpdateSidebarState();
    }

    private void DetachSidebarHandlers()
    {
        if (_sidebar is not null)
        {
            foreach (var button in _sidebar.GetVisualDescendants().OfType<Button>())
            {
                button.Click -= OnSidebarButtonClick;
            }
        }
    }

    private void OnSidebarButtonClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        if (sender is not Button { Tag: string index })
        {
            return;
        }

        ScrollToIndex(index);
        IndexSelected?.Invoke(this, new IndexesChangedEventArgs(index));
    }

    private void ScrollToIndex(string index)
    {
        if (_scrollViewer is null)
        {
            return;
        }

        var anchor = _anchors.FirstOrDefault(x => string.Equals(x.Index, index, StringComparison.OrdinalIgnoreCase));
        if (anchor is null)
        {
            return;
        }

        var position = anchor.TranslatePoint(new Point(0, 0), _scrollViewer);
        if (!position.HasValue)
        {
            return;
        }

        var targetOffset = Math.Max(0, _scrollViewer.Offset.Y + position.Value.Y - StickyOffset);
        _scrollViewer.Offset = new Vector(_scrollViewer.Offset.X, targetOffset);
        UpdateCurrentIndex();
    }

    private void UpdateSidebarState()
    {
        if (_sidebar is null)
        {
            return;
        }

        foreach (var button in _sidebar.GetVisualDescendants().OfType<Button>())
        {
            if (button.Tag is not string index)
            {
                continue;
            }

            var isActive = string.Equals(index, CurrentIndex, StringComparison.OrdinalIgnoreCase);
            if (isActive)
            {
                if (!button.Classes.Contains("active"))
                {
                    button.Classes.Add("active");
                }
            }
            else
            {
                button.Classes.Remove("active");
            }
        }
    }
}
