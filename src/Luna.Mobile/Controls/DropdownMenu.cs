using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.Metadata;
using Avalonia.Controls.Shapes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Luna.Mobile.Controls;

/// <summary>
/// 下拉菜单展开方向。
/// </summary>
public enum DropdownMenuDirection
{
    /// <summary>
    /// 向下展开。
    /// </summary>
    Down,

    /// <summary>
    /// 向上展开。
    /// </summary>
    Up,
}

/// <summary>
/// 下拉菜单选项定义。
/// </summary>
public sealed class DropdownMenuOption
{
    /// <summary>
    /// 获取或设置选项显示文本。
    /// </summary>
    public string Label { get; set; } = string.Empty;

    /// <summary>
    /// 获取或设置选项值。
    /// </summary>
    public object? Value { get; set; }

    /// <summary>
    /// 获取或设置是否禁用。
    /// </summary>
    public bool IsDisabled { get; set; }
}

/// <summary>
/// 下拉菜单项值变化事件参数。
/// </summary>
public sealed class DropdownMenuItemValueChangedEventArgs : EventArgs
{
    /// <summary>
    /// 初始化 <see cref="DropdownMenuItemValueChangedEventArgs"/> 的新实例。
    /// </summary>
    public DropdownMenuItemValueChangedEventArgs(object? value, IReadOnlyList<object?> values)
    {
        Value = value;
        Values = values;
    }

    /// <summary>
    /// 获取单选值。
    /// </summary>
    public object? Value { get; }

    /// <summary>
    /// 获取多选值集合。
    /// </summary>
    public IReadOnlyList<object?> Values { get; }
}

/// <summary>
/// 单个下拉菜单项。
/// </summary>
[PseudoClasses(":open", ":disabled", ":multiple", ":selected")]
public sealed class DropdownMenuItem : HeaderedContentControl
{
    private readonly AvaloniaList<DropdownMenuOption> _options = [];
    private readonly AvaloniaList<object?> _selectedValues = [];
    private DropdownMenu? _owner;
    private bool _suppressSelectedValuesChanged;

    /// <inheritdoc cref="Options" />
    public static readonly DirectProperty<DropdownMenuItem, AvaloniaList<DropdownMenuOption>> OptionsProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenuItem, AvaloniaList<DropdownMenuOption>>(
            nameof(Options),
            o => o.Options);

    /// <inheritdoc cref="SelectedValues" />
    public static readonly DirectProperty<DropdownMenuItem, AvaloniaList<object?>> SelectedValuesProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenuItem, AvaloniaList<object?>>(
            nameof(SelectedValues),
            o => o.SelectedValues);

    /// <inheritdoc cref="Value" />
    public static readonly StyledProperty<object?> ValueProperty =
        AvaloniaProperty.Register<DropdownMenuItem, object?>(nameof(Value));

    /// <inheritdoc cref="Label" />
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<DropdownMenuItem, string?>(nameof(Label));

    /// <inheritdoc cref="HeaderIcon" />
    public static readonly StyledProperty<object?> HeaderIconProperty =
        AvaloniaProperty.Register<DropdownMenuItem, object?>(nameof(HeaderIcon));

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DropdownMenuItem, bool>(nameof(IsOpen));

    /// <inheritdoc cref="IsDisabled" />
    public static readonly StyledProperty<bool> IsDisabledProperty =
        AvaloniaProperty.Register<DropdownMenuItem, bool>(nameof(IsDisabled));

    /// <inheritdoc cref="Multiple" />
    public static readonly StyledProperty<bool> MultipleProperty =
        AvaloniaProperty.Register<DropdownMenuItem, bool>(nameof(Multiple));

    /// <inheritdoc cref="OptionsColumns" />
    public static readonly StyledProperty<int> OptionsColumnsProperty =
        AvaloniaProperty.Register<DropdownMenuItem, int>(nameof(OptionsColumns), 1);

    /// <inheritdoc cref="Footer" />
    public static readonly StyledProperty<object?> FooterProperty =
        AvaloniaProperty.Register<DropdownMenuItem, object?>(nameof(Footer));

