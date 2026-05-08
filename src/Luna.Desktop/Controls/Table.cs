using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Luna.Desktop.Controls;

[PseudoClasses(":bordered", ":striped", ":hoverable", ":sorted-asc", ":sorted-desc")]
public class Table : ItemsControl
{
    public static readonly StyledProperty<bool> BorderedProperty =
        AvaloniaProperty.Register<Table, bool>(nameof(Bordered), true);

    public static readonly StyledProperty<bool> StripedProperty =
        AvaloniaProperty.Register<Table, bool>(nameof(Striped), false);

    public static readonly StyledProperty<bool> HoverableProperty =
        AvaloniaProperty.Register<Table, bool>(nameof(Hoverable), true);

    public static readonly StyledProperty<double> RowMinHeightProperty =
        AvaloniaProperty.Register<Table, double>(nameof(RowMinHeight), 48);

    public static readonly StyledProperty<ObservableCollection<TableColumn>> ColumnsProperty =
        AvaloniaProperty.Register<Table, ObservableCollection<TableColumn>>(nameof(Columns));

    public static readonly StyledProperty<TableColumn?> SortedColumnProperty =
        AvaloniaProperty.Register<Table, TableColumn?>(nameof(SortedColumn));

    public bool Bordered
    {
        get => GetValue(BorderedProperty);
        set => SetValue(BorderedProperty, value);
    }

    public bool Striped
    {
        get => GetValue(StripedProperty);
        set => SetValue(StripedProperty, value);
    }

    public bool Hoverable
    {
        get => GetValue(HoverableProperty);
        set => SetValue(HoverableProperty, value);
    }

    public double RowMinHeight
    {
        get => GetValue(RowMinHeightProperty);
        set => SetValue(RowMinHeightProperty, value);
    }

    public ObservableCollection<TableColumn> Columns
    {
        get => GetValue(ColumnsProperty);
        set => SetValue(ColumnsProperty, value);
    }

    public TableColumn? SortedColumn
    {
        get => GetValue(SortedColumnProperty);
        set => SetValue(SortedColumnProperty, value);
    }

    static Table()
    {
        BorderedProperty.Changed.AddClassHandler<Table>(OnBorderedChanged);
        StripedProperty.Changed.AddClassHandler<Table>(OnStripedChanged);
        HoverableProperty.Changed.AddClassHandler<Table>(OnHoverableChanged);
    }

    public Table()
    {
        Columns = new ObservableCollection<TableColumn>();
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdatePseudoClasses();
    }

    private static void OnBorderedChanged(Table sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":bordered", sender.Bordered);
    }

    private static void OnStripedChanged(Table sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":striped", sender.Striped);
    }

    private static void OnHoverableChanged(Table sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":hoverable", sender.Hoverable);
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":bordered", Bordered);
        PseudoClasses.Set(":striped", Striped);
        PseudoClasses.Set(":hoverable", Hoverable);
    }

    internal void ToggleSort(TableColumn column)
    {
        if (!column.Sortable) return;

        if (SortedColumn == column)
        {
            if (column.SortDirection == ListSortDirection.Ascending)
            {
                column.SortDirection = ListSortDirection.Descending;
                PseudoClasses.Set(":sorted-asc", false);
                PseudoClasses.Set(":sorted-desc", true);
            }
            else
            {
                column.SortDirection = null;
                SortedColumn = null;
                PseudoClasses.Set(":sorted-asc", false);
                PseudoClasses.Set(":sorted-desc", false);
            }
        }
        else
        {
            if (SortedColumn != null) SortedColumn.SortDirection = null;
            column.SortDirection = ListSortDirection.Ascending;
            SortedColumn = column;
            PseudoClasses.Set(":sorted-asc", true);
            PseudoClasses.Set(":sorted-desc", false);
        }
    }
}

public class TableColumn : AvaloniaObject
{
    public static readonly StyledProperty<string> HeaderProperty =
        AvaloniaProperty.Register<TableColumn, string>(nameof(Header));

    public static readonly StyledProperty<string> BindingPathProperty =
        AvaloniaProperty.Register<TableColumn, string>(nameof(BindingPath));

    public static readonly StyledProperty<double> WidthProperty =
        AvaloniaProperty.Register<TableColumn, double>(nameof(Width), double.NaN);

    public static readonly StyledProperty<double> MinWidthProperty =
        AvaloniaProperty.Register<TableColumn, double>(nameof(MinWidth), 80);

    public static readonly StyledProperty<bool> SortableProperty =
        AvaloniaProperty.Register<TableColumn, bool>(nameof(Sortable), false);

    public static readonly StyledProperty<TextAlignment> AlignmentProperty =
        AvaloniaProperty.Register<TableColumn, TextAlignment>(nameof(Alignment), TextAlignment.Left);

    public static readonly StyledProperty<ListSortDirection?> SortDirectionProperty =
        AvaloniaProperty.Register<TableColumn, ListSortDirection?>(nameof(SortDirection));

    public string Header
    {
        get => GetValue(HeaderProperty);
        set => SetValue(HeaderProperty, value);
    }

    public string BindingPath
    {
        get => GetValue(BindingPathProperty);
        set => SetValue(BindingPathProperty, value);
    }

    public double Width
    {
        get => GetValue(WidthProperty);
        set => SetValue(WidthProperty, value);
    }

    public double MinWidth
    {
        get => GetValue(MinWidthProperty);
        set => SetValue(MinWidthProperty, value);
    }

    public bool Sortable
    {
        get => GetValue(SortableProperty);
        set => SetValue(SortableProperty, value);
    }

    public TextAlignment Alignment
    {
        get => GetValue(AlignmentProperty);
        set => SetValue(AlignmentProperty, value);
    }

    public ListSortDirection? SortDirection
    {
        get => GetValue(SortDirectionProperty);
        set => SetValue(SortDirectionProperty, value);
    }
}
