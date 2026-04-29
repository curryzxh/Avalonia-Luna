using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

/// <summary>
/// Picker 的关闭原因。
/// </summary>
public enum PickerCloseReason
{
    Unknown,
    Overlay,
    Cancel,
    Confirm,
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
    public PickerConfirmedEventArgs(IReadOnlyList<int> indices, IReadOnlyList<string?> values)
    {
        Indices = indices;
        Values = values;
    }

    public IReadOnlyList<int> Indices { get; }
    public IReadOnlyList<string?> Values { get; }
}

/// <summary>
/// Picker 关闭事件参数。
/// </summary>
public sealed class PickerClosedEventArgs : EventArgs
{
    public PickerClosedEventArgs(PickerCloseReason reason)
    {
        Reason = reason;
    }

    public PickerCloseReason Reason { get; }
}

/// <summary>
/// Picker 的静态入口，依赖页面中的 <see cref="PickerHost"/>。
/// </summary>
public static class Picker
{
    public static void Show(PickerOptions options) => PickerHost.Current?.Show(options);

    public static void Close() => PickerHost.Current?.Close(PickerCloseReason.Programmatic);
}
