using System;

namespace Luna.Mobile.Controls;

public enum DialogButtonLayout
{
    Horizontal,
    Vertical,
}

public enum DialogButtonTheme
{
    Default,
    Primary,
    Danger,
}

public sealed record DialogButtonOptions
{
    public required string Content { get; init; }
    public DialogButtonTheme Theme { get; init; } = DialogButtonTheme.Default;
}

public sealed record DialogOptions
{
    public string? Title { get; init; }
    public string? Content { get; init; }
    public object? Middle { get; init; }
    public object? Top { get; init; }
    public DialogButtonOptions? ConfirmButton { get; init; }
    public DialogButtonOptions? CancelButton { get; init; }
    public DialogButtonLayout ButtonLayout { get; init; } = DialogButtonLayout.Horizontal;
    public bool CloseOnOverlayClick { get; init; }
    public bool ShowCloseButton { get; init; }
    public double MaxContentHeight { get; init; } = 120;
}

public static class Dialog
{
    public static void Show(DialogOptions options) => DialogHost.Current?.Show(options);

    public static void Close() => DialogHost.Current?.Close();
}
