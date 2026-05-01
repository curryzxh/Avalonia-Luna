using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// Picker 的关闭原因。
/// </summary>
public enum PickerCloseReason
{
    /// <summary>
    /// 未知原因关闭。
    /// </summary>
    Unknown,

    /// <summary>
    /// 点击遮罩关闭。
    /// </summary>
    Overlay,

    /// <summary>
    /// 点击取消关闭。
    /// </summary>
    Cancel,

    /// <summary>
    /// 点击确认关闭。
    /// </summary>
    Confirm,

    /// <summary>
    /// 通过代码主动关闭。
    /// </summary>
    Programmatic,
}

/// <summary>
/// Picker 的单列数据定义。
/// </summary>
public sealed class PickerColumn
{
    /// <summary>
    /// 获取列选项列表。
    /// </summary>
    public IReadOnlyList<string> Items { get; init; } = Array.Empty<string>();

    /// <summary>
    /// 获取或设置当前选中索引。
    /// </summary>
    public int SelectedIndex { get; set; }

    /// <summary>
    /// 获取当前选中值；索引越界时返回 null。
    /// </summary>
    public string? SelectedValue
    {
        get
        {
            if (SelectedIndex < 0 || SelectedIndex >= Items.Count)
            {
                return null;
            }

            return Items[SelectedIndex];
        }
    }
}

/// <summary>
/// Picker 显示参数。
/// </summary>
public sealed class PickerOptions
{
    /// <summary>
    /// 获取列集合。
    /// </summary>
    public IReadOnlyList<PickerColumn> Columns { get; init; } = Array.Empty<PickerColumn>();

    /// <summary>
    /// 获取标题文本。
    /// </summary>
    public string? Title { get; init; }

    /// <summary>
    /// 获取取消按钮文本。
    /// </summary>
    public string CancelText { get; init; } = "取消";

    /// <summary>
    /// 获取确认按钮文本。
    /// </summary>
    public string ConfirmText { get; init; } = "确认";

    /// <summary>
    /// 获取是否允许点击遮罩关闭。
    /// </summary>
    public bool CloseOnOverlayClick { get; init; } = true;

    /// <summary>
    /// 获取面板高度。
    /// </summary>
    public double SheetHeight { get; init; } = 320;
}

/// <summary>
/// Picker 确认事件参数。
/// </summary>
public sealed class PickerConfirmedEventArgs : EventArgs
{
    /// <summary>
    /// 使用确认结果初始化事件参数。
    /// </summary>
    /// <param name="indices">各列选中索引。</param>
    /// <param name="values">各列选中值。</param>
    public PickerConfirmedEventArgs(IReadOnlyList<int> indices, IReadOnlyList<string?> values)
    {
        Indices = indices;
        Values = values;
    }

    /// <summary>
    /// 获取各列最终选中的索引集合。
    /// </summary>
    public IReadOnlyList<int> Indices { get; }

    /// <summary>
    /// 获取各列最终选中的值集合。
    /// </summary>
    public IReadOnlyList<string?> Values { get; }
}

/// <summary>
/// Picker 关闭事件参数。
/// </summary>
public sealed class PickerClosedEventArgs : EventArgs
{
    /// <summary>
    /// 使用关闭原因初始化事件参数。
    /// </summary>
    /// <param name="reason">关闭原因。</param>
    public PickerClosedEventArgs(PickerCloseReason reason)
    {
        Reason = reason;
    }

    /// <summary>
    /// 获取关闭原因。
    /// </summary>
    public PickerCloseReason Reason { get; }
}

/// <summary>
/// Picker 的静态入口，依赖页面中的 <see cref="PickerHost"/>。
/// </summary>
public static class Picker
{
    /// <summary>
    /// 使用指定参数显示选择器。
    /// </summary>
    /// <param name="options">选择器配置参数。</param>
    public static void Show(PickerOptions options) => PickerHost.Current?.Show(options);

    /// <summary>
    /// 以编程方式关闭当前选择器。
    /// </summary>
    public static void Close() => PickerHost.Current?.Close(PickerCloseReason.Programmatic);
}
