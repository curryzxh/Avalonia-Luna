using System;

namespace Luna.Mobile.Controls;

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
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(5);
    public bool ShowIcon { get; init; } = true;
    public bool CloseBtn { get; init; }
    public string? Link { get; init; }
    public bool Marquee { get; init; }
}

public static class Message
{
    public static void Info(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Info });

    public static void Success(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Success });

    public static void Warning(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Warning });

    public static void Error(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Error });

    public static void CloseAll() => MessageHost.Current?.CloseAll();
}
