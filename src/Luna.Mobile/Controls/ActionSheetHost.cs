using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Luna.Mobile.Controls;

public enum ActionSheetTheme
{
    List,
    Grid,
}

public enum ActionSheetAlign
{
    Center,
    Left,
}

public enum ActionSheetCloseReason
{
    Unknown,
    Overlay,
    Cancel,
    Selected,
    Programmatic,
}

public sealed class ActionSheetItem
{
    public required string Label { get; init; }
    public object? Icon { get; init; }
    public bool IsDisabled { get; init; }
    public IBrush? Foreground { get; init; }
    public bool BadgeDot { get; init; }
    public string? BadgeCount { get; init; }

    public bool HasIcon => Icon is not null;
    public bool HasBadge => BadgeDot || !string.IsNullOrWhiteSpace(BadgeCount);
    public bool HasNoBadge => !HasBadge;
    public bool HasCustomForeground => Foreground is not null;
    public bool HasDefaultForeground => Foreground is null;
    public bool IsEnabled => !IsDisabled;
}

public sealed class ActionSheetOptions
{
    public IReadOnlyList<ActionSheetItem> Items { get; init; } = Array.Empty<ActionSheetItem>();
    public string? Description { get; init; }
    public string CancelText { get; init; } = "取消";
    public bool ShowOverlay { get; init; } = true;
    public ActionSheetTheme Theme { get; init; } = ActionSheetTheme.List;
    public ActionSheetAlign Align { get; init; } = ActionSheetAlign.Center;
    public bool CloseOnSelect { get; init; } = true;
}

public sealed class ActionSheetItemSelectedEventArgs : EventArgs
{
    public ActionSheetItemSelectedEventArgs(ActionSheetItem item, int index)
    {
        Item = item;
        Index = index;
    }

    public ActionSheetItem Item { get; }
    public int Index { get; }
}

public sealed class ActionSheetClosedEventArgs : EventArgs
{
    public ActionSheetClosedEventArgs(ActionSheetCloseReason reason)
    {
        Reason = reason;
    }

    public ActionSheetCloseReason Reason { get; }
}