    /// <inheritdoc cref="HasCustomContent" />
    public static readonly DirectProperty<DropdownMenuItem, bool> HasCustomContentProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenuItem, bool>(
            nameof(HasCustomContent),
            o => o.HasCustomContent);

    /// <inheritdoc cref="DisplayText" />
    public static readonly DirectProperty<DropdownMenuItem, string> DisplayTextProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenuItem, string>(
            nameof(DisplayText),
            o => o.DisplayText);

    private bool _hasCustomContent;
    private string _displayText = string.Empty;

    static DropdownMenuItem()
    {
        ValueProperty.Changed.AddClassHandler<DropdownMenuItem>((control, _) => control.SyncSelectedValuesFromValue());
        LabelProperty.Changed.AddClassHandler<DropdownMenuItem>((control, _) => control.UpdateDisplayText());
        IsOpenProperty.Changed.AddClassHandler<DropdownMenuItem>((control, args) =>
        {
            var isOpen = args.GetNewValue<bool>();
            control.PseudoClasses.Set(":open", isOpen);
            if (isOpen)
            {
                control._owner?.SetExpandedItem(control);
            }
        });
        IsDisabledProperty.Changed.AddClassHandler<DropdownMenuItem>((control, args) =>
        {
            control.PseudoClasses.Set(":disabled", args.GetNewValue<bool>());
        });
        MultipleProperty.Changed.AddClassHandler<DropdownMenuItem>((control, args) =>
        {
            control.PseudoClasses.Set(":multiple", args.GetNewValue<bool>());
            control.SyncSelectedValuesFromValue();
            control.UpdateDisplayText();
        });
        OptionsColumnsProperty.Changed.AddClassHandler<DropdownMenuItem>((control, _) => control.InvalidateVisual());
    }

    /// <summary>
    /// 初始化 <see cref="DropdownMenuItem"/> 的新实例。
    /// </summary>
    public DropdownMenuItem()
    {
        _options.CollectionChanged += OnOptionsChanged;
        _selectedValues.CollectionChanged += OnSelectedValuesChanged;
        UpdateHasCustomContent();
        UpdateDisplayText();
    }

    /// <summary>
    /// 获取选项集合。
    /// </summary>
    public AvaloniaList<DropdownMenuOption> Options => _options;

    /// <summary>
    /// 获取多选值集合。
    /// </summary>
    public AvaloniaList<object?> SelectedValues => _selectedValues;

    /// <summary>
    /// 获取或设置单选值。
    /// </summary>
    public object? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    /// <summary>
    /// 获取或设置顶部菜单文本。
    /// </summary>
    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    /// <summary>
    /// 获取或设置顶部菜单自定义图标。
    /// </summary>
    public object? HeaderIcon
    {
        get => GetValue(HeaderIconProperty);
        set => SetValue(HeaderIconProperty, value);
    }

    /// <summary>
    /// 获取或设置当前是否展开。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置是否禁用。
    /// </summary>
    public bool IsDisabled
    {
        get => GetValue(IsDisabledProperty);
        set => SetValue(IsDisabledProperty, value);
    }

    /// <summary>
    /// 获取或设置是否为多选模式。
    /// </summary>
    public bool Multiple
    {
        get => GetValue(MultipleProperty);
        set => SetValue(MultipleProperty, value);
    }

    /// <summary>
    /// 获取或设置选项列数。
    /// </summary>
    public int OptionsColumns
    {
        get => GetValue(OptionsColumnsProperty);
        set => SetValue(OptionsColumnsProperty, value);
    }

