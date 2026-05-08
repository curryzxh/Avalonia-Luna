using Avalonia;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public class LunaPopup : TemplatedControl
{
    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<LunaPopup, object?>(nameof(Content));

    public static readonly StyledProperty<object?> AnchorProperty =
        AvaloniaProperty.Register<LunaPopup, object?>(nameof(Anchor));

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<LunaPopup, bool>(nameof(IsOpen));

    public static readonly StyledProperty<Avalonia.Controls.PlacementMode> PlacementProperty =
        AvaloniaProperty.Register<LunaPopup, Avalonia.Controls.PlacementMode>(nameof(Placement),
            Avalonia.Controls.PlacementMode.Bottom);

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public object? Anchor
    {
        get => GetValue(AnchorProperty);
        set => SetValue(AnchorProperty, value);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public Avalonia.Controls.PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }
}
