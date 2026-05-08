using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class Descriptions : TemplatedControl
{
    public static readonly StyledProperty<IEnumerable?> ItemsSourceProperty =
        AvaloniaProperty.Register<Descriptions, IEnumerable?>(nameof(ItemsSource));

    public static readonly StyledProperty<int> ColumnCountProperty =
        AvaloniaProperty.Register<Descriptions, int>(nameof(ColumnCount), defaultValue: 2);

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Descriptions, string?>(nameof(Title));

    public static readonly StyledProperty<double> LabelWidthProperty =
        AvaloniaProperty.Register<Descriptions, double>(nameof(LabelWidth), defaultValue: 120);

    public IEnumerable? ItemsSource
    {
        get => GetValue(ItemsSourceProperty);
        set => SetValue(ItemsSourceProperty, value);
    }

    public int ColumnCount
    {
        get => GetValue(ColumnCountProperty);
        set => SetValue(ColumnCountProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }
}
