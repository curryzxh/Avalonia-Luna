using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class StickyTool : ContentControl
{
    public static readonly StyledProperty<double> OffsetProperty =
        AvaloniaProperty.Register<StickyTool, double>(nameof(Offset), 100);

    public static readonly StyledProperty<ControlsPosition> PositionProperty =
        AvaloniaProperty.Register<StickyTool, ControlsPosition>(nameof(Position), ControlsPosition.Right);

    public double Offset
    {
        get => GetValue(OffsetProperty);
        set => SetValue(OffsetProperty, value);
    }

    public ControlsPosition Position
    {
        get => GetValue(PositionProperty);
        set => SetValue(PositionProperty, value);
    }
}

public enum ControlsPosition
{
    Right,
    Left,
}
