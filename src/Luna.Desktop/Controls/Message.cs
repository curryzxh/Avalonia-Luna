using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Threading;
using Avalonia.VisualTree;

namespace Luna.Desktop.Controls;

public enum MessageTheme
{
    Info,
    Success,
    Warning,
    Error,
}

public sealed record MessageOptions
{
    public required string Content { get; init; }
    public MessageTheme Theme { get; init; } = MessageTheme.Info;
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(3);
    public bool ShowClose { get; init; }
}

[PseudoClasses(PC_Open, PC_Info, PC_Success, PC_Warning, PC_Error)]
public sealed class MessageHost : TemplatedControl
{
    private const string PC_Open = ":open";
    private const string PC_Info = ":info";
    private const string PC_Success = ":success";
    private const string PC_Warning = ":warning";
    private const string PC_Error = ":error";

    private static MessageHost? _current;
    private readonly DispatcherTimer _timer;
    private Button? _closeButton;

    public static MessageHost? Current => _current;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<MessageHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> ContentProperty =
        AvaloniaProperty.Register<MessageHost, string?>(nameof(Content));

    public static readonly StyledProperty<MessageTheme> ThemeProperty =
        AvaloniaProperty.Register<MessageHost, MessageTheme>(nameof(Theme), MessageTheme.Info);

    public static readonly StyledProperty<bool> ShowCloseProperty =
        AvaloniaProperty.Register<MessageHost, bool>(nameof(ShowClose));

    static MessageHost()
    {
        IsOpenProperty.Changed.AddClassHandler<MessageHost>((control, _) => control.UpdateThemePseudoClasses());
        ThemeProperty.Changed.AddClassHandler<MessageHost>((control, _) => control.UpdateThemePseudoClasses());
    }

    public MessageHost()
    {
        _timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(3) };
        _timer.Tick += (_, _) => Close();
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public MessageTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public bool ShowClose
    {
        get => GetValue(ShowCloseProperty);
        set => SetValue(ShowCloseProperty, value);
    }

    public void Show(MessageOptions options)
    {
        Content = options.Content;
        Theme = options.Theme;
        ShowClose = options.ShowClose;
        IsOpen = true;
        UpdateThemePseudoClasses();

        _timer.Stop();
        if (options.Duration > TimeSpan.Zero)
        {
            _timer.Interval = options.Duration;
            _timer.Start();
        }
    }

    public void Close()
    {
        _timer.Stop();
        IsOpen = false;
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
            _current = null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_closeButton is not null)
            _closeButton.Click -= OnCloseClick;

        _closeButton = e.NameScope.Find<Button>("PART_CloseButton");
        if (_closeButton is not null)
            _closeButton.Click += OnCloseClick;
    }

    private void OnCloseClick(object? sender, EventArgs e) => Close();

    private void UpdateThemePseudoClasses()
    {
        PseudoClasses.Set(PC_Open, IsOpen);
        PseudoClasses.Set(PC_Info, Theme == MessageTheme.Info);
        PseudoClasses.Set(PC_Success, Theme == MessageTheme.Success);
        PseudoClasses.Set(PC_Warning, Theme == MessageTheme.Warning);
        PseudoClasses.Set(PC_Error, Theme == MessageTheme.Error);
    }
}

public static class LunaMessage
{
    public static void Info(string content) =>
        MessageHost.Current?.Show(new MessageOptions { Content = content, Theme = MessageTheme.Info });

    public static void Success(string content) =>
        MessageHost.Current?.Show(new MessageOptions { Content = content, Theme = MessageTheme.Success });

    public static void Warning(string content) =>
        MessageHost.Current?.Show(new MessageOptions { Content = content, Theme = MessageTheme.Warning });

    public static void Error(string content) =>
        MessageHost.Current?.Show(new MessageOptions { Content = content, Theme = MessageTheme.Error });

    public static void Show(MessageOptions options) => MessageHost.Current?.Show(options);

    public static void CloseAll() => MessageHost.Current?.Close();
}
