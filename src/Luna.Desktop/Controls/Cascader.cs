using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class Cascader : ContentControl
{
    public static readonly StyledProperty<AvaloniaList<string>?> ValuePathProperty =
        AvaloniaProperty.Register<Cascader, AvaloniaList<string>?>(nameof(ValuePath));

    public static readonly StyledProperty<string?> PlaceholderProperty =
        AvaloniaProperty.Register<Cascader, string?>(nameof(Placeholder), "请选择");

    public static readonly StyledProperty<bool> IsDropDownOpenProperty =
        AvaloniaProperty.Register<Cascader, bool>(nameof(IsDropDownOpen), false);

    public AvaloniaList<string>? ValuePath
    {
        get => GetValue(ValuePathProperty);
        set => SetValue(ValuePathProperty, value);
    }

    public string? Placeholder
    {
        get => GetValue(PlaceholderProperty);
        set => SetValue(PlaceholderProperty, value);
    }

    public bool IsDropDownOpen
    {
        get => GetValue(IsDropDownOpenProperty);
        set => SetValue(IsDropDownOpenProperty, value);
    }
}
