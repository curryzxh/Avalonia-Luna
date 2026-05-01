using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 标签主题类型。
/// </summary>
public enum TagTheme
{
    /// <summary>
    /// Default。
    /// </summary>
    Default,
    /// <summary>
    /// Primary。
    /// </summary>
    Primary,
    /// <summary>
    /// Warning。
    /// </summary>
    Warning,
    /// <summary>
    /// Danger。
    /// </summary>
    Danger,
    /// <summary>
    /// Success。
    /// </summary>
    Success,
}

/// <summary>
/// 标签风格变体。
/// </summary>
public enum TagVariant
{
    /// <summary>
    /// Light。
    /// </summary>
    Light,
    /// <summary>
    /// Dark。
    /// </summary>
    Dark,
    /// <summary>
    /// Outline。
    /// </summary>
    Outline,
    /// <summary>
    /// LightOutline。
    /// </summary>
    LightOutline,
}

/// <summary>
/// 标签形状。
/// </summary>
public enum TagShape
{
    /// <summary>
    /// Square。
    /// </summary>
    Square,
    /// <summary>
    /// Round。
    /// </summary>
    Round,
    /// <summary>
    /// Mark。
    /// </summary>
    Mark,
}

/// <summary>
/// 标签尺寸预设。
/// </summary>
public enum TagSize
{
    /// <summary>
    /// Small。
    /// </summary>
    Small,
    /// <summary>
    /// Medium。
    /// </summary>
    Medium,
    /// <summary>
    /// Large。
    /// </summary>
    Large,
    /// <summary>
    /// ExtraLarge。
    /// </summary>
    ExtraLarge,
}

/// <summary>
/// 标签控件，用于展示短文本/状态，可选图标与关闭按钮。
/// </summary>
[TemplatePart(CloseButtonPartName, typeof(Button))]
public sealed class Tag : ContentControl
{
    private const string CloseButtonPartName = "PART_CloseButton";

    private bool _isIconVisible;
    private Button? _closeButton;

    /// <inheritdoc cref="Theme" />
    public static readonly StyledProperty<TagTheme> ThemeProperty =
        AvaloniaProperty.Register<Tag, TagTheme>(nameof(Theme), TagTheme.Default);

    /// <inheritdoc cref="Variant" />
    public static readonly StyledProperty<TagVariant> VariantProperty =
        AvaloniaProperty.Register<Tag, TagVariant>(nameof(Variant), TagVariant.Light);

    /// <inheritdoc cref="Shape" />
    public static readonly StyledProperty<TagShape> ShapeProperty =
        AvaloniaProperty.Register<Tag, TagShape>(nameof(Shape), TagShape.Square);

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<TagSize> SizeProperty =
        AvaloniaProperty.Register<Tag, TagSize>(nameof(Size), TagSize.Medium);

    /// <inheritdoc cref="IsClosable" />
    public static readonly StyledProperty<bool> IsClosableProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(IsClosable));

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(IsOpen), true);

    /// <inheritdoc cref="Icon" />
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Tag, object?>(nameof(Icon));

    /// <inheritdoc cref="IsIconVisible" />
    public static readonly DirectProperty<Tag, bool> IsIconVisibleProperty =
        AvaloniaProperty.RegisterDirect<Tag, bool>(
            nameof(IsIconVisible),
            o => o.IsIconVisible);

    static Tag()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Tag>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<Tag>(Avalonia.Layout.HorizontalAlignment.Left);

        ThemeProperty.Changed.AddClassHandler<Tag>((o, _) => o.UpdateState());
        VariantProperty.Changed.AddClassHandler<Tag>((o, _) => o.UpdateState());
        ShapeProperty.Changed.AddClassHandler<Tag>((o, _) => o.UpdateState());
        SizeProperty.Changed.AddClassHandler<Tag>((o, _) => o.UpdateState());
        IconProperty.Changed.AddClassHandler<Tag>((o, _) => o.UpdateState());
    }

    /// <summary>
    /// 当用户点击关闭按钮时触发。
    /// </summary>
    public event EventHandler? CloseRequested;

    /// <summary>
    /// 获取或设置主题类型。
    /// </summary>
    public TagTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置风格变体。
    /// </summary>
    public TagVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>
    /// 获取或设置形状。
    /// </summary>
    public TagShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    /// <summary>
    /// 获取或设置尺寸预设。
    /// </summary>
    public TagSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示关闭按钮。
    /// </summary>
    public bool IsClosable
    {
        get => GetValue(IsClosableProperty);
        set => SetValue(IsClosableProperty, value);
    }

    /// <summary>
    /// 获取或设置当前是否打开（关闭后可用于隐藏标签）。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置图标内容。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取图标是否可见（由 <see cref="Icon"/> 自动计算）。
    /// </summary>
    public bool IsIconVisible
    {
        get => _isIconVisible;
        private set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseButtonClick;
        }

        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseButtonClick;
        }
    }

    private void OnCloseButtonClick(object? sender, EventArgs e)
    {
        IsOpen = false;
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateState()
    {
        IsIconVisible = Icon is not null;

        PseudoClasses.Set(":default", Theme == TagTheme.Default);
        PseudoClasses.Set(":primary", Theme == TagTheme.Primary);
        PseudoClasses.Set(":warning", Theme == TagTheme.Warning);
        PseudoClasses.Set(":danger", Theme == TagTheme.Danger);
        PseudoClasses.Set(":success", Theme == TagTheme.Success);

        PseudoClasses.Set(":light", Variant == TagVariant.Light);
        PseudoClasses.Set(":dark", Variant == TagVariant.Dark);
        PseudoClasses.Set(":outline", Variant == TagVariant.Outline);
        PseudoClasses.Set(":light-outline", Variant == TagVariant.LightOutline);

        PseudoClasses.Set(":square", Shape == TagShape.Square);
        PseudoClasses.Set(":round", Shape == TagShape.Round);
        PseudoClasses.Set(":mark", Shape == TagShape.Mark);

        PseudoClasses.Set(":small", Size == TagSize.Small);
        PseudoClasses.Set(":medium", Size == TagSize.Medium);
        PseudoClasses.Set(":large", Size == TagSize.Large);
        PseudoClasses.Set(":extra-large", Size == TagSize.ExtraLarge);
    }
}

