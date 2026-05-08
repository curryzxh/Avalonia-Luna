using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

namespace Luna.Desktop.Controls;

public class Divider : ContentControl
{
    public static readonly StyledProperty<Orientation> OrientationProperty =
        AvaloniaProperty.Register<Divider, Orientation>(nameof(Orientation), defaultValue: Orientation.Horizontal);

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Divider, string?>(nameof(Text));

    public static readonly StyledProperty<bool> IsDashedProperty =
        AvaloniaProperty.Register<Divider, bool>(nameof(IsDashed), defaultValue: false);

    public Orientation Orientation
    {
        get => GetValue(OrientationProperty);
        set => SetValue(OrientationProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsDashed
    {
        get => GetValue(IsDashedProperty);
        set => SetValue(IsDashedProperty, value);
    }

    static Divider()
    {
        OrientationProperty.Changed.AddClassHandler<Divider>((x, _) => x.OnOrientationChanged());
        IsDashedProperty.Changed.AddClassHandler<Divider>((x, _) => x.UpdatePseudoClasses());
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        UpdatePseudoClasses();
    }

    private void OnOrientationChanged()
    {
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":vertical", Orientation == Orientation.Vertical);
        PseudoClasses.Set(":horizontal", Orientation == Orientation.Horizontal);
        PseudoClasses.Set(":dashed", IsDashed);
    }
}
