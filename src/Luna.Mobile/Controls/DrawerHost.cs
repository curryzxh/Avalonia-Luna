using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

public sealed class DrawerHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string CloseButtonPartName = "PART_CloseButton";

    private static DrawerHost? _current;

    private Border? _overlay;
    private Button? _closeButton;

    private bool _isOverlayVisible;
    private bool _hasTitle;

    public static DrawerHost? Current => _current;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DrawerHost, string?>(nameof(Title));

    public static readonly StyledProperty<object?> DrawerContentProperty =
        AvaloniaProperty.Register<DrawerHost, object?>(nameof(DrawerContent));

    public static readonly StyledProperty<double> DrawerWidthProperty =
        AvaloniaProperty.Register<DrawerHost, double>(nameof(DrawerWidth), 280);

    public static readonly StyledProperty<DrawerPlacement> PlacementProperty =
        AvaloniaProperty.Register<DrawerHost, DrawerPlacement>(nameof(Placement), DrawerPlacement.Right);

    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(ShowOverlay), true);

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(CloseOnOverlayClick), true);

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(ShowCloseButton), true);

    public static readonly DirectProperty<DrawerHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<DrawerHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    public static readonly DirectProperty<DrawerHost, bool> HasTitleProperty =
        AvaloniaProperty.RegisterDirect<DrawerHost, bool>(
            nameof(HasTitle),
            o => o.HasTitle);

    static DrawerHost()
    {
        IsOpenProperty.Changed.AddClassHandler<DrawerHost>((control, args) =>
        {
            control.PseudoClasses.Set(":open", args.GetNewValue<bool>());
            control.UpdateOverlayVisible();
        });

        ShowOverlayProperty.Changed.AddClassHandler<DrawerHost>((control, _) => control.UpdateOverlayVisible());

        TitleProperty.Changed.AddClassHandler<DrawerHost>((control, args) =>
        {
            control.HasTitle = !string.IsNullOrWhiteSpace(args.GetNewValue<string?>());
        });

        PlacementProperty.Changed.AddClassHandler<DrawerHost>((control, args) =>
        {
            var placement = args.GetNewValue<DrawerPlacement>();
            control.PseudoClasses.Set(":left", placement == DrawerPlacement.Left);
            control.PseudoClasses.Set(":right", placement == DrawerPlacement.Right);
        });
    }

    public DrawerHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
        PseudoClasses.Set(":left", Placement == DrawerPlacement.Left);
        PseudoClasses.Set(":right", Placement == DrawerPlacement.Right);
        UpdateOverlayVisible();
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public object? DrawerContent
    {
        get => GetValue(DrawerContentProperty);
        set => SetValue(DrawerContentProperty, value);
    }

    public double DrawerWidth
    {
        get => GetValue(DrawerWidthProperty);
        set => SetValue(DrawerWidthProperty, value);
    }

    public DrawerPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
    }

    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    public event EventHandler? Closed;

    public void Show(DrawerOptions options)
    {
        Title = options.Title;
        DrawerContent = options.Content;
        DrawerWidth = options.Width;
        Placement = options.Placement;
        ShowOverlay = options.ShowOverlay;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        ShowCloseButton = options.ShowCloseButton;

        HasTitle = !string.IsNullOrWhiteSpace(Title);
        IsOpen = true;
    }

    public void Close()
    {
        IsOpen = false;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
        {
            _overlay.PointerPressed -= OnOverlayPressed;
        }
        if (_closeButton is not null)
        {
            _closeButton.Click -= OnCloseClick;
        }

        _overlay = e.NameScope.Find<Border>(OverlayPartName);
        _closeButton = e.NameScope.Find<Button>(CloseButtonPartName);

        if (_overlay is not null)
        {
            _overlay.PointerPressed += OnOverlayPressed;
        }
        if (_closeButton is not null)
        {
            _closeButton.Click += OnCloseClick;
        }
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (!CloseOnOverlayClick)
        {
            return;
        }

        Close();
    }

    private void OnCloseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Close();
    }

    private void UpdateOverlayVisible()
    {
        IsOverlayVisible = IsOpen && ShowOverlay;
    }
}

