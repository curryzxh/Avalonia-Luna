using System;
using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Templates;
using Avalonia.Data;
using Avalonia.Layout;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

public class Breadcrumb : ItemsControl
{
    public static readonly StyledProperty<object?> SeparatorProperty =
        AvaloniaProperty.Register<Breadcrumb, object?>(nameof(Separator), "/");

    public static readonly StyledProperty<BindingBase?> CommandBindingProperty =
        AvaloniaProperty.Register<Breadcrumb, BindingBase?>(nameof(CommandBinding));

    public object? Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    [AssignBinding]
    public BindingBase? CommandBinding
    {
        get => GetValue(CommandBindingProperty);
        set => SetValue(CommandBindingProperty, value);
    }

    static Breadcrumb()
    {
        ItemsPanelProperty.OverrideDefaultValue<Breadcrumb>(
            new FuncTemplate<Panel?>(() => new StackPanel { Orientation = Orientation.Horizontal }));
    }

    protected override bool NeedsContainerOverride(object? item, int index, out object? recycleKey)
    {
        return NeedsContainer<BreadcrumbItem>(item, out recycleKey);
    }

    protected override Control CreateContainerForItemOverride(object? item, int index, object? recycleKey)
    {
        return new BreadcrumbItem();
    }

    protected override void PrepareContainerForItemOverride(Control container, object? item, int index)
    {
        if (container is not BreadcrumbItem breadcrumbItem) return;

        if (!breadcrumbItem.IsSet(BreadcrumbItem.SeparatorProperty) && Separator is not null)
        {
            breadcrumbItem.Separator = Separator;
        }

        if (container == item) return;

        if (!breadcrumbItem.IsSet(ContentControl.ContentProperty))
        {
            breadcrumbItem.SetCurrentValue(ContentControl.ContentProperty, item);
            if (DisplayMemberBinding is not null)
            {
                breadcrumbItem[!ContentControl.ContentProperty] = DisplayMemberBinding;
            }
        }

        if (!breadcrumbItem.IsSet(ContentControl.ContentTemplateProperty) && ItemTemplate != null)
        {
            breadcrumbItem.SetCurrentValue(ContentControl.ContentTemplateProperty, ItemTemplate);
        }

        if (!breadcrumbItem.IsSet(BreadcrumbItem.CommandProperty) && CommandBinding != null)
        {
            breadcrumbItem[!BreadcrumbItem.CommandProperty] = CommandBinding;
        }
    }

    internal void InvalidateContainers()
    {
        var items = this.GetVisualDescendants().OfType<BreadcrumbItem>().ToList();
        for (var i = 0; i < items.Count; i++)
        {
            items[i].SetPseudoClassLast(i == items.Count - 1);
        }
    }
}

public class BreadcrumbItem : ContentControl
{
    private const string PC_Last = ":last";

    public static readonly StyledProperty<object?> SeparatorProperty =
        AvaloniaProperty.Register<BreadcrumbItem, object?>(nameof(Separator));

    public static readonly StyledProperty<ICommand?> CommandProperty =
        AvaloniaProperty.Register<BreadcrumbItem, ICommand?>(nameof(Command));

    public static readonly StyledProperty<object?> CommandParameterProperty =
        AvaloniaProperty.Register<BreadcrumbItem, object?>(nameof(CommandParameter));

    public object? Separator
    {
        get => GetValue(SeparatorProperty);
        set => SetValue(SeparatorProperty, value);
    }

    public ICommand? Command
    {
        get => GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public object? CommandParameter
    {
        get => GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    internal void SetPseudoClassLast(bool isLast)
    {
        PseudoClasses.Set(PC_Last, isLast);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        if (this.GetVisualParent() is Breadcrumb parent)
        {
            parent.InvalidateContainers();
        }
    }
}
