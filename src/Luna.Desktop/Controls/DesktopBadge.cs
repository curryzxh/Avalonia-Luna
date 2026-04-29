using Avalonia;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class DesktopBadge : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<DesktopBadge, string?>(nameof(Text));

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }
}
