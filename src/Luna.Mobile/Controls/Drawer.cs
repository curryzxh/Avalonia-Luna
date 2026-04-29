using System;

namespace Luna.Mobile.Controls;

public enum DrawerPlacement
{
    Left,
    Right,
}

public sealed record DrawerOptions
{
    public object? Content { get; init; }
    public string? Title { get; init; }
    public double Width { get; init; } = 280;
    public DrawerPlacement Placement { get; init; } = DrawerPlacement.Right;
    public bool ShowOverlay { get; init; } = true;
    public bool CloseOnOverlayClick { get; init; } = true;
    public bool ShowCloseButton { get; init; } = true;
}

public static class Drawer
{
    public static void Show(DrawerOptions options) => DrawerHost.Current?.Show(options);

    public static void Close() => DrawerHost.Current?.Close();
}
