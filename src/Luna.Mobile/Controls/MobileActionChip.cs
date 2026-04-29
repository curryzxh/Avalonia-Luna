using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Mobile.Controls;

public class MobileActionChip : TemplatedControl
{
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<MobileActionChip, string?>(nameof(Text));

    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<MobileActionChip, bool>(nameof(IsActive));

    static MobileActionChip()
    {
        IsActiveProperty.Changed.AddClassHandler<MobileActionChip>((control, args) =>
        {
            control.PseudoClasses.Set(":active", args.GetNewValue<bool>());
        });
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
}