    /// <summary>
    /// 获取或设置底部操作内容。
    /// </summary>
    public object? Footer
    {
        get => GetValue(FooterProperty);
        set => SetValue(FooterProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在自定义内容。
    /// </summary>
    public bool HasCustomContent
    {
        get => _hasCustomContent;
        private set => SetAndRaise(HasCustomContentProperty, ref _hasCustomContent, value);
    }

    /// <summary>
    /// 获取当前顶部显示文本。
    /// </summary>
    public string DisplayText
    {
        get => _displayText;
        private set => SetAndRaise(DisplayTextProperty, ref _displayText, value);
    }

    /// <summary>
    /// 获取当前是否已选择值。
    /// </summary>
    public bool HasSelectionValue => HasSelection();

    /// <summary>
    /// 选中值变化时触发。
    /// </summary>
    public event EventHandler<DropdownMenuItemValueChangedEventArgs>? ValueChanged;

    internal void AttachOwner(DropdownMenu owner)
    {
        _owner = owner;
    }

    internal void DetachOwner(DropdownMenu owner)
    {
        if (ReferenceEquals(_owner, owner))
        {
            _owner = null;
        }
    }

    /// <inheritdoc />
    protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
    {
        base.OnPropertyChanged(change);

        if (change.Property == ContentProperty)
        {
            UpdateHasCustomContent();
        }
    }

    internal void ToggleOpen()
    {
        if (IsDisabled)
        {
            return;
        }

        IsOpen = !IsOpen;
    }

    internal void Close()
    {
        IsOpen = false;
    }

    internal void SelectOption(DropdownMenuOption option)
    {
        if (option.IsDisabled)
        {
            return;
        }

        if (Multiple)
        {
            ToggleSelectedValue(option.Value);
            RaiseValueChanged();
            UpdateDisplayText();
            return;
        }

        Value = option.Value;
        RaiseValueChanged();
        UpdateDisplayText();
        _owner?.CollapseMenu();
    }

    internal bool IsOptionSelected(DropdownMenuOption option)
    {
        if (Multiple)
        {
            return _selectedValues.Any(value => Equals(value, option.Value));
        }

        return Equals(Value, option.Value);
    }

    private void ToggleSelectedValue(object? value)
    {
        var existingIndex = _selectedValues
            .Select((item, index) => (item, index))
            .FirstOrDefault(pair => Equals(pair.item, value))
            .index;

        if (existingIndex >= 0 && existingIndex < _selectedValues.Count && Equals(_selectedValues[existingIndex], value))
        {
            _selectedValues.RemoveAt(existingIndex);
            return;
        }

        _selectedValues.Add(value);
    }

    private void SyncSelectedValuesFromValue()
    {
        if (!Multiple)
        {
            _suppressSelectedValuesChanged = true;
            try
            {
                _selectedValues.Clear();
            }
            finally
            {
                _suppressSelectedValuesChanged = false;
            }

            UpdateDisplayText();
            return;
        }

        if (Value is IEnumerable enumerable and not string)
        {
            _suppressSelectedValuesChanged = true;
            try
            {
                _selectedValues.Clear();
                foreach (var item in enumerable)
                {
                    _selectedValues.Add(item);
                }
            }
            finally
            {
                _suppressSelectedValuesChanged = false;
            }
        }

        UpdateDisplayText();
    }

    private void OnSelectedValuesChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (_suppressSelectedValuesChanged || !Multiple)
        {
            return;
        }

        Value = _selectedValues.ToArray();
        UpdateDisplayText();
    }

    private void OnOptionsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateDisplayText();
    }

    private void UpdateHasCustomContent()
    {
        HasCustomContent = Content is not null;
        PseudoClasses.Set(":custom", HasCustomContent);
    }

    private void UpdateDisplayText()
    {
        var explicitLabel = Label;
        if (!string.IsNullOrWhiteSpace(explicitLabel))
        {
            DisplayText = explicitLabel!;
            PseudoClasses.Set(":selected", HasSelection());
            return;
        }

        if (Multiple)
        {
            var selectedLabels = _options
                .Where(IsOptionSelected)
                .Select(option => option.Label)
                .Where(label => !string.IsNullOrWhiteSpace(label))
                .ToArray();

            DisplayText = selectedLabels.Length > 0 ? string.Join("、", selectedLabels) : "筛选";
            PseudoClasses.Set(":selected", selectedLabels.Length > 0);
            return;
        }

        var selected = _options.FirstOrDefault(option => Equals(option.Value, Value));
        DisplayText = !string.IsNullOrWhiteSpace(selected?.Label) ? selected!.Label : "筛选";
        PseudoClasses.Set(":selected", selected is not null);
    }

    private bool HasSelection()
    {
        if (Multiple)
        {
            return _selectedValues.Count > 0;
        }

        return Value is not null;
    }

