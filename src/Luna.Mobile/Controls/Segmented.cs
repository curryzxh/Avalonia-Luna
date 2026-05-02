using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Avalonia.Metadata;
using System;
using System.Collections.Specialized;

namespace Luna.Mobile.Controls;

/// <summary>
/// 分段控制器选项。
/// </summary>
public sealed class SegmentedItem : AvaloniaObject
{
    private bool _hasIcon;

    /// <inheritdoc cref="Content" />
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<SegmentedItem, object?>(nameof(Content));

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<SegmentedItem, object?>(nameof(Value));

    /// <inheritdoc cref="Icon" />
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<SegmentedItem, object?>(nameof(Icon));

    /// <inheritdoc cref="IsEnabled" />
    public static readonly StyledProperty<bool> IsEnabledProperty =
        AvaloniaProperty.Register<SegmentedItem, bool>(nameof(IsEnabled), true);

    /// <inheritdoc cref="HasIcon" />
    public static readonly DirectProperty<SegmentedItem, bool> HasIconProperty =
        AvaloniaProperty.RegisterDirect<SegmentedItem, bool>(
            nameof(HasIcon),
            o => o.HasIcon);

    static SegmentedItem()
    {
        IconProperty.Changed.AddClassHandler<SegmentedItem>((item, _) => item.UpdateFlags());
    }

    /// <summary>
    /// 获取或设置展示内容。
    /// </summary>
    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// 获取或设置选项值。
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 获取或设置选项图标。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取或设置选项是否可用。
    /// </summary>
    public bool IsEnabled
    {
        get => GetValue(IsEnabledProperty);
        set => SetValue(IsEnabledProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在图标。
    /// </summary>
    public bool HasIcon
    {
        get => _hasIcon;
        private set => SetAndRaise(HasIconProperty, ref _hasIcon, value);
    }

    /// <inheritdoc />
    public override string ToString()
    {
        return Content?.ToString() ?? string.Empty;
    }

    private void UpdateFlags()
    {
        HasIcon = Icon is not null;
    }
}

/// <summary>
/// 分段控制器，用于在多个互斥选项中切换单个值。
/// </summary>
[TemplatePart(ItemsHostPartName, typeof(Grid))]
public sealed class Segmented : TemplatedControl
{
    private const string ItemsHostPartName = "PART_ItemsHost";

    private readonly AvaloniaList<SegmentedItem> _items = [];
    private Grid? _itemsHost;
    private bool _isSynchronizingSelection;

    /// <inheritdoc cref="Items" />
    public static readonly DirectProperty<Segmented, AvaloniaList<SegmentedItem>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<Segmented, AvaloniaList<SegmentedItem>>(
            nameof(Items),
            o => o.Items);

    /// <inheritdoc cref="SelectedIndex" />
    public static readonly StyledProperty<int> SelectedIndexProperty =
        AvaloniaProperty.Register<Segmented, int>(
            nameof(SelectedIndex),
            0,
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    /// <inheritdoc cref="SelectedValue" />
    public static readonly StyledProperty<object?> SelectedValueProperty =
        AvaloniaProperty.Register<Segmented, object?>(
            nameof(SelectedValue),
            defaultBindingMode: Avalonia.Data.BindingMode.TwoWay);

    /// <inheritdoc cref="Block" />
    public static readonly StyledProperty<bool> BlockProperty =
        AvaloniaProperty.Register<Segmented, bool>(nameof(Block));

    static Segmented()
    {
        HorizontalAlignmentProperty.OverrideDefaultValue<Segmented>(HorizontalAlignment.Left);

        SelectedIndexProperty.Changed.AddClassHandler<Segmented>((control, _) => control.OnSelectedIndexChanged());
        SelectedValueProperty.Changed.AddClassHandler<Segmented>((control, _) => control.OnSelectedValueChanged());
        BlockProperty.Changed.AddClassHandler<Segmented>((control, _) => control.OnBlockChanged());
        IsEnabledProperty.Changed.AddClassHandler<Segmented>((control, _) => control.RebuildButtons());
    }

    /// <summary>
    /// 初始化分段控制器。
    /// </summary>
    public Segmented()
    {
        _items.CollectionChanged += OnItemsChanged;
        PseudoClasses.Set(":block", Block);
    }

    /// <summary>
    /// 当选中项变化时触发。
    /// </summary>
    public event EventHandler? SelectionChanged;

    /// <summary>
    /// 获取选项集合。
    /// </summary>
    [Content]
    public AvaloniaList<SegmentedItem> Items => _items;

    /// <summary>
    /// 获取或设置当前选中的索引。
    /// </summary>
    public int SelectedIndex
    {
        get => GetValue(SelectedIndexProperty);
        set => SetValue(SelectedIndexProperty, value);
    }

    /// <summary>
    /// 获取或设置当前选中的值。
    /// </summary>
    public object? SelectedValue
    {
        get => GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }

    /// <summary>
    /// 获取或设置是否占满可用宽度。
    /// </summary>
    public bool Block
    {
        get => GetValue(BlockProperty);
        set => SetValue(BlockProperty, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _itemsHost = e.NameScope.Find<Grid>(ItemsHostPartName);
        EnsureSelection();
        RebuildButtons();
    }

    private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems)
            {
                if (item is SegmentedItem segmentedItem)
                {
                    segmentedItem.PropertyChanged -= OnItemPropertyChanged;
                }
            }
        }

        if (e.NewItems is not null)
        {
            foreach (var item in e.NewItems)
            {
                if (item is SegmentedItem segmentedItem)
                {
                    segmentedItem.PropertyChanged += OnItemPropertyChanged;
                }
            }
        }

        EnsureSelection();
        RebuildButtons();
    }

    private void OnItemPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (e.Property == SegmentedItem.ContentProperty ||
            e.Property == SegmentedItem.IconProperty ||
            e.Property == SegmentedItem.ValueProperty ||
            e.Property == SegmentedItem.IsEnabledProperty)
        {
            EnsureSelection();
            RebuildButtons();
        }
    }

