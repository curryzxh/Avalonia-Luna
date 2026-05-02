using Avalonia;
using Avalonia.Collections;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;
using System.Collections.Specialized;

namespace Luna.Mobile.Controls;

/// <summary>
/// 页脚链接项。
/// </summary>
public sealed class FooterLink
{
    /// <summary>
    /// 获取或设置链接名称。
    /// </summary>
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// 获取或设置链接地址。
    /// </summary>
    public string? Url { get; init; }

    /// <summary>
    /// 获取或设置跳转目标。
    /// </summary>
    public string? Target { get; init; }

    /// <inheritdoc />
    public override string ToString()
    {
        return Name;
    }
}

/// <summary>
/// 页脚控件，用于展示品牌信息、快捷链接与版权说明。
/// </summary>
public sealed class Footer : TemplatedControl
{
    private bool _hasLogo;
    private bool _hasLogoTitle;
    private bool _hasLinks;
    private bool _hasText;

    /// <inheritdoc cref="LogoIcon" />
    public static readonly StyledProperty<object?> LogoIconProperty =
        AvaloniaProperty.Register<Footer, object?>(nameof(LogoIcon));

    /// <inheritdoc cref="LogoTitle" />
    public static readonly StyledProperty<string?> LogoTitleProperty =
        AvaloniaProperty.Register<Footer, string?>(nameof(LogoTitle));

    /// <inheritdoc cref="Text" />
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Footer, string?>(nameof(Text));

    /// <inheritdoc cref="HasLogo" />
    public static readonly DirectProperty<Footer, bool> HasLogoProperty =
        AvaloniaProperty.RegisterDirect<Footer, bool>(
            nameof(HasLogo),
            o => o.HasLogo);

    /// <inheritdoc cref="HasLogoTitle" />
    public static readonly DirectProperty<Footer, bool> HasLogoTitleProperty =
        AvaloniaProperty.RegisterDirect<Footer, bool>(
            nameof(HasLogoTitle),
            o => o.HasLogoTitle);

    /// <inheritdoc cref="HasLinks" />
    public static readonly DirectProperty<Footer, bool> HasLinksProperty =
        AvaloniaProperty.RegisterDirect<Footer, bool>(
            nameof(HasLinks),
            o => o.HasLinks);

    /// <inheritdoc cref="HasText" />
    public static readonly DirectProperty<Footer, bool> HasTextProperty =
        AvaloniaProperty.RegisterDirect<Footer, bool>(
            nameof(HasText),
            o => o.HasText);

    static Footer()
    {
        LogoIconProperty.Changed.AddClassHandler<Footer>((control, _) => control.UpdateFlags());
        LogoTitleProperty.Changed.AddClassHandler<Footer>((control, _) => control.UpdateFlags());
        TextProperty.Changed.AddClassHandler<Footer>((control, _) => control.UpdateFlags());
    }

    /// <summary>
    /// 初始化页脚控件。
    /// </summary>
    public Footer()
    {
        Links.CollectionChanged += OnLinksCollectionChanged;
    }

    /// <summary>
    /// 获取或设置品牌图标内容。
    /// </summary>
    public object? LogoIcon
    {
        get => GetValue(LogoIconProperty);
        set => SetValue(LogoIconProperty, value);
    }

    /// <summary>
    /// 获取或设置品牌标题。
    /// </summary>
    public string? LogoTitle
    {
        get => GetValue(LogoTitleProperty);
        set => SetValue(LogoTitleProperty, value);
    }

    /// <summary>
    /// 获取或设置链接集合。
    /// </summary>
    public AvaloniaList<FooterLink> Links { get; } = [];

    /// <summary>
    /// 获取或设置版权说明文本。
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 获取当前是否显示品牌图标。
    /// </summary>
    public bool HasLogo
    {
        get => _hasLogo;
        private set => SetAndRaise(HasLogoProperty, ref _hasLogo, value);
    }

    /// <summary>
    /// 获取当前是否显示品牌标题。
    /// </summary>
    public bool HasLogoTitle
    {
        get => _hasLogoTitle;
        private set => SetAndRaise(HasLogoTitleProperty, ref _hasLogoTitle, value);
    }

    /// <summary>
    /// 获取当前是否存在链接。
    /// </summary>
    public bool HasLinks
    {
        get => _hasLinks;
        private set => SetAndRaise(HasLinksProperty, ref _hasLinks, value);
    }

    /// <summary>
    /// 获取当前是否显示版权文本。
    /// </summary>
    public bool HasText
    {
        get => _hasText;
        private set => SetAndRaise(HasTextProperty, ref _hasText, value);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateFlags();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
    }

    private void OnLinksCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        UpdateFlags();
    }

    private void UpdateFlags()
    {
        HasLogo = LogoIcon is not null;
        HasLogoTitle = !string.IsNullOrWhiteSpace(LogoTitle);
        HasLinks = Links.Count > 0;
        HasText = !string.IsNullOrWhiteSpace(Text);

        PseudoClasses.Set(":has-logo", HasLogo);
        PseudoClasses.Set(":has-logo-title", HasLogoTitle);
        PseudoClasses.Set(":has-links", HasLinks);
        PseudoClasses.Set(":has-text", HasText);
    }
}
