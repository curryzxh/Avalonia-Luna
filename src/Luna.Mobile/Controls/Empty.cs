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

    /// <inheritdoc cref="Icon" />
    public static readonly StyledProperty<object?> IconProperty =
        AvaloniaProperty.Register<Empty, object?>(nameof(Icon));

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Empty, string?>(nameof(Title));

    /// <inheritdoc cref="Description" />
    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Empty, string?>(nameof(Description));

    /// <inheritdoc cref="Action" />
    public static readonly StyledProperty<object?> ActionProperty =
        AvaloniaProperty.Register<Empty, object?>(nameof(Action));

    /// <inheritdoc cref="HasIcon" />
    public static readonly DirectProperty<Empty, bool> HasIconProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasIcon),
            o => o.HasIcon);

    /// <inheritdoc cref="ShowDefaultIcon" />
    public static readonly DirectProperty<Empty, bool> ShowDefaultIconProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(ShowDefaultIcon),
            o => o.ShowDefaultIcon);

    /// <inheritdoc cref="HasTitle" />
    public static readonly DirectProperty<Empty, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    /// <inheritdoc cref="HasDescription" />
    public static readonly DirectProperty<Empty, bool> HasDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Empty, bool>(
            nameof(HasDescription),
            o => o.HasDescription);

    /// <inheritdoc cref="HasAction" />
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

    /// <summary>
    /// 获取或设置自定义图标内容。
    /// </summary>
    public object? Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    /// <summary>
    /// 获取或设置标题文本。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 获取或设置描述文本。
    /// </summary>
    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    /// <summary>
    /// 获取或设置操作区域内容。
    /// </summary>
    public object? Action
    {
        get => GetValue(ActionProperty);
        set => SetValue(ActionProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在自定义图标。
    /// </summary>
    public bool HasIcon
    {
        get => _hasIcon;
        private set => SetAndRaise(HasIconProperty, ref _hasIcon, value);
    }

    /// <summary>
    /// 获取当前是否显示默认占位图标。
    /// </summary>
    public bool ShowDefaultIcon
    {
        get => _showDefaultIcon;
        private set => SetAndRaise(ShowDefaultIconProperty, ref _showDefaultIcon, value);
    }

    /// <summary>
    /// 获取当前是否存在标题。
    /// </summary>
    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    /// <summary>
    /// 获取当前是否存在描述。
    /// </summary>
    public bool HasDescription
    {
        get => _hasDescription;
        private set => SetAndRaise(HasDescriptionProperty, ref _hasDescription, value);
    }

    /// <summary>
    /// 获取当前是否存在操作区域内容。
    /// </summary>
    public bool HasAction
    {
        get => _hasAction;
        private set => SetAndRaise(HasActionProperty, ref _hasAction, value);
    }

    /// <inheritdoc />
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
