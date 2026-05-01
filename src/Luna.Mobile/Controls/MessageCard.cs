using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 单条 Message 卡片控件，用于展示主题图标、文本、可选链接与关闭按钮。
/// </summary>
[TemplatePart(CloseButtonPartName, typeof(Button))]
public sealed class MessageCard : TemplatedControl
{
    private const string CloseButtonPartName = "PART_CloseButton";

    private Button? _closeButton;

    /// <inheritdoc cref="Content" />
    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<MessageCard, string?>(nameof(Content));

    /// <inheritdoc cref="Theme" />
    public static readonly new StyledProperty<MessageTheme> ThemeProperty =
        AvaloniaProperty.Register<MessageCard, MessageTheme>(nameof(Theme), MessageTheme.Info);

    /// <inheritdoc cref="ShowIcon" />
    public static readonly StyledProperty<bool> ShowIconProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(ShowIcon), true);

    /// <inheritdoc cref="CloseBtn" />
    public static readonly StyledProperty<bool> CloseBtnProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(CloseBtn));

    /// <inheritdoc cref="Link" />
    public static readonly StyledProperty<string?> LinkProperty =
        AvaloniaProperty.Register<MessageCard, string?>(nameof(Link));

    /// <inheritdoc cref="HasLink" />
    public static readonly DirectProperty<MessageCard, bool> HasLinkProperty =
        AvaloniaProperty.RegisterDirect<MessageCard, bool>(
            nameof(HasLink),
            o => o.HasLink);

    /// <inheritdoc cref="Marquee" />
    public static readonly StyledProperty<bool> MarqueeProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(Marquee));

    /// <inheritdoc cref="IconGlyph" />
    public static readonly DirectProperty<MessageCard, string?> IconGlyphProperty =
        AvaloniaProperty.RegisterDirect<MessageCard, string?>(
            nameof(IconGlyph),
            o => o.IconGlyph);

    private string? _iconGlyph;
    private bool _hasLink;

    static MessageCard()
    {
        ThemeProperty.Changed.AddClassHandler<MessageCard>((control, args) =>
        {
            var theme = args.GetNewValue<MessageTheme>();
            control.PseudoClasses.Set(":info", theme == MessageTheme.Info);
            control.PseudoClasses.Set(":success", theme == MessageTheme.Success);
            control.PseudoClasses.Set(":warning", theme == MessageTheme.Warning);
            control.PseudoClasses.Set(":error", theme == MessageTheme.Error);
            control.UpdateIconGlyph(theme);
        });

        MarqueeProperty.Changed.AddClassHandler<MessageCard>((control, args) =>
        {
            control.PseudoClasses.Set(":marquee", args.GetNewValue<bool>());
        });

        LinkProperty.Changed.AddClassHandler<MessageCard>((control, args) =>
        {
            control.HasLink = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });
    }

    /// <summary>
    /// 点击关闭按钮后触发。
    /// </summary>
    public event EventHandler? CloseRequested;

    /// <summary>
    /// 获取或设置消息文本。
    /// </summary>
    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    /// <summary>
    /// 获取或设置消息主题。
    /// </summary>
    public new MessageTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示图标。
    /// </summary>
    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示关闭按钮。
    /// </summary>
    public bool CloseBtn
    {
        get => GetValue(CloseBtnProperty);
        set => SetValue(CloseBtnProperty, value);
    }

    /// <summary>
    /// 获取或设置附加链接文案。
    /// </summary>
    public string? Link
    {
        get => GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    /// <summary>
    /// 获取当前是否存在链接文案。
    /// </summary>
    public bool HasLink
    {
        get => _hasLink;
        private set => SetAndRaise(HasLinkProperty, ref _hasLink, value);
    }

    /// <summary>
    /// 获取或设置是否启用跑马灯展示。
    /// </summary>
    public bool Marquee
    {
        get => GetValue(MarqueeProperty);
        set => SetValue(MarqueeProperty, value);
    }

    /// <summary>
    /// 获取当前主题对应的图标字符。
    /// </summary>
    public string? IconGlyph
    {
        get => _iconGlyph;
        private set => SetAndRaise(IconGlyphProperty, ref _iconGlyph, value);
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

    private void OnCloseButtonClick(object? sender, RoutedEventArgs e)
    {
        CloseRequested?.Invoke(this, EventArgs.Empty);
    }

    private void UpdateIconGlyph(MessageTheme theme)
    {
        IconGlyph = theme switch
        {
            MessageTheme.Success => "✓",
            MessageTheme.Warning => "!",
            MessageTheme.Error => "×",
            _ => "i",
        };
    }
}
