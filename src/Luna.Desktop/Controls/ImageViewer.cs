using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;

namespace Luna.Desktop.Controls;

public class ImageViewer : TemplatedControl
{
    public static readonly StyledProperty<IImage?> SourceProperty =
        AvaloniaProperty.Register<ImageViewer, IImage?>(nameof(Source));

    public static readonly StyledProperty<double> ZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(Zoom), 1.0);

    public static readonly StyledProperty<double> MinZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MinZoom), 0.1);

    public static readonly StyledProperty<double> MaxZoomProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(MaxZoom), 10.0);

    public static readonly StyledProperty<double> RotateProperty =
        AvaloniaProperty.Register<ImageViewer, double>(nameof(Rotate), 0);

    public static readonly StyledProperty<bool> ShowToolbarProperty =
        AvaloniaProperty.Register<ImageViewer, bool>(nameof(ShowToolbar), true);

    private Border? _imageContainer;
    private ScaleTransform? _scaleTransform;
    private RotateTransform? _rotateTransform;
    private bool _isDragging;
    private Point _lastPosition;
    private Button? _zoomInButton;
    private Button? _zoomOutButton;
    private Button? _rotateLeftButton;
    private Button? _rotateRightButton;
    private Button? _resetButton;

    public IImage? Source
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

    static ImageViewer()
    {
        ZoomProperty.Changed.AddClassHandler<ImageViewer>(OnZoomChanged);
        RotateProperty.Changed.AddClassHandler<ImageViewer>(OnRotateChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        UnsubscribeButtons();

        _imageContainer = e.NameScope.Find<Border>("PART_ImageContainer");
        _zoomInButton = e.NameScope.Find<Button>("PART_ZoomIn");
        _zoomOutButton = e.NameScope.Find<Button>("PART_ZoomOut");
        _rotateLeftButton = e.NameScope.Find<Button>("PART_RotateLeft");
        _rotateRightButton = e.NameScope.Find<Button>("PART_RotateRight");
        _resetButton = e.NameScope.Find<Button>("PART_Reset");

        if (_imageContainer != null)
        {
            _imageContainer.RenderTransform = new TransformGroup
            {
                Children = new Transforms
                {
                    new ScaleTransform(Zoom, Zoom),
                    new RotateTransform(Rotate),
                }
            };

            _scaleTransform = (ScaleTransform)((TransformGroup)_imageContainer.RenderTransform).Children[0];
            _rotateTransform = (RotateTransform)((TransformGroup)_imageContainer.RenderTransform).Children[1];
        }

        SubscribeButtons();
    }

    private void SubscribeButtons()
    {
        if (_zoomInButton != null) _zoomInButton.Click += OnZoomInClick;
        if (_zoomOutButton != null) _zoomOutButton.Click += OnZoomOutClick;
        if (_rotateLeftButton != null) _rotateLeftButton.Click += OnRotateLeftClick;
        if (_rotateRightButton != null) _rotateRightButton.Click += OnRotateRightClick;
        if (_resetButton != null) _resetButton.Click += OnResetClick;
    }

    private void UnsubscribeButtons()
    {
        if (_zoomInButton != null) _zoomInButton.Click -= OnZoomInClick;
        if (_zoomOutButton != null) _zoomOutButton.Click -= OnZoomOutClick;
        if (_rotateLeftButton != null) _rotateLeftButton.Click -= OnRotateLeftClick;
        if (_rotateRightButton != null) _rotateRightButton.Click -= OnRotateRightClick;
        if (_resetButton != null) _resetButton.Click -= OnResetClick;
    }

    private void OnZoomInClick(object? sender, RoutedEventArgs e) => ZoomIn();
    private void OnZoomOutClick(object? sender, RoutedEventArgs e) => ZoomOut();
    private void OnRotateLeftClick(object? sender, RoutedEventArgs e) => RotateLeft();
    private void OnRotateRightClick(object? sender, RoutedEventArgs e) => RotateRight();
    private void OnResetClick(object? sender, RoutedEventArgs e) => Reset();

    protected override void OnPointerWheelChanged(PointerWheelEventArgs e)
    {
        base.OnPointerWheelChanged(e);

        var delta = e.Delta.Y > 0 ? 0.1 : -0.1;
        var newZoom = Zoom + delta;
        newZoom = Math.Max(MinZoom, Math.Min(MaxZoom, newZoom));
        Zoom = Math.Round(newZoom, 2);
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed && Zoom > 1.0)
        {
            _isDragging = true;
            _lastPosition = e.GetPosition(this);
            e.Handled = true;
        }
    }

    protected override void OnPointerMoved(PointerEventArgs e)
    {
        base.OnPointerMoved(e);
        if (_isDragging && _imageContainer != null)
        {
            var current = e.GetPosition(this);
            var delta = current - _lastPosition;
            _lastPosition = current;

            var currentMargin = _imageContainer.Margin;
            _imageContainer.Margin = new Thickness(
                currentMargin.Left + delta.X,
                currentMargin.Top + delta.Y,
                currentMargin.Right,
                currentMargin.Bottom);
        }
    }

    protected override void OnPointerReleased(PointerReleasedEventArgs e)
    {
        base.OnPointerReleased(e);
        _isDragging = false;
    }

    private static void OnZoomChanged(ImageViewer sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender._scaleTransform?.SetValue(ScaleTransform.ScaleXProperty, sender.Zoom);
        sender._scaleTransform?.SetValue(ScaleTransform.ScaleYProperty, sender.Zoom);
    }

    private static void OnRotateChanged(ImageViewer sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender._rotateTransform?.SetValue(RotateTransform.AngleProperty, sender.Rotate);
    }

    public void ZoomIn()
    {
        Zoom = Math.Min(MaxZoom, Math.Round(Zoom + 0.2, 2));
    }

    public void ZoomOut()
    {
        Zoom = Math.Max(MinZoom, Math.Round(Zoom - 0.2, 2));
    }

    public void Reset()
    {
        Zoom = 1.0;
        Rotate = 0;
        if (_imageContainer != null)
        {
            _imageContainer.Margin = new Thickness(0);
        }
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
