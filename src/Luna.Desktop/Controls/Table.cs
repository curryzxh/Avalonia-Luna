using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

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
}
