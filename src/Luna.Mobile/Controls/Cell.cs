using Avalonia;
using Avalonia.Controls;

namespace Luna.Mobile.Controls;

/// <summary>
/// 单元格控件，常用于列表项展示，支持标题/描述/左右内容与右侧箭头。
/// </summary>
public sealed class Cell : Button
{
    private bool _showLeftContent;
    private bool _showDescription;
    private bool _showRightText;
    private bool _showRightContent;

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(Title));

    /// <inheritdoc cref="Description" />
    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(Description));

    /// <inheritdoc cref="RightText" />
    public static readonly StyledProperty<string?> RightTextProperty =
        AvaloniaProperty.Register<Cell, string?>(nameof(RightText));

    /// <inheritdoc cref="LeftContent" />
    public static readonly StyledProperty<object?> LeftContentProperty =
        AvaloniaProperty.Register<Cell, object?>(nameof(LeftContent));

    /// <inheritdoc cref="RightContent" />
    public static readonly StyledProperty<object?> RightContentProperty =
        AvaloniaProperty.Register<Cell, object?>(nameof(RightContent));

    /// <inheritdoc cref="ShowArrow" />
    public static readonly StyledProperty<bool> ShowArrowProperty =
        AvaloniaProperty.Register<Cell, bool>(nameof(ShowArrow), true);

    /// <inheritdoc cref="ShowLeftContent" />
    public static readonly DirectProperty<Cell, bool> ShowLeftContentProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowLeftContent),
            o => o.ShowLeftContent);

    /// <inheritdoc cref="ShowDescription" />
    public static readonly DirectProperty<Cell, bool> ShowDescriptionProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowDescription),
            o => o.ShowDescription);

    /// <inheritdoc cref="ShowRightText" />
    public static readonly DirectProperty<Cell, bool> ShowRightTextProperty =
        AvaloniaProperty.RegisterDirect<Cell, bool>(
            nameof(ShowRightText),
            o => o.ShowRightText);

    /// <inheritdoc cref="ShowRightContent" />
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

    /// <summary>
    /// 获取或设置主标题文本。
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
    /// 获取或设置右侧辅助文本。
    /// </summary>
    public string? RightText
    {
        get => GetValue(RightTextProperty);
        set => SetValue(RightTextProperty, value);
    }

    /// <summary>
    /// 获取或设置左侧自定义内容。
    /// </summary>
    public object? LeftContent
    {
        get => GetValue(LeftContentProperty);
        set => SetValue(LeftContentProperty, value);
    }

    /// <summary>
    /// 获取或设置右侧自定义内容。
    /// </summary>
    public object? RightContent
    {
        get => GetValue(RightContentProperty);
        set => SetValue(RightContentProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示右侧箭头。
    /// </summary>
    public bool ShowArrow
    {
        get => GetValue(ShowArrowProperty);
        set => SetValue(ShowArrowProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在左侧内容。
    /// </summary>
    public bool ShowLeftContent
    {
        get => _showLeftContent;
        private set => SetAndRaise(ShowLeftContentProperty, ref _showLeftContent, value);
    }

    /// <summary>
    /// 获取当前是否存在描述文本。
    /// </summary>
    public bool ShowDescription
    {
        get => _showDescription;
        private set => SetAndRaise(ShowDescriptionProperty, ref _showDescription, value);
    }

    /// <summary>
    /// 获取当前是否存在右侧文本。
    /// </summary>
    public bool ShowRightText
    {
        get => _showRightText;
        private set => SetAndRaise(ShowRightTextProperty, ref _showRightText, value);
    }

    /// <summary>
    /// 获取当前是否存在右侧自定义内容。
    /// </summary>
    public bool ShowRightContent
    {
        get => _showRightContent;
        private set => SetAndRaise(ShowRightContentProperty, ref _showRightContent, value);
    }

    /// <inheritdoc />
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
