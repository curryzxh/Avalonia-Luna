using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 抽屉出现的位置。
/// </summary>
public enum DrawerPlacement
{
    Left,
    Right,
}

/// <summary>
/// 抽屉显示参数。
/// </summary>
public sealed record DrawerOptions
{
    /// <summary>
    /// 获取抽屉内容。
    /// </summary>
    public object? Content { get; init; }

    /// <summary>
    /// 获取标题文本。
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 获取抽屉宽度。
    /// </summary>
    public double Width { get; init; } = 280;

    /// <summary>
    /// 获取抽屉出现的位置。
    /// </summary>
    public DrawerPlacement Placement { get; init; } = DrawerPlacement.Right;

    /// <summary>
    /// 获取是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay { get; init; } = true;

    /// <summary>
    /// 获取是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick { get; init; } = true;

    /// <summary>
    /// 获取是否显示关闭按钮。
    /// </summary>
    public bool ShowCloseButton { get; init; } = true;
}

/// <summary>
/// 抽屉的静态入口，依赖页面中的 <see cref="DrawerHost"/>。
/// </summary>
public static class Drawer
{
    public static void Show(DrawerOptions options) => DrawerHost.Current?.Show(options);

    public static void Close() => DrawerHost.Current?.Close();
}
