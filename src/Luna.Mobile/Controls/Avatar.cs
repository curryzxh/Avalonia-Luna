using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

public enum AvatarShape
{
    Circle,
    Round,
}

public enum AvatarSize
{
    Small,
    Medium,
    Large,
}

public sealed class Avatar : ContentControl
{
    private bool _isImageVisible;
    private bool _isIconVisible;
    private bool _isContentVisible;
    private double _avatarLength = 40;
    private CornerRadius _avatarCornerRadius = new(999);

    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<Avatar, IImage?>(nameof(Source));

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Avatar, object?>(nameof(Icon));

    public static readonly StyledProperty<AvatarShape> ShapeProperty =
        AvaloniaProperty.Register<Avatar, AvatarShape>(nameof(Shape), AvatarShape.Circle);

    public static readonly StyledProperty<AvatarSize> SizeProperty =
        AvaloniaProperty.Register<Avatar, AvatarSize>(nameof(Size), AvatarSize.Medium);

    public static readonly DirectProperty<Avatar, bool> IsImageVisibleProperty =
        AvaloniaProperty.RegisterDirect<Avatar, bool>(
            nameof(IsImageVisible),
            o => o.IsImageVisible);

    public static readonly DirectProperty<Avatar, bool> IsIconVisibleProperty =
        AvaloniaProperty.RegisterDirect<Avatar, bool>(
            nameof(IsIconVisible),
            o => o.IsIconVisible);

    public static readonly DirectProperty<Avatar, bool> IsContentVisibleProperty =
        AvaloniaProperty.RegisterDirect<Avatar, bool>(
            nameof(IsContentVisible),
            o => o.IsContentVisible);

    public static readonly DirectProperty<Avatar, double> AvatarLengthProperty =
        AvaloniaProperty.RegisterDirect<Avatar, double>(
            nameof(AvatarLength),
            o => o.AvatarLength);

    public static readonly DirectProperty<Avatar, CornerRadius> AvatarCornerRadiusProperty =
        AvaloniaProperty.RegisterDirect<Avatar, CornerRadius>(
            nameof(AvatarCornerRadius),
            o => o.AvatarCornerRadius);

    static Avatar()
    {
        ClipToBoundsProperty.OverrideDefaultValue<Avatar>(false);
        HorizontalAlignmentProperty.OverrideDefaultValue<Avatar>(Avalonia.Layout.HorizontalAlignment.Left);

        SourceProperty.Changed.AddClassHandler<Avatar>((o, _) => o.UpdateState());
        IconProperty.Changed.AddClassHandler<Avatar>((o, _) => o.UpdateState());
        ShapeProperty.Changed.AddClassHandler<Avatar>((o, _) => o.UpdateState());
        SizeProperty.Changed.AddClassHandler<Avatar>((o, _) => o.UpdateState());
    }

    public IImage? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    public AvatarShape Shape
    {
        get => GetValue(ShapeProperty);
        set => SetValue(ShapeProperty, value);
    }

    public AvatarSize Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public bool IsImageVisible
    {
        get => _isImageVisible;
        private set => SetAndRaise(IsImageVisibleProperty, ref _isImageVisible, value);
    }

    public bool IsIconVisible
    {
        get => _isIconVisible;
        private set => SetAndRaise(IsIconVisibleProperty, ref _isIconVisible, value);
    }

    public bool IsContentVisible
    {
        get => _isContentVisible;
        private set => SetAndRaise(IsContentVisibleProperty, ref _isContentVisible, value);
    }

    public double AvatarLength
    {
        get => _avatarLength;
        private set => SetAndRaise(AvatarLengthProperty, ref _avatarLength, value);
    }

    public CornerRadius AvatarCornerRadius
    {
        get => _avatarCornerRadius;
        private set => SetAndRaise(AvatarCornerRadiusProperty, ref _avatarCornerRadius, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    private void UpdateState()
    {
        IsImageVisible = Source is not null;
        IsIconVisible = Icon is not null;
        IsContentVisible = !IsImageVisible && !IsIconVisible;

        PseudoClasses.Set(":circle", Shape == AvatarShape.Circle);
        PseudoClasses.Set(":round", Shape == AvatarShape.Round);

        PseudoClasses.Set(":small", Size == AvatarSize.Small);
        PseudoClasses.Set(":medium", Size == AvatarSize.Medium);
        PseudoClasses.Set(":large", Size == AvatarSize.Large);

        AvatarLength = Size switch
        {
            AvatarSize.Large => 56,
            AvatarSize.Small => 24,
            _ => 40,
        };

        AvatarCornerRadius = Shape switch
        {
            AvatarShape.Round => new CornerRadius(12),
            _ => new CornerRadius(999),
        };
    }
}
