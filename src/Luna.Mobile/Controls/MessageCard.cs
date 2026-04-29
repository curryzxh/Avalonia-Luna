using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Interactivity;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 单条 Message 卡片控件，用于展示主题图标、文本、可选链接与关闭按钮。
/// </summary>
public sealed class MessageCard : TemplatedControl
{
    private const string CloseButtonPartName = "PART_CloseButton";

    private Button? _closeButton;

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<MessageCard, string?>(nameof(Content));

    public static readonly new StyledProperty<MessageTheme> ThemeProperty =
        AvaloniaProperty.Register<MessageCard, MessageTheme>(nameof(Theme), MessageTheme.Info);

    public static readonly StyledProperty<bool> ShowIconProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(ShowIcon), true);

    public static readonly StyledProperty<bool> CloseBtnProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(CloseBtn));

    public static readonly StyledProperty<string?> LinkProperty =
        AvaloniaProperty.Register<MessageCard, string?>(nameof(Link));

    public static readonly DirectProperty<MessageCard, bool> HasLinkProperty =
        AvaloniaProperty.RegisterDirect<MessageCard, bool>(
            nameof(HasLink),
            o => o.HasLink);

    public static readonly StyledProperty<bool> MarqueeProperty =
        AvaloniaProperty.Register<MessageCard, bool>(nameof(Marquee));

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

    public event EventHandler? CloseRequested;

    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public new MessageTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public bool ShowIcon
    {
        get => GetValue(ShowIconProperty);
        set => SetValue(ShowIconProperty, value);
    }

    public bool CloseBtn
    {
        get => GetValue(CloseBtnProperty);
        set => SetValue(CloseBtnProperty, value);
    }

    public string? Link
    {
        get => GetValue(LinkProperty);
        set => SetValue(LinkProperty, value);
    }

    public bool HasLink
    {
        get => _hasLink;
        private set => SetAndRaise(HasLinkProperty, ref _hasLink, value);
    }

    public bool Marquee
    {
        get => GetValue(MarqueeProperty);
        set => SetValue(MarqueeProperty, value);
    }

    public string? IconGlyph
    {
        get => _iconGlyph;
        private set => SetAndRaise(IconGlyphProperty, ref _iconGlyph, value);
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
