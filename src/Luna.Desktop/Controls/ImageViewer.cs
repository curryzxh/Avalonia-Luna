using Avalonia;
using Avalonia.Controls;

namespace Luna.Desktop.Controls;

public class ImageViewer : ContentControl
{
    public static readonly StyledProperty<string?> SourceProperty =
        AvaloniaProperty.Register<ImageViewer, string?>(nameof(Source));

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MinZoom), 0.1);

    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MaxZoom), 5.0);

    public static readonly StyledProperty<double> RotateProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(Rotate), 0);

    public static readonly StyledProperty<bool> ShowToolbarProperty =
        AvaloniaProperty.Register<ImageViewer, bool>(nameof(ShowToolbar), true);

    public string? Source
    {
        get => GetValue(SourceProperty);
        set => SetValue(SourceProperty, value);
    }

    public double Zoom
    {
        get => GetValue(ZoomProperty);
        set => SetValue(ZoomProperty, value);
    }

    public double MinZoom
    {
        get => GetValue(MinZoomProperty);
        set => SetValue(MinZoomProperty, value);
    }

    public double MaxZoom
    {
        get => GetValue(MaxZoomProperty);
        set => SetValue(MaxZoomProperty, value);
    }

    public double Rotate
    {
        get => GetValue(RotateProperty);
        set => SetValue(RotateProperty, value);
    }

    public bool ShowToolbar
    {
        get => GetValue(ShowToolbarProperty);
        set => SetValue(ShowToolbarProperty, value);
    }

    public void ZoomIn()
    {
        var next = Zoom + 0.25;
        if (next <= MaxZoom) Zoom = next;
    }

    public void ZoomOut()
    {
        var next = Zoom - 0.25;
        if (next >= MinZoom) Zoom = next;
    }

    public void Reset()
    {
        Zoom = 1.0;
        Rotate = 0;
    }

    public void RotateLeft()
    {
        Rotate -= 90;
    }

    public void RotateRight()
    {
        Rotate += 90;
    }
}
