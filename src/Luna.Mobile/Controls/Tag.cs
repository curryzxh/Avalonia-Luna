using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

public enum TagTheme
{
    Default,
    Primary,
    Warning,
    Danger,
    Success,
}

public enum TagVariant
{
    Light,
    Dark,
    Outline,
    LightOutline,
}

public enum TagShape
{
    Square,
    Round,
    Mark,
}

public enum TagSize
{
    Small,
    Medium,
    Large,
    ExtraLarge,
}

public sealed class Tag : ContentControl
{
    private const string CloseButtonPartName = "PART_CloseButton";

    private bool _isIconVisible;
    private Button? _closeButton;

    public static readonly StyledProperty<TagTheme> ThemeProperty =
        AvaloniaProperty.Register<Tag, TagTheme>(nameof(Theme), TagTheme.Default);

    public static readonly StyledProperty<TagVariant> VariantProperty =
        AvaloniaProperty.Register<Tag, TagVariant>(nameof(Variant), TagVariant.Light);

    public static readonly StyledProperty<TagShape> ShapeProperty =
        AvaloniaProperty.Register<Tag, TagShape>(nameof(Shape), TagShape.Square);

    public static readonly StyledProperty<TagSize> SizeProperty =
        AvaloniaProperty.Register<Tag, TagSize>(nameof(Size), TagSize.Medium);

    public static readonly StyledProperty<bool> IsClosableProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(IsClosable));

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Tag, bool>(nameof(IsOpen), true);

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Tag, object?>(nameof(Icon));

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

    public event EventHandler? CloseRequested;

    public TagTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public TagVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public TagShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public TagSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public bool IsClosable
    {
        get => GetValue(IsClosableProperty);
        set => SetValue(IsClosableProperty, value);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsIconVisible
    {
        get => _isIconVisible;
        private set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

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

public sealed class CheckTag : ToggleButton
{
    private bool _isIconVisible;

    public static readonly StyledProperty<TagTheme> ThemeProperty =
        AvaloniaProperty.Register<CheckTag, TagTheme>(nameof(Theme), TagTheme.Default);

    public static readonly StyledProperty<TagVariant> VariantProperty =
        AvaloniaProperty.Register<CheckTag, TagVariant>(nameof(Variant), TagVariant.Light);

    public static readonly StyledProperty<TagShape> ShapeProperty =
        AvaloniaProperty.Register<CheckTag, TagShape>(nameof(Shape), TagShape.Square);

    public static readonly StyledProperty<TagSize> SizeProperty =
        AvaloniaProperty.Register<CheckTag, TagSize>(nameof(Size), TagSize.Medium);

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<CheckTag, object?>(nameof(Icon));

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

    public TagTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public TagVariant Variant
    {
        get => GetValue(VariantProperty);
        set => SetValue(VariantProperty, value);
    }

    public TagShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public TagSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public bool IsIconVisible
    {
        get => _isIconVisible;
        private set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
    }

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
