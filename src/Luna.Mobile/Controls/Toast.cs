using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// Toast 的主题类型。
/// </summary>
public enum ToastTheme
{
    Default,
    Success,
    Warning,
    Error,
    Loading,
}

/// <summary>
/// Toast 内容布局方向。
/// </summary>
public enum ToastDirection
{
    Row,
    Column,
}

/// <summary>
/// Toast 在屏幕中的垂直位置。
/// </summary>
public enum ToastPlacement
{
    Top,
    Middle,
    Bottom,
}

/// <summary>
/// Toast 显示参数。
/// </summary>
public sealed class ToastOptions
{
    /// <summary>
    /// 获取要显示的消息文本。
    /// </summary>
    public required string Message { get; init; }

    /// <summary>
    /// 获取主题类型。
    /// </summary>
    public ToastTheme Theme { get; init; } = ToastTheme.Default;

    /// <summary>
    /// 获取内容布局方向。
    /// </summary>
    public ToastDirection Direction { get; init; } = ToastDirection.Row;

    /// <summary>
    /// 获取垂直位置。
    /// </summary>
    public ToastPlacement Placement { get; init; } = ToastPlacement.Middle;

    /// <summary>
    /// 获取持续时间；为 <see cref="TimeSpan.Zero"/> 表示不自动关闭。
    /// </summary>
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(2);

    /// <summary>
    /// 获取是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay { get; init; }

    /// <summary>
    /// 获取是否显示关闭按钮。
    /// </summary>
    public bool ShowClose { get; init; }
}

/// <summary>
/// Toast 的静态入口，依赖页面中的 <see cref="ToastHost"/>。
/// </summary>
public static class Toast
{
    /// <summary>
    /// 使用默认参数显示 Toast。
    /// </summary>
    public static void Show(string message)
    {
        ToastHost.Current?.Show(new ToastOptions { Message = message });
    }

    /// <summary>
    /// 使用自定义参数显示 Toast。
    /// </summary>
    public static void Show(ToastOptions options)
    {
        ToastHost.Current?.Show(options);
    }

    /// <summary>
    /// 清除/关闭当前 Toast。
    /// </summary>
    public static void Clear()
    {
        ToastHost.Current?.Clear();
    }
}
