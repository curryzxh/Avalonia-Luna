using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 对话框按钮布局方式。
/// </summary>
public enum DialogButtonLayout
{
    Horizontal,
    Vertical,
}

/// <summary>
/// 对话框按钮主题。
/// </summary>
public enum DialogButtonTheme
{
    Default,
    Primary,
    Danger,
}

/// <summary>
/// 对话框按钮配置。
/// </summary>
public sealed record DialogButtonOptions
{
    /// <summary>
    /// 获取按钮文本。
    /// </summary>
    public required string Content { get; init; }

    /// <summary>
    /// 获取按钮主题。
    /// </summary>
    public DialogButtonTheme Theme { get; init; } = DialogButtonTheme.Default;
}

/// <summary>
/// 对话框显示参数。
/// </summary>
public sealed record DialogOptions
{
    /// <summary>
    /// 获取标题文本。
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 获取正文文本。
    /// </summary>
    public string? Content { get; init; }

    /// <summary>
    /// 获取中间区域内容（可放自定义控件）。
    /// </summary>
    public object? Middle { get; init; }

    /// <summary>
    /// 获取顶部区域内容（可放自定义控件）。
    /// </summary>
    public object? Top { get; init; }

    /// <summary>
    /// 获取确认按钮配置。
    /// </summary>
    public DialogButtonOptions? ConfirmButton { get; init; }

    /// <summary>
    /// 获取取消按钮配置。
    /// </summary>
    public DialogButtonOptions? CancelButton { get; init; }

    /// <summary>
    /// 获取按钮布局方式。
    /// </summary>
    public DialogButtonLayout ButtonLayout { get; init; } = DialogButtonLayout.Horizontal;

    /// <summary>
    /// 获取是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick { get; init; }

    /// <summary>
    /// 获取是否显示右上角关闭按钮。
    /// </summary>
    public bool ShowCloseButton { get; init; }

    /// <summary>
    /// 获取内容区域最大高度。
    /// </summary>
    public double MaxContentHeight { get; init; } = 120;
}

/// <summary>
/// 对话框的静态入口，依赖页面中的 <see cref="DialogHost"/>。
/// </summary>
public static class Dialog
{
    public static void Show(DialogOptions options) => DialogHost.Current?.Show(options);

    public static void Close() => DialogHost.Current?.Close();
}
