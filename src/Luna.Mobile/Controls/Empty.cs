using Avalonia;
using Avalonia.Controls.Primitives;

namespace Luna.Mobile.Controls;

/// <summary>
/// 空状态控件，用于在无数据/无内容时展示占位图标、标题、描述与操作区域。
/// </summary>
public sealed class Empty : TemplatedControl
{
    private bool _hasIcon;
    private bool _showDefaultIcon = true;
    private bool _hasTitle;
    private bool _hasDescription;
    private bool _hasAction;

    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Empty, object?>(nameof(Icon));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Empty, string?>(nameof(Title));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Empty, string?>(nameof(Description));

    public static readonly StyledProperty<object?> ActionProperty =
        AvaloniaProperty.Register<Empty, object?>(nameof(Action));

    public static readonly DirectProperty<Empty, bool> HasIconProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasIcon),
            o => o.HasIcon);

    public static readonly DirectProperty<Empty, bool> ShowDefaultIconProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(ShowDefaultIcon),
            o => o.ShowDefaultIcon);

    public static readonly DirectProperty<Empty, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    public static readonly DirectProperty<Empty, bool> HasDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasDescription),
            o => o.HasDescription);

    public static readonly DirectProperty<Empty, bool> HasActionProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasAction),
            o => o.HasAction);

    static Empty()
    {
        IconProperty.Changed.AddClassHandler<Empty>((control, _) => control.UpdateFlags());
        TitleProperty.Changed.AddClassHandler<Empty>((control, _) => control.UpdateFlags());
        DescriptionProperty.Changed.AddClassHandler<Empty>((control, _) => control.UpdateFlags());
        ActionProperty.Changed.AddClassHandler<Empty>((control, _) => control.UpdateFlags());
    }

    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
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

    public object? Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    public bool HasIcon
    {
        get => _hasIcon;
        private set => SetAndRaise(HasIconProperty, ref _hasIcon, value);
    }

    public bool ShowDefaultIcon
    {
        get => _showDefaultIcon;
        private set => SetAndRaise(ShowDefaultIconProperty, ref _showDefaultIcon, value);
    }

    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    public bool HasDescription
    {
        get => _hasDescription;
        private set => SetAndRaise(HasDescriptionProperty, ref _hasDescription, value);
    }

    public bool HasAction
    {
        get => _hasAction;
        private set => SetAndRaise(HasActionProperty, ref _hasAction, value);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateFlags();
    }

    private void UpdateFlags()
    {
        HasIcon = Icon is not null;
        ShowDefaultIcon = !HasIcon;
        HasTitle = !string.IsNullOrWhiteSpace(Title);
        HasDescription = !string.IsNullOrWhiteSpace(Description);
        HasAction = Action is not null;
    }
}
