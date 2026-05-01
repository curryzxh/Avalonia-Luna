using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// Message 全局提示的主题类型。
/// </summary>
public enum MessageTheme
{
    /// <summary>
    /// 信息提示。
    /// </summary>
    Info,

    /// <summary>
    /// 成功提示。
    /// </summary>
    Success,

    /// <summary>
    /// 警告提示。
    /// </summary>
    Warning,

    /// <summary>
    /// 错误提示。
    /// </summary>
    Error,
}

/// <summary>
/// Message 显示参数。
/// </summary>
public sealed record MessageOptions
{
    /// <summary>
    /// 获取提示内容。
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// 获取主题类型。
    /// </summary>
    public MessageTheme Theme { get; init; } = MessageTheme.Info;

    /// <summary>
    /// 获取持续时间；为 <see cref="TimeSpan.Zero"/> 表示不自动关闭。
    /// </summary>
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// 获取是否显示图标。
    /// </summary>
    public bool ShowIcon { get; init; } = true;

    /// <summary>
    /// 获取是否显示关闭按钮。
    /// </summary>
    public bool CloseBtn { get; init; }

    /// <summary>
    /// 获取可选链接文案。
    /// </summary>
    public string? Link { get; init; }

    /// <summary>
    /// 获取是否启用跑马灯效果。
    /// </summary>
    public bool Marquee { get; init; }
}

/// <summary>
/// Message 的静态入口，依赖页面中的 <see cref="MessageHost"/>。
/// </summary>
public static class Message
{
    /// <summary>
    /// 显示信息类型的消息提示。
    /// </summary>
    /// <param name="options">消息配置参数。</param>
    public static void Info(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Info });

    /// <summary>
    /// 显示成功类型的消息提示。
    /// </summary>
    /// <param name="options">消息配置参数。</param>
    public static void Success(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Success });

    /// <summary>
    /// 显示警告类型的消息提示。
    /// </summary>
    /// <param name="options">消息配置参数。</param>
    public static void Warning(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Warning });

    /// <summary>
    /// 显示错误类型的消息提示。
    /// </summary>
    /// <param name="options">消息配置参数。</param>
    public static void Error(MessageOptions options) => MessageHost.Current?.Show(options with { Theme = MessageTheme.Error });

    /// <summary>
    /// 关闭当前页面上的全部消息提示。
    /// </summary>
    public static void CloseAll() => MessageHost.Current?.CloseAll();
}
