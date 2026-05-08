using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Mixins;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.LogicalTree;

namespace Luna.Desktop.Controls;

public class AnchorItem : HeaderedItemsControl, ISelectable
{
    public static readonly StyledProperty<string?> AnchorIdProperty =
        AvaloniaProperty.Register<AnchorItem, string?>(nameof(AnchorId));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        SelectingItemsControl.IsSelectedProperty.AddOwner<AnchorItem>();

    internal static readonly DirectProperty<AnchorItem, int> LevelProperty =
        AvaloniaProperty.RegisterDirect<AnchorItem, int>(
            nameof(Level), o => o.Level, (o, v) => o.Level = v);

    private static readonly FuncTemplate<Panel?> DefaultPanel =
        new(() => new StackPanel());

    private Anchor? _root;

    static AnchorItem()
    {
        SelectableMixin.Attach<AnchorItem>(IsSelectedProperty);
        PressedMixin.Attach<AnchorItem>();
        ItemsPanelProperty.OverrideDefaultValue<AnchorItem>(DefaultPanel);
    }

    public int Level
    {
        get;
        set => SetAndRaise(LevelProperty, ref field, value);
    }

    public string? AnchorId
    {
        get => GetValue(AnchorIdProperty);
        set => SetValue(AnchorIdProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _root = this.GetLogicalAncestors().OfType<Anchor>().FirstOrDefault();
        Level = CalculateLevel();
        if (ItemTemplate is null && _root?.ItemTemplate is not null)
            SetCurrentValue(ItemTemplateProperty, _root.ItemTemplate);
        if (ItemContainerTheme is null && _root?.ItemContainerTheme is not null)
            SetCurrentValue(ItemContainerThemeProperty, _root.ItemContainerTheme);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return EnsureRoot().CreateContainerForItemOverrideInternal(item, index, recycleKey);
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return EnsureRoot().NeedsContainerOverrideInternal(item, index, out recycleKey);
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        EnsureRoot().PrepareContainerForItemOverrideInternal(container, item, index);
    }

    protected override void ContainerForItemPreparedOverride(Control container, object? item, int index)
    {
        EnsureRoot().ContainerForItemPreparedOverrideInternal(container, item, index);
    }

    private int CalculateLevel()
    {
        var level = 0;
        var current = this.GetLogicalParent();
        while (current is not null)
        {
            if (current is Anchor)
                return level - 1;
            if (current is AnchorItem)
                level++;
            current = current.GetLogicalParent();
        }
        return level;
    }

    private Anchor EnsureRoot()
    {
        return _root ?? throw new InvalidOperationException("AnchorItem must be inside an Anchor control.");
    }
}