    private void RaiseValueChanged()
    {
        var values = Multiple
            ? _selectedValues.ToArray()
            : (Value is null ? Array.Empty<object?>() : [Value]);

        ValueChanged?.Invoke(this, new DropdownMenuItemValueChangedEventArgs(Value, values));
    }
}

/// <summary>
/// 下拉菜单容器。
/// </summary>
[PseudoClasses(":open", ":up", ":down")]
[TemplatePart(HeaderHostPartName, typeof(Grid))]
[TemplatePart(OverlayPartName, typeof(Overlay))]
[TemplatePart(PanelBorderPartName, typeof(Border))]
[TemplatePart(PanelHostPartName, typeof(ContentPresenter))]
public sealed class DropdownMenu : TemplatedControl
{
    private const string HeaderHostPartName = "PART_HeaderHost";
    private const string OverlayPartName = "PART_Overlay";
    private const string PanelBorderPartName = "PART_PanelBorder";
    private const string PanelHostPartName = "PART_PanelHost";
    private readonly AvaloniaList<DropdownMenuItem> _items = [];
    private readonly Dictionary<DropdownMenuItem, HeaderRefs> _headerRefs = [];
    private Grid? _headerHost;
    private Overlay? _overlay;
    private Border? _panelBorder;
    private ContentPresenter? _panelHost;
    private DropdownMenuItem? _expandedItem;

    /// <inheritdoc cref="Items" />
    public static readonly DirectProperty<DropdownMenu, AvaloniaList<DropdownMenuItem>> ItemsProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenu, AvaloniaList<DropdownMenuItem>>(
            nameof(Items),
            o => o.Items);

    /// <inheritdoc cref="Direction" />
    public static readonly StyledProperty<DropdownMenuDirection> DirectionProperty =
        AvaloniaProperty.Register<DropdownMenu, DropdownMenuDirection>(nameof(Direction), DropdownMenuDirection.Down);

    /// <inheritdoc cref="OverlayVisible" />
    public static readonly DirectProperty<DropdownMenu, bool> OverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<DropdownMenu, bool>(
            nameof(OverlayVisible),
            o => o.OverlayVisible);

    private bool _overlayVisible;

    static DropdownMenu()
    {
        DirectionProperty.Changed.AddClassHandler<DropdownMenu>((control, args) =>
        {
            var direction = args.GetNewValue<DropdownMenuDirection>();
            control.PseudoClasses.Set(":up", direction == DropdownMenuDirection.Up);
            control.PseudoClasses.Set(":down", direction == DropdownMenuDirection.Down);
        });
    }

    /// <summary>
    /// 初始化 <see cref="DropdownMenu"/> 的新实例。
    /// </summary>
    public DropdownMenu()
    {
        _items.CollectionChanged += OnItemsChanged;
        PseudoClasses.Set(":down", true);
    }

    /// <summary>
    /// 获取菜单项集合。
    /// </summary>
    [Content]
    public AvaloniaList<DropdownMenuItem> Items => _items;

    /// <summary>
    /// 获取或设置展开方向。
    /// </summary>
    public DropdownMenuDirection Direction
    {
        get => GetValue(DirectionProperty);
        set => SetValue(DirectionProperty, value);
    }

