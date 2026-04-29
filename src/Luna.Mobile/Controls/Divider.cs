using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Mobile.Controls;

/// <summary>
/// 分割线方向。
/// </summary>
public enum DividerLayout
{
    Horizontal,
    Vertical,
}

/// <summary>
/// 分割线内容对齐方式（仅横向分割线有效）。
/// </summary>
public enum DividerAlign
{
    Center,
    Left,
    Right,
}

/// <summary>
/// 分割线控件，支持横向/纵向、虚线以及带文字内容。
/// </summary>
public sealed class Divider : TemplatedControl
{
    public static readonly StyledProperty<DividerLayout> LayoutProperty =
        AvaloniaProperty.Register<Divider, DividerLayout>(nameof(Layout), DividerLayout.Horizontal);

    public static readonly StyledProperty<DividerAlign> AlignProperty =
        AvaloniaProperty.Register<Divider, DividerAlign>(nameof(Align), DividerAlign.Center);

    public static readonly StyledProperty<bool> DashedProperty =
        AvaloniaProperty.Register<Divider, bool>(nameof(Dashed));

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<Divider, string?>(nameof(Content));

    public static readonly StyledProperty<double> LineThicknessProperty =
        AvaloniaProperty.Register<Divider, double>(nameof(LineThickness), 1);

    public static readonly StyledProperty<double> LineLengthProperty =
        AvaloniaProperty.Register<Divider, double>(nameof(LineLength), 16);

    static Divider()
    {
        ContentProperty.Changed.AddClassHandler<Divider>((control, args) =>
        {
            control.UpdateContentPseudo(args.GetNewValue<string?>());
        });

        LayoutProperty.Changed.AddClassHandler<Divider>((control, args) =>
        {
            control.UpdateLayoutPseudo(args.GetNewValue<DividerLayout>());
        });

        AlignProperty.Changed.AddClassHandler<Divider>((control, args) =>
        {
            control.UpdateAlignPseudo(args.GetNewValue<DividerAlign>());
        });

        DashedProperty.Changed.AddClassHandler<Divider>((control, args) =>
        {
            control.PseudoClasses.Set(":dashed", args.GetNewValue<bool>());
        });
    }

    public Divider()
    {
        UpdateContentPseudo(Content);
        UpdateLayoutPseudo(Layout);
        UpdateAlignPseudo(Align);
        PseudoClasses.Set(":dashed", Dashed);
    }

    public DividerLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public DividerAlign Align
    {
        get => GetValue(AlignProperty);
        set => SetValue(AlignProperty, value);
    }

    public bool Dashed
    {
        get => GetValue(DashedProperty);
        set => SetValue(DashedProperty, value);
    }

    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public double LineThickness
    {
        get => GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    public double LineLength
    {
        get => GetValue(LineLengthProperty);
        set => SetValue(LineLengthProperty, value);
    }

    private void UpdateLayoutPseudo(DividerLayout layout)
    {
        PseudoClasses.Set(":horizontal", layout == DividerLayout.Horizontal);
        PseudoClasses.Set(":vertical", layout == DividerLayout.Vertical);
    }

    private void UpdateAlignPseudo(DividerAlign align)
    {
        PseudoClasses.Set(":align-left", align == DividerAlign.Left);
        PseudoClasses.Set(":align-center", align == DividerAlign.Center);
        PseudoClasses.Set(":align-right", align == DividerAlign.Right);
    }

    private void UpdateContentPseudo(string? content)
    {
        var hasContent = !string.IsNullOrWhiteSpace(content);
        PseudoClasses.Set(":has-content", hasContent);
        PseudoClasses.Set(":no-content", !hasContent);
    }
}
