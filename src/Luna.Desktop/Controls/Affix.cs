using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class Affix : ContentControl
{
    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<Affix, double>(nameof(Offset), 0);

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }
}