    /// <summary>
    /// 获取当前遮罩是否显示。
    /// </summary>
    public bool OverlayVisible
    {
        get => _overlayVisible;
        private set => SetAndRaise(OverlayVisibleProperty, ref _overlayVisible, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
        {
            _overlay.Clicked -= OnOverlayClicked;
        }

        _headerHost = e.NameScope.Find<Grid>(HeaderHostPartName);
        _overlay = e.NameScope.Find<Overlay>(OverlayPartName);
        _panelBorder = e.NameScope.Find<Border>(PanelBorderPartName);
        _panelHost = e.NameScope.Find<ContentPresenter>(PanelHostPartName);

        if (_overlay is not null)
        {
            _overlay.Clicked += OnOverlayClicked;
        }

        BuildHeaders();
        UpdateExpandedPresentation();
    }

    /// <inheritdoc />
    protected override Size ArrangeOverride(Size finalSize)
    {
        var arranged = base.ArrangeOverride(finalSize);
        UpdatePanelPlacement();
        return arranged;
    }

    /// <summary>
    /// 收起当前菜单。
    /// </summary>
    public void CollapseMenu()
    {
        if (_expandedItem is not null)
        {
            _expandedItem.IsOpen = false;
            _expandedItem = null;
        }

        UpdateExpandedPresentation();
    }

    /// <summary>
    /// 设置当前展开项。
    /// </summary>
    public void SetExpandedItem(DropdownMenuItem item)
    {
        if (_expandedItem is not null && !ReferenceEquals(_expandedItem, item))
        {
            _expandedItem.IsOpen = false;
        }

        _expandedItem = item;
        OverlayVisible = item.IsOpen;
        PseudoClasses.Set(":open", item.IsOpen);

        if (!item.IsOpen)
        {
            _expandedItem = null;
        }

        UpdateExpandedPresentation();
    }

    internal void ToggleItem(DropdownMenuItem item)
    {
        if (item.IsDisabled)
        {
            return;
        }

        if (_expandedItem is not null && !ReferenceEquals(_expandedItem, item))
        {
            _expandedItem.IsOpen = false;
        }

        item.ToggleOpen();
        if (!item.IsOpen)
        {
            _expandedItem = null;
        }

        UpdateExpandedPresentation();
    }

    internal void OnOverlayPressed()
    {
        CollapseMenu();
    }

    private void OnItemsChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        if (e.OldItems is not null)
        {
            foreach (var item in e.OldItems.OfType<DropdownMenuItem>())
            {
                item.DetachOwner(this);
                item.ValueChanged -= OnItemValueChanged;
                item.PropertyChanged -= OnItemPropertyChanged;
            }
        }

        if (e.NewItems is not null)
        {
            foreach (var item in e.NewItems.OfType<DropdownMenuItem>())
            {
                item.AttachOwner(this);
                item.ValueChanged += OnItemValueChanged;
                item.PropertyChanged += OnItemPropertyChanged;
            }
        }

        BuildHeaders();
        UpdateExpandedPresentation();
    }

    private void OnItemValueChanged(object? sender, DropdownMenuItemValueChangedEventArgs e)
    {
        if (sender is DropdownMenuItem item)
        {
            UpdateHeaderState(item);
            if (ReferenceEquals(_expandedItem, item))
            {
                UpdatePanelContent();
            }
        }
    }

    private void OnItemPropertyChanged(object? sender, AvaloniaPropertyChangedEventArgs e)
    {
        if (sender is not DropdownMenuItem item)
        {
            return;
        }

        if (e.Property == DropdownMenuItem.IsOpenProperty)
        {
            if (item.IsOpen)
            {
                SetExpandedItem(item);
            }
            else if (ReferenceEquals(_expandedItem, item))
            {
                _expandedItem = null;
                UpdateExpandedPresentation();
            }

            return;
        }

        UpdateHeaderState(item);
        if (ReferenceEquals(_expandedItem, item))
        {
            UpdatePanelContent();
        }
    }

    private void OnOverlayClicked(object? sender, EventArgs e)
    {
        OnOverlayPressed();
    }

    private void BuildHeaders()
    {
        if (_headerHost is null)
        {
            return;
        }

        _headerHost.Children.Clear();
        _headerRefs.Clear();

        var panel = new UniformGrid
        {
            Rows = 1,
            Columns = Math.Max(1, _items.Count),
        };

        foreach (var item in _items)
        {
            var refs = CreateHeader(item);
            _headerRefs[item] = refs;
            panel.Children.Add(refs.Host);
        }

        _headerHost.Children.Add(panel);
        UpdateAllHeaderStates();
    }

