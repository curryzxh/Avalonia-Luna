using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;

namespace Luna.Desktop.Controls;

[PseudoClasses(":selected")]
public class AnchorItem : ContentControl
{
    public static readonly StyledProperty<string?> AnchorIdProperty =
        AvaloniaProperty.Register<AnchorItem, string?>(nameof(AnchorId));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<AnchorItem, string?>(nameof(Text));

    public static readonly StyledProperty<bool> IsSelectedProperty =
        AvaloniaProperty.Register<AnchorItem, bool>(nameof(IsSelected));

    public string? AnchorId
    {
        get => GetValue(AnchorIdProperty);
        set => SetValue(AnchorIdProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsSelected
    {
        get => GetValue(IsSelectedProperty);
        set => SetValue(IsSelectedProperty, value);
    }

    static AnchorItem()
    {
        IsSelectedProperty.Changed.AddClassHandler<AnchorItem>(OnIsSelectedChanged);
    }

    private static void OnIsSelectedChanged(AnchorItem sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":selected", sender.IsSelected);
    }
}
