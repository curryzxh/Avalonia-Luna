using System;

namespace Luna.Mobile.Controls;

public enum ToastTheme
{
    Default,
    Success,
    Warning,
    Error,
    Loading,
}

public enum ToastDirection
{
    Row,
    Column,
}

public enum ToastPlacement
{
    Top,
    Middle,
    Bottom,
}

public sealed class ToastOptions
{
    public required string Message { get; init; }
    public ToastTheme Theme { get; init; } = ToastTheme.Default;
    public ToastDirection Direction { get; init; } = ToastDirection.Row;
    public ToastPlacement Placement { get; init; } = ToastPlacement.Middle;
    public TimeSpan Duration { get; init; } = TimeSpan.FromSeconds(2);
    public bool ShowOverlay { get; init; }
    public bool ShowClose { get; init; }
}

public static class Toast
{
    public static void Show(string message)
    {
        ToastHost.Current?.Show(new ToastOptions { Message = message });
    }

    public static void Show(ToastOptions options)
    {
        ToastHost.Current?.Show(options);
    }

    public static void Clear()
    {
        ToastHost.Current?.Clear();
    }
}
