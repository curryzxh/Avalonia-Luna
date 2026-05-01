using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Mobile.Controls;

/// <summary>
/// 移动端 ActionChip 控件，支持选中态与文本展示。
/// </summary>
public class MobileActionChip : TemplatedControl
{
    /// <inheritdoc cref="Text" />
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<MobileActionChip, string?>(nameof(Text));

    /// <inheritdoc cref="IsActive" />
    public static readonly StyledProperty<bool> IsActiveProperty =
        AvaloniaProperty.Register<MobileActionChip, bool>(nameof(IsActive));

    static MobileActionChip()
    {
        IsActiveProperty.Changed.AddClassHandler<MobileActionChip>((control, args) =>
        {
            control.PseudoClasses.Set(":active", args.GetNewValue<bool>());
        });
    }

    /// <summary>
    /// 获取或设置芯片文本。
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 获取或设置是否处于激活状态。
    /// </summary>
    public bool IsActive
    {
        get => GetValue(IsActiveProperty);
        set => SetValue(IsActiveProperty, value);
    }
}
