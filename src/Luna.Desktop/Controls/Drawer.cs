using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Layout;

namespace Luna.Desktop.Controls;

public enum DrawerPlacement
{
    Left,
    Right,
    Top,
    Bottom,
}

public sealed record DrawerOptions
{
    public string? Title { get; init; }
    public object? Content { get; init; }
    public DrawerPlacement Placement { get; init; } = DrawerPlacement.Right;
    public double Size { get; init; } = 400;
    public bool ShowCloseButton { get; init; } = true;
    public bool CloseOnOverlayClick { get; init; } = true;
}

[PseudoClasses(PC_Open, PC_Left, PC_Right, PC_Top, PC_Bottom)]
public sealed class DrawerHost : TemplatedControl
{
    private const string PC_Open = ":open";
    private const string PC_Left = ":left";
    private const string PC_Right = ":right";
    private const string PC_Top = ":top";
    private const string PC_Bottom = ":bottom";

    private static DrawerHost? _current;
    private Border? _overlay;
    private DrawerOptions? _options;

    public static DrawerHost? Current => _current;

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(IsOpen));

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DrawerHost, string?>(nameof(Title));

    public static readonly StyledProperty<object?> ContentProperty =
        AvaloniaProperty.Register<DrawerHost, object?>(nameof(Content));

    public static readonly StyledProperty<DrawerPlacement> PlacementProperty =
        AvaloniaProperty.Register<DrawerHost, DrawerPlacement>(nameof(Placement), DrawerPlacement.Right);

    public static readonly StyledProperty<double> DrawerSizeProperty =
        AvaloniaProperty.Register<DrawerHost, double>(nameof(DrawerSize), 400);

    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(ShowCloseButton), true);

    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(CloseOnOverlayClick), true);

    static DrawerHost()
    {
        IsOpenProperty.Changed.AddClassHandler<DrawerHost>((control, args) =>
            control.PseudoClasses.Set(PC_Open, args.GetNewValue<bool>()));
        PlacementProperty.Changed.AddClassHandler<DrawerHost>((control, _) => control.UpdatePlacementPseudoClasses());
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

    public object? Content
    {
        get => GetValue(ContentProperty);
        set => SetValue(ContentProperty, value);
    }

    public DrawerPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public double DrawerSize
    {
        get => GetValue(DrawerSizeProperty);
        set => SetValue(DrawerSizeProperty, value);
    }

    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    public event EventHandler? Closed;

    public void Show(DrawerOptions options)
    {
        _options = options;
        Title = options.Title;
        Content = options.Content;
        Placement = options.Placement;
        DrawerSize = options.Size;
        ShowCloseButton = options.ShowCloseButton;
        CloseOnOverlayClick = options.CloseOnOverlayClick;
        IsOpen = true;
        UpdatePlacementPseudoClasses();
    }

    public void Close()
    {
        _options = null;
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
            _current = null;
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        if (_overlay is not null)
            _overlay.PointerPressed -= OnOverlayPressed;

        _overlay = e.NameScope.Find<Border>("PART_Overlay");

        var closeBtn = e.NameScope.Find<Button>("PART_CloseButton");
        if (closeBtn is not null)
            closeBtn.Click += (_, _) => Close();

        if (_overlay is not null)
            _overlay.PointerPressed += OnOverlayPressed;
    }

    private void OnOverlayPressed(object? sender, PointerPressedEventArgs e)
    {
        if (IsOpen && CloseOnOverlayClick)
        {
            e.Handled = true;
            Close();
        }
    }

    private void UpdatePlacementPseudoClasses()
    {
        PseudoClasses.Set(PC_Left, Placement == DrawerPlacement.Left);
        PseudoClasses.Set(PC_Right, Placement == DrawerPlacement.Right);
        PseudoClasses.Set(PC_Top, Placement == DrawerPlacement.Top);
        PseudoClasses.Set(PC_Bottom, Placement == DrawerPlacement.Bottom);
    }
}

public static class LunaDrawer
{
    public static void Show(DrawerOptions options) => DrawerHost.Current?.Show(options);
    public static void Close() => DrawerHost.Current?.Close();
}
