using Avalonia;
using Avalonia.Controls;

namespace Luna.Mobile.Controls;

public sealed class Cell : Button
{
    private bool _showLeftContent;
    private bool _showDescription;
    private bool _showRightText;
    private bool _showRightContent;

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(Title));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(Description));

    public static readonly StyledProperty<string?> RightTextProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(RightText));

    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<Cell, object?>(nameof(LeftContent));

    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<Cell, object?>(nameof(RightContent));

    public static readonly StyledProperty<bool> ShowArrowProperty =
        AvaloniaProperty.Register<Cell, bool>(nameof(ShowArrow), true);

    public static readonly DirectProperty<Cell, bool> ShowLeftContentProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowLeftContent),
            o => o.ShowLeftContent);

    public static readonly DirectProperty<Cell, bool> ShowDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowDescription),
            o => o.ShowDescription);

    public static readonly DirectProperty<Cell, bool> ShowRightTextProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowRightText),
            o => o.ShowRightText);

    public static readonly DirectProperty<Cell, bool> ShowRightContentProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowRightContent),
            o => o.ShowRightContent);

    static Cell()
    {
        TitleProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
        DescriptionProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
        RightTextProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
        LeftContentProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
        RightContentProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
        ShowArrowProperty.Changed.AddClassHandler<Cell>((control, _) => control.UpdateFlags());
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public string? RightText
    {
        get => GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }

    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    public bool ShowArrow
    {
        get => GetValue(ShowArrowProperty);
        set => SetValue(ShowArrowProperty, value);
    }

    public bool ShowLeftContent
    {
        get => _showLeftContent;
        private set => SetAndRaise(ShowLeftContentProperty, ref _showLeftContent, value);
    }

    public bool ShowDescription
    {
        get => _showDescription;
        private set => SetAndRaise(ShowDescriptionProperty, ref _showDescription, value);
    }

    public bool ShowRightText
    {
        get => _showRightText;
        private set => SetAndRaise(ShowRightTextProperty, ref _showRightText, value);
    }

    public bool ShowRightContent
    {
        get => _showRightContent;
        private set => SetAndRaise(ShowRightContentProperty, ref _showRightContent, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateFlags();
    }

    private void UpdateFlags()
    {
        ShowLeftContent = LeftContent is not null;
        ShowDescription = !string.IsNullOrWhiteSpace(Description);
        ShowRightText = !string.IsNullOrWhiteSpace(RightText);
        ShowRightContent = RightContent is not null;

        PseudoClasses.Set(":has-left", ShowLeftContent);
        PseudoClasses.Set(":has-description", ShowDescription);
        PseudoClasses.Set(":has-right-text", ShowRightText);
        PseudoClasses.Set(":has-right-content", ShowRightContent);
        PseudoClasses.Set(":no-arrow", !ShowArrow);
    }
}
