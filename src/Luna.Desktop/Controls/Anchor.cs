using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class Anchor : ContentControl
{
    public static readonly StyledProperty<string?> TargetIdProperty =
        AvaloniaProperty.Register<Anchor, string?>(nameof(TargetId));

    public string? TargetId
    {
        get => GetValue(TargetIdProperty);
        set => SetValue(TargetIdProperty, value);
    }
}