/// <summary>
/// 可勾选的标签控件（基于 <see cref="ToggleButton"/>）。
/// </summary>
public sealed class CheckTag : ToggleButton
{
    private bool _isIconVisible;

    /// <inheritdoc cref="Theme" />
    public static readonly StyledProperty<TagTheme> ThemeProperty =
        AvaloniaProperty.Register<CheckTag, TagTheme>(nameof(Theme), TagTheme.Default);

    /// <inheritdoc cref="Variant" />
    public static readonly StyledProperty<TagVariant> VariantProperty =
        AvaloniaProperty.Register<CheckTag, TagVariant>(nameof(Variant), TagVariant.Light);

    /// <inheritdoc cref="Shape" />
    public static readonly StyledProperty<TagShape> ShapeProperty =
        AvaloniaProperty.Register<CheckTag, TagShape>(nameof(Shape), TagShape.Square);

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<TagSize> SizeProperty =
        AvaloniaProperty.Register<CheckTag, TagSize>(nameof(Size), TagSize.Medium);

    /// <inheritdoc cref="Icon" />
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<CheckTag, object?>(nameof(Icon));

    /// <inheritdoc cref="IsIconVisible" />
    public static readonly DirectProperty<CheckTag, bool> IsIconVisibleProperty =
        AvaloniaProperty.RegisterDirect<CheckTag, bool>(
            nameof(IsIconVisible),
            o => o.IsIconVisible);

    static CheckTag()
    {
        ClipToBoundsProperty.OverrideDefaultValue<CheckTag>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<CheckTag>(Avalonia.Layout.HorizontalAlignment.Left);

        ThemeProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
        VariantProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
        ShapeProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
        SizeProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
        IconProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
        IsCheckedProperty.Changed.AddClassHandler<CheckTag>((o, _) => o.UpdateState());
    }

    /// <summary>
    /// 获取或设置主题类型。
    /// </summary>
    public TagTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置风格变体。
    /// </summary>
    public TagVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    /// <summary>
    /// 获取或设置形状。
    /// </summary>
    public TagShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    /// <summary>
    /// 获取或设置尺寸预设。
    /// </summary>
    public TagSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置图标内容。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取图标是否可见（由 <see cref="Icon"/> 自动计算）。
    /// </summary>
    public bool IsIconVisible
    {
        get => _isIconVisible;
        private set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    private void UpdateState()
    {
        IsIconVisible = Icon is not null;

        PseudoClasses.Set(":default", Theme == TagTheme.Default);
        PseudoClasses.Set(":primary", Theme == TagTheme.Primary);
        PseudoClasses.Set(":warning", Theme == TagTheme.Warning);
        PseudoClasses.Set(":danger", Theme == TagTheme.Danger);
        PseudoClasses.Set(":success", Theme == TagTheme.Success);

        PseudoClasses.Set(":light", Variant == TagVariant.Light);
        PseudoClasses.Set(":dark", Variant == TagVariant.Dark);
        PseudoClasses.Set(":outline", Variant == TagVariant.Outline);
        PseudoClasses.Set(":light-outline", Variant == TagVariant.LightOutline);

        PseudoClasses.Set(":square", Shape == TagShape.Square);
        PseudoClasses.Set(":round", Shape == TagShape.Round);
        PseudoClasses.Set(":mark", Shape == TagShape.Mark);

        PseudoClasses.Set(":small", Size == TagSize.Small);
        PseudoClasses.Set(":medium", Size == TagSize.Medium);
        PseudoClasses.Set(":large", Size == TagSize.Large);
        PseudoClasses.Set(":extra-large", Size == TagSize.ExtraLarge);
    }
}
