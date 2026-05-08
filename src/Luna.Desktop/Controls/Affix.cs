using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class Affix : TemplatedControl
{
    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<Affix, double>(nameof(Offset), 0);

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }
}
