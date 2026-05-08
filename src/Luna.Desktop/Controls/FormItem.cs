using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

[PseudoClasses(":error")]
public class FormItem : ContentControl
{
    public static readonly StyledProperty<string?> LabelProperty =
        AvaloniaProperty.Register<FormItem, string?>(nameof(Label));

    public static readonly StyledProperty<string?> ErrorMessageProperty =
        AvaloniaProperty.Register<FormItem, string?>(nameof(ErrorMessage));

    public static readonly StyledProperty<bool> IsRequiredProperty =
        AvaloniaProperty.Register<FormItem, bool>(nameof(IsRequired));

    public static readonly StyledProperty<bool> ShowColonProperty =
        AvaloniaProperty.Register<FormItem, bool>(nameof(ShowColon), true);

    public static readonly StyledProperty<double> LabelWidthProperty =
        AvaloniaProperty.Register<FormItem, double>(nameof(LabelWidth), defaultValue: 80);

    public static readonly StyledProperty<LayoutOrientation> LabelAlignProperty =
        AvaloniaProperty.Register<FormItem, LayoutOrientation>(nameof(LabelAlign), LayoutOrientation.Horizontal);

    static FormItem()
    {
        ErrorMessageProperty.Changed.AddClassHandler<FormItem>((item, args) =>
            item.PseudoClasses.Set(":error", !string.IsNullOrEmpty(args.NewValue as string)));
    }

    public string? Label
    {
        get => GetValue(LabelProperty);
        set => SetValue(LabelProperty, value);
    }

    public string? ErrorMessage
    {
        get => GetValue(ErrorMessageProperty);
        set => SetValue(ErrorMessageProperty, value);
    }

    public bool IsRequired
    {
        get => GetValue(IsRequiredProperty);
        set => SetValue(IsRequiredProperty, value);
    }

    public bool ShowColon
    {
        get => GetValue(ShowColonProperty);
        set => SetValue(ShowColonProperty, value);
    }

    public double LabelWidth
    {
        get => GetValue(LabelWidthProperty);
        set => SetValue(LabelWidthProperty, value);
    }

    public LayoutOrientation LabelAlign
    {
        get => GetValue(LabelAlignProperty);
        set => SetValue(LabelAlignProperty, value);
    }
}

public enum LayoutOrientation
{
    Horizontal,
    Vertical,
}
