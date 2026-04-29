using System;
using System.Collections.Generic;

namespace Luna.Mobile.Controls;

public enum PickerCloseReason
{
    Unknown,
    Overlay,
    Cancel,
    Confirm,
    Programmatic,
}

public sealed class PickerColumn
{
    public IReadOnlyList<string> Items { get; init; } = Array.Empty<string>();
    public int SelectedIndex { get; set; }

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

public sealed class PickerOptions
{
    public IReadOnlyList<PickerColumn> Columns { get; init; } = Array.Empty<PickerColumn>();
    public string? Title { get; init; }
    public string CancelText { get; init; } = "取消";
    public string ConfirmText { get; init; } = "确认";
    public bool CloseOnOverlayClick { get; init; } = true;
    public double SheetHeight { get; init; } = 320;
}

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

public sealed class PickerClosedEventArgs : EventArgs
{
    public PickerClosedEventArgs(PickerCloseReason reason)
    {
        Reason = reason;
    }

    public PickerCloseReason Reason { get; }
}

public static class Picker
{
    public static void Show(PickerOptions options) => PickerHost.Current?.Show(options);

    public static void Close() => PickerHost.Current?.Close(PickerCloseReason.Programmatic);
}

