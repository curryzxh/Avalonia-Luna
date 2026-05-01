using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Mobile.Controls;

/// <summary>
/// 分割线方向。
/// </summary>
public enum DividerLayout
{
    /// <summary>
    /// Horizontal。
    /// </summary>
    Horizontal,
    /// <summary>
    /// Vertical。
    /// </summary>
    Vertical,
}

/// <summary>
/// 分割线内容对齐方式（仅横向分割线有效）。
/// </summary>
public enum DividerAlign
{
    /// <summary>
    /// Center。
    /// </summary>
    Center,
    /// <summary>
    /// Left。
    /// </summary>
    Left,
    /// <summary>
    /// Right。
    /// </summary>
    Right,
}

/// <summary>
/// 分割线控件，支持横向/纵向、虚线以及带文字内容。
/// </summary>
public sealed class Divider : TemplatedControl
{
    /// <inheritdoc cref="Layout" />
    public static readonly StyledProperty<DividerLayout> LayoutProperty =
        AvaloniaProperty.Register<Divider, DividerLayout>(nameof(Layout), DividerLayout.Horizontal);

    /// <inheritdoc cref="Align" />
    public static readonly StyledProperty<DividerAlign> AlignProperty =
        AvaloniaProperty.Register<Divider, DividerAlign>(nameof(Align), DividerAlign.Center);

    /// <inheritdoc cref="Dashed" />
    public static readonly StyledProperty<bool> DashedProperty =
        AvaloniaProperty.Register<Divider, bool>(nameof(Dashed));

    /// <inheritdoc cref="Content" />
    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<Divider, string?>(nameof(Content));

    /// <inheritdoc cref="LineThickness" />
    public static readonly StyledProperty<double> LineThicknessProperty =
        AvaloniaProperty.Register<Divider, double>(nameof(LineThickness), 1);

    /// <inheritdoc cref="LineLength" />
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

    /// <summary>
    /// 初始化 <see cref="Divider" /> 的新实例。
    /// </summary>
    public Divider()
    {
        UpdateContentPseudo(Content);
        UpdateLayoutPseudo(Layout);
        UpdateAlignPseudo(Align);
        PseudoClasses.Set(":dashed", Dashed);
    }

    /// <summary>
    /// 获取或设置分割线布局方向。
    /// </summary>
    public DividerLayout Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    /// <summary>
    /// 获取或设置内容对齐方式。
    /// </summary>
    public DividerAlign Align
    {
        get => GetValue(AlignProperty);
        set => SetValue(AlignProperty, value);
    }

    /// <summary>
    /// 获取或设置是否使用虚线样式。
    /// </summary>
    public bool Dashed
    {
        get => GetValue(DashedProperty);
        set => SetValue(DashedProperty, value);
    }

    /// <summary>
    /// 获取或设置分割线中间的文本内容。
    /// </summary>
    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// 获取或设置线条粗细。
    /// </summary>
    public double LineThickness
    {
        get => GetValue(LineThicknessProperty);
        set => SetValue(LineThicknessProperty, value);
    }

    /// <summary>
    /// 获取或设置纵向线段长度或横向内容间距参考值。
    /// </summary>
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