public sealed class ActionSheetHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string CancelButtonPartName = "PART_CancelButton";

    private static ActionSheetHost? _current;

    private Border? _overlay;
    private Button? _cancelButton;
    private ActionSheetCloseReason _closeReason = ActionSheetCloseReason.Unknown;

    private bool _isOverlayVisible;
    private bool _isDescriptionVisible;
    private bool _isListVisible = true;
    private bool _isGridVisible;
    private bool _isAlignLeft;
    private TextAlignment _itemTextAlignment = TextAlignment.Center;
    private HorizontalAlignment _itemHorizontalAlignment = HorizontalAlignment.Center;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<ActionSheetHost, string?>(nameof(Description));

    public static readonly DirectProperty<ActionSheetHost, bool> IsDescriptionVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsDescriptionVisible),
            o => o.IsDescriptionVisible);

    public static readonly StyledProperty<string> CancelTextProperty =
        AvaloniaProperty.Register<ActionSheetHost, string>(nameof(CancelText), "取消");

    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(ShowOverlay), true);

    public static readonly DirectProperty<ActionSheetHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    public static readonly StyledProperty<IReadOnlyList<ActionSheetItem>> ItemsProperty =
        AvaloniaProperty.Register<ActionSheetHost, IReadOnlyList<ActionSheetItem>>(nameof(Items), Array.Empty<ActionSheetItem>());

    public static readonly StyledProperty<ActionSheetTheme> ThemeProperty =
        AvaloniaProperty.Register<ActionSheetHost, ActionSheetTheme>(nameof(Theme), ActionSheetTheme.List);

    public static readonly StyledProperty<ActionSheetAlign> AlignProperty =
        AvaloniaProperty.Register<ActionSheetHost, ActionSheetAlign>(nameof(Align), ActionSheetAlign.Center);

    public static readonly DirectProperty<ActionSheetHost, bool> IsListVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsListVisible),
            o => o.IsListVisible);

    public static readonly DirectProperty<ActionSheetHost, bool> IsGridVisibleProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsGridVisible),
            o => o.IsGridVisible);

    public static readonly DirectProperty<ActionSheetHost, bool> IsAlignLeftProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, bool>(
            nameof(IsAlignLeft),
            o => o.IsAlignLeft);

    public static readonly DirectProperty<ActionSheetHost, TextAlignment> ItemTextAlignmentProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, TextAlignment>(
            nameof(ItemTextAlignment),
            o => o.ItemTextAlignment);

    public static readonly DirectProperty<ActionSheetHost, HorizontalAlignment> ItemHorizontalAlignmentProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, HorizontalAlignment>(
            nameof(ItemHorizontalAlignment),
            o => o.ItemHorizontalAlignment);

    public static readonly DirectProperty<ActionSheetHost, ICommand> ItemClickCommandProperty =
        AvaloniaProperty.RegisterDirect<ActionSheetHost, ICommand>(
            nameof(ItemClickCommand),
            o => o.ItemClickCommand);

    public static readonly StyledProperty<bool> CloseOnSelectProperty =
        AvaloniaProperty.Register<ActionSheetHost, bool>(nameof(CloseOnSelect), true);

    static ActionSheetHost()
    {
        IsOpenProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdateOverlayVisible());
        ShowOverlayProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdateOverlayVisible());

        DescriptionProperty.Changed.AddClassHandler<ActionSheetHost>((control, args) =>
        {
            control.IsDescriptionVisible = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });

        ThemeProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdatePseudoClasses());
        AlignProperty.Changed.AddClassHandler<ActionSheetHost>((control, _) => control.UpdatePseudoClasses());
    }

    public ActionSheetHost()
    {
        ItemClickCommand = new ItemClickCommandImpl(this);
        IsDescriptionVisible = !string.IsNullOrWhiteSpace(Description);
        UpdatePseudoClasses();
    }

    public static ActionSheetHost? Current => _current;

    public event EventHandler<ActionSheetItemSelectedEventArgs>? Selected;
    public event EventHandler? CancelRequested;
    public event EventHandler<ActionSheetClosedEventArgs>? Closed;

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public bool IsDescriptionVisible
    {
        get => _isDescriptionVisible;
        private set => SetAndRaise(IsDescriptionVisibleProperty, ref _isDescriptionVisible, value);
    }

    public string CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
    }

    public IReadOnlyList<ActionSheetItem> Items
    {
        get => GetValue(ItemsProperty);
        set => SetValue(ItemsProperty, value);
    }

    public ActionSheetTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public ActionSheetAlign Align
    {
        get => GetValue(AlignProperty);
        set => SetValue(AlignProperty, value);
    }

    public bool IsListVisible
    {
        get => _isListVisible;
        private set => SetAndRaise(IsListVisibleProperty, ref _isListVisible, value);
    }

    public bool IsGridVisible
    {
        get => _isGridVisible;
        private set => SetAndRaise(IsGridVisibleProperty, ref _isGridVisible, value);
    }

    public bool IsAlignLeft
    {
        get => _isAlignLeft;
        private set => SetAndRaise(IsAlignLeftProperty, ref _isAlignLeft, value);
    }

    public TextAlignment ItemTextAlignment
    {
        get => _itemTextAlignment;
        private set => SetAndRaise(ItemTextAlignmentProperty, ref _itemTextAlignment, value);
    }

    public HorizontalAlignment ItemHorizontalAlignment
    {
        get => _itemHorizontalAlignment;
        private set => SetAndRaise(ItemHorizontalAlignmentProperty, ref _itemHorizontalAlignment, value);
    }

    public ICommand ItemClickCommand { get; }

    public bool CloseOnSelect
    {
        get => GetValue(CloseOnSelectProperty);
        set => SetValue(CloseOnSelectProperty, value);
    }

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

    public void Close(ActionSheetCloseReason reason = ActionSheetCloseReason.Programmatic)
    {
        if (!IsOpen)
        {
            return;
        }

        _closeReason = reason;
        IsOpen = false;
        Closed?.Invoke(this, new ActionSheetClosedEventArgs(reason));
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
            _overlay.PointerPressed -= OnOverlayPointerPressed;
        }

        if (_cancelButton is not null)
        {
            _cancelButton.Click -= OnCancelButtonClick;
        }

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPointerPressed;
        }

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

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen && ShowOverlay;
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

public static class ActionSheet
{
    public static void Show(ActionSheetOptions options)
    {
        ActionSheetHost.Current?.Show(options);
    }

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