    private void OnSelectedIndexChanged()
    {
        if (_isSynchronizingSelection)
        {
            return;
        }

        UpdateSelection(CoerceSelectedIndex(SelectedIndex), raiseEvent: true);
    }

    private void OnSelectedValueChanged()
    {
        if (_isSynchronizingSelection)
        {
            return;
        }

        if (SelectedValue is null)
        {
            UpdateSelection(-1, raiseEvent: true);
            return;
        }

        var index = FindIndexByValue(SelectedValue);
        if (index >= 0)
        {
            UpdateSelection(index, raiseEvent: true);
            return;
        }

        RebuildButtons();
    }

    private void OnBlockChanged()
    {
        PseudoClasses.Set(":block", Block);
        RebuildButtons();
    }

    private void EnsureSelection()
    {
        if (Items.Count == 0)
        {
            UpdateSelection(-1, raiseEvent: false);
            return;
        }

        if (SelectedValue is not null)
        {
            var valueIndex = FindIndexByValue(SelectedValue);
            if (valueIndex >= 0)
            {
                UpdateSelection(valueIndex, raiseEvent: false);
                return;
            }
        }

        var index = CoerceSelectedIndex(SelectedIndex);
        if (index < 0)
        {
            index = FindFirstEnabledIndex();
        }

        if (index < 0)
        {
            index = 0;
        }

        UpdateSelection(index, raiseEvent: false);
    }

    private void UpdateSelection(int index, bool raiseEvent)
    {
        var coercedIndex = CoerceSelectedIndex(index);
        var newValue = ResolveSelectionValue(coercedIndex);
        var changed = coercedIndex != SelectedIndex || !Equals(newValue, SelectedValue);

        _isSynchronizingSelection = true;
        try
        {
            if (SelectedIndex != coercedIndex)
            {
                SetValue(SelectedIndexProperty, coercedIndex);
            }

            if (!Equals(SelectedValue, newValue))
            {
                SetValue(SelectedValueProperty, newValue);
            }
        }
        finally
        {
            _isSynchronizingSelection = false;
        }

        RebuildButtons();

        if (raiseEvent && changed)
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int CoerceSelectedIndex(int index)
    {
        if (Items.Count == 0)
        {
            return -1;
        }

        if (index < 0 || index >= Items.Count)
        {
            return -1;
        }

        return index;
    }

    private int FindFirstEnabledIndex()
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Items[i].IsEnabled)
            {
                return i;
            }
        }

        return -1;
    }

    private int FindIndexByValue(object? value)
    {
        for (var i = 0; i < Items.Count; i++)
        {
            if (Equals(ResolveItemValue(Items[i]), value))
            {
                return i;
            }
        }

        return -1;
    }

    private object? ResolveSelectionValue(int index)
    {
        if (index < 0 || index >= Items.Count)
        {
            return null;
        }

        return ResolveItemValue(Items[index]);
    }

    private static object? ResolveItemValue(SegmentedItem item)
    {
        return item.Value ?? item.Content;
    }

    private void RebuildButtons()
    {
        if (_itemsHost is null)
        {
            return;
        }

        _itemsHost.Children.Clear();
        _itemsHost.ColumnDefinitions.Clear();

        for (var i = 0; i < Items.Count; i++)
        {
            _itemsHost.ColumnDefinitions.Add(new ColumnDefinition(Block ? GridLength.Star : GridLength.Auto));

            var index = i;
            var item = Items[i];
            var button = new Button
            {
                Content = BuildItemContent(item),
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                IsEnabled = IsEnabled && item.IsEnabled,
            };

            button.Classes.Add("segmented-item");
            if (index == SelectedIndex)
            {
                button.Classes.Add("selected");
            }

            if (!item.IsEnabled)
            {
                button.Classes.Add("item-disabled");
            }

            Grid.SetColumn(button, i);
            button.Click += (_, _) =>
            {
                if (!IsEnabled || !item.IsEnabled)
                {
                    return;
                }

                UpdateSelection(index, raiseEvent: true);
            };

            _itemsHost.Children.Add(button);
        }
    }

    private static object BuildItemContent(SegmentedItem item)
    {
        if (item.Icon is null)
        {
            return item.Content ?? string.Empty;
        }

        var stack = new StackPanel
        {
            Orientation = Orientation.Horizontal,
            Spacing = 4,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center,
        };

        stack.Children.Add(new ContentPresenter
        {
            Content = item.Icon,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        });

        stack.Children.Add(new ContentPresenter
        {
            Content = item.Content,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
        });

        return stack;
    }
}
