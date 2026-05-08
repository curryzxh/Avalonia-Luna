using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class Anchor : TemplatedControl
{
    public static readonly StyledProperty<string?> TargetIdProperty =
        AvaloniaProperty.Register<Anchor, string?>(nameof(TargetId));

    public string? TargetId
    {
        get => GetValue(TargetIdProperty);
        set => SetValue(TargetIdProperty, value);
    }
}
