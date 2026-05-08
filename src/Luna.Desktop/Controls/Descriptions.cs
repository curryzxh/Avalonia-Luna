using System.Collections;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Luna.Desktop.Controls;

public enum DescriptionLayout
{
    Horizontal,
    Bordered,
}

public class DescriptionItem : AvaloniaObject
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<DescriptionItem, string?>(nameof(Label));

    public static readonly StyledProperty<string?> ValueProperty =
        AvaloniaProperty.Register<DescriptionItem, string?>(nameof(Value));

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }
}

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

    public static readonly StyledProperty<DescriptionLayout> LayoutProperty =
        AvaloniaProperty.Register<Descriptions, DescriptionLayout>(nameof(Layout), defaultValue: DescriptionLayout.Horizontal);

    public static readonly StyledProperty<bool> ShowColonProperty =
        AvaloniaProperty.Register<Descriptions, bool>(nameof(ShowColon), defaultValue: true);

    public static readonly StyledProperty<IDataTemplate?> ItemTemplateProperty =
        AvaloniaProperty.Register<Descriptions, IDataTemplate?>(nameof(ItemTemplate));

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

    public DescriptionLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public bool ShowColon
    {
        get => GetValue(ShowColonProperty);
        set => SetValue(ShowColonProperty, value);
    }

    public IDataTemplate? ItemTemplate
    {
        get => GetValue(ItemTemplateProperty);
        set => SetValue(ItemTemplateProperty, value);
    }
}