    private HeaderRefs CreateHeader(DropdownMenuItem item)
    {
        var textBlock = new TextBlock
        {
            FontSize = 16,
            VerticalAlignment = VerticalAlignment.Center,
        };

        var customIcon = new ContentPresenter
        {
            Content = item.HeaderIcon,
            VerticalAlignment = VerticalAlignment.Center,
            HorizontalAlignment = HorizontalAlignment.Center,
            IsVisible = item.HeaderIcon is not null,
        };

        var icon = new Path
        {
            Width = 10,
            Height = 10,
            Stretch = Stretch.Uniform,
            Data = Geometry.Parse("M2 3L5 7L8 3"),
            VerticalAlignment = VerticalAlignment.Center,
            IsVisible = item.HeaderIcon is null,
        };

        var iconRotate = new RotateTransform();
        icon.RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative);
        icon.RenderTransform = iconRotate;

        var content = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,10"),
            ColumnSpacing = 6,
            HorizontalAlignment = HorizontalAlignment.Center,
        };
        content.Children.Add(textBlock);
        content.Children.Add(customIcon);
        content.Children.Add(icon);
        Grid.SetColumn(customIcon, 1);
        Grid.SetColumn(icon, 1);

        var host = new Border
        {
            Background = Brushes.Transparent,
            Padding = new Thickness(16, 12),
            Child = content,
        };
        host.PointerPressed += (_, args) =>
        {
            ToggleItem(item);
            args.Handled = true;
        };

        return new HeaderRefs(host, textBlock, customIcon, icon, iconRotate);
    }

    private void UpdateExpandedPresentation()
    {
        OverlayVisible = _expandedItem?.IsOpen == true;
        PseudoClasses.Set(":open", OverlayVisible);
        UpdateAllHeaderStates();
        UpdatePanelPlacement();
        UpdatePanelContent();
    }

    private void UpdateAllHeaderStates()
    {
        foreach (var item in _items)
        {
            UpdateHeaderState(item);
        }
    }

    private void UpdateHeaderState(DropdownMenuItem item)
    {
        if (!_headerRefs.TryGetValue(item, out var refs))
        {
            return;
        }

        refs.Text.Text = item.DisplayText;
        refs.CustomIcon.Content = item.HeaderIcon;
        refs.CustomIcon.IsVisible = item.HeaderIcon is not null;
        refs.Icon.IsVisible = item.HeaderIcon is null;

        var foreground = item.IsDisabled
            ? GetBrush("Luna.Brush.Text.Disabled", Brushes.Gray)
            : (item.IsOpen || item.HasSelectionValue
                ? GetBrush("Luna.Brush.Brand", Brushes.Blue)
                : GetBrush("Luna.Brush.Text.Secondary", Brushes.Gray));

        refs.Text.Foreground = foreground;
        refs.Icon.Fill = foreground;
        refs.IconRotate.Angle = item.HeaderIcon is null && item.IsOpen ? 180 : 0;
        if (item.HeaderIcon is Shape shape)
        {
            shape.Fill = foreground;
            shape.Stroke = foreground;
        }

        if (item.HeaderIcon is TextBlock iconText)
        {
            iconText.Foreground = foreground;
        }

        refs.Host.Opacity = item.IsDisabled ? 0.56 : 1;
    }

    private void UpdatePanelPlacement()
    {
        if (_panelBorder is null)
        {
            return;
        }

        var headerHeight = _headerHost?.Bounds.Height ?? Bounds.Height;
        if (headerHeight <= 0)
        {
            headerHeight = 44;
        }

        if (Direction == DropdownMenuDirection.Up)
        {
            _panelBorder.VerticalAlignment = VerticalAlignment.Bottom;
            _panelBorder.Margin = new Thickness(0, 0, 0, headerHeight);
        }
        else
        {
            _panelBorder.VerticalAlignment = VerticalAlignment.Top;
            _panelBorder.Margin = new Thickness(0, headerHeight, 0, 0);
        }
    }

    private void UpdatePanelContent()
    {
        if (_panelHost is null)
        {
            return;
        }

        if (_expandedItem is null || !_expandedItem.IsOpen)
        {
            _panelHost.Content = null;
            return;
        }

        _panelHost.Content = BuildPanelContent(_expandedItem);
    }

    private Control BuildPanelContent(DropdownMenuItem item)
    {
        var stack = new StackPanel();

        if (item.HasCustomContent)
        {
            stack.Children.Add(new ContentPresenter
            {
                Content = item.Content,
            });
        }
        else
        {
            stack.Children.Add(BuildOptionsContent(item));
        }

        if (item.Footer is not null)
        {
            stack.Children.Add(new ContentPresenter
            {
                Content = item.Footer,
            });
        }

        return stack;
    }

    private Control BuildOptionsContent(DropdownMenuItem item)
    {
        if (!item.Multiple && item.OptionsColumns <= 1)
        {
            var stack = new StackPanel
            {
                Margin = new Thickness(0, 4),
            };

            foreach (var option in item.Options)
            {
                stack.Children.Add(BuildSingleOption(item, option));
            }

            return stack;
        }

        var grid = new UniformGrid
        {
            Columns = Math.Max(1, item.OptionsColumns),
            Margin = new Thickness(16, 8, 16, 16),
        };

        foreach (var option in item.Options)
        {
            grid.Children.Add(BuildMultipleOption(item, option));
        }

        return grid;
    }

    private Control BuildSingleOption(DropdownMenuItem item, DropdownMenuOption option)
    {
        var text = new TextBlock
        {
            Text = option.Label,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = option.IsDisabled
                ? GetBrush("Luna.Brush.Text.Disabled", Brushes.Gray)
                : GetBrush(item.IsOptionSelected(option) ? "Luna.Brush.Brand" : "Luna.Brush.Text.Primary", Brushes.Black),
        };

        var row = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("*,20"),
        };
        row.Margin = new Thickness(16, 14);
        row.Children.Add(text);

        if (item.IsOptionSelected(option))
        {
            var check = new Path
            {
                Width = 14,
                Height = 14,
                Stretch = Stretch.Uniform,
                Fill = GetBrush("Luna.Brush.Brand", Brushes.Blue),
                Data = Geometry.Parse("M2 7L5.2 10.2L12 3.5L10.8 2.3L5.2 7.9L3.2 5.9Z"),
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Right,
            };
            row.Children.Add(check);
            Grid.SetColumn(check, 1);
        }

        var host = new Border
        {
            Child = row,
            Background = Brushes.Transparent,
        };
        host.PointerPressed += (_, args) =>
        {
            item.SelectOption(option);
            UpdateExpandedPresentation();
            args.Handled = true;
        };

        return host;
    }

    private Control BuildMultipleOption(DropdownMenuItem item, DropdownMenuOption option)
    {
        var isSelected = item.IsOptionSelected(option);
        var background = option.IsDisabled
            ? GetBrush("Luna.Brush.Background.Component", Brushes.LightGray)
            : GetBrush(isSelected ? "Luna.Brush.Brand.Subtle" : "Luna.Brush.Background.Component", Brushes.WhiteSmoke);
        var foreground = option.IsDisabled
            ? GetBrush("Luna.Brush.Text.Disabled", Brushes.Gray)
            : GetBrush(isSelected ? "Luna.Brush.Brand" : "Luna.Brush.Text.Primary", Brushes.Black);

        var content = new Border
        {
            Margin = new Thickness(4),
            Padding = new Thickness(12, 10),
            CornerRadius = new CornerRadius(8),
            Background = background,
            Child = new TextBlock
            {
                Text = option.Label,
                HorizontalAlignment = HorizontalAlignment.Center,
                TextAlignment = TextAlignment.Center,
                Foreground = foreground,
                TextWrapping = TextWrapping.Wrap,
            },
        };

        content.PointerPressed += (_, args) =>
        {
            item.SelectOption(option);
            UpdateExpandedPresentation();
            args.Handled = true;
        };

        return content;
    }

    private IBrush GetBrush(string resourceKey, IBrush fallback)
    {
        return this.TryFindResource(resourceKey, out var value) && value is IBrush brush ? brush : fallback;
    }

    private sealed class HeaderRefs
    {
        public HeaderRefs(Border host, TextBlock text, ContentPresenter customIcon, Path icon, RotateTransform iconRotate)
        {
            Host = host;
            Text = text;
            CustomIcon = customIcon;
            Icon = icon;
            IconRotate = iconRotate;
        }

        public Border Host { get; }

        public TextBlock Text { get; }

        public ContentPresenter CustomIcon { get; }

        public Path Icon { get; }

        public RotateTransform IconRotate { get; }
    }
}
