using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.VisualTree;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 抽屉宿主控件，负责渲染遮罩与侧边抽屉面板，并处理打开/关闭逻辑。
/// </summary>
/// <remarks>
/// 通常每个页面放置一个实例；静态入口 <see cref="Drawer"/> 会使用最近附加到可视树的 <see cref="Current"/>。
/// </remarks>
[TemplatePart(OverlayPartName, typeof(Border))]
[TemplatePart(CloseButtonPartName, typeof(Button))]
public sealed class DrawerHost : TemplatedControl
{
    private const string OverlayPartName = "PART_Overlay";
    private const string CloseButtonPartName = "PART_CloseButton";

    private static DrawerHost? _current;

    private Border? _overlay;
    private Button? _closeButton;

    private bool _isOverlayVisible;
    private bool _hasTitle;

    /// <summary>
    /// 获取当前附加到可视树的抽屉宿主实例。
    /// </summary>
    public static DrawerHost? Current => _current;

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(IsOpen));

    /// <inheritdoc cref="Title" />
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<DrawerHost, string?>(nameof(Title));

    /// <inheritdoc cref="DrawerContent" />
    public static readonly StyledProperty<object?> DrawerContentProperty =
        AvaloniaProperty.Register<DrawerHost, object?>(nameof(DrawerContent));

    /// <inheritdoc cref="DrawerWidth" />
    public static readonly StyledProperty<double> DrawerWidthProperty =
        AvaloniaProperty.Register<DrawerHost, double>(nameof(DrawerWidth), 280);

    /// <inheritdoc cref="Placement" />
    public static readonly StyledProperty<DrawerPlacement> PlacementProperty =
        AvaloniaProperty.Register<DrawerHost, DrawerPlacement>(nameof(Placement), DrawerPlacement.Right);

    /// <inheritdoc cref="ShowOverlay" />
    public static readonly StyledProperty<bool> ShowOverlayProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(ShowOverlay), true);

    /// <inheritdoc cref="CloseOnOverlayClick" />
    public static readonly StyledProperty<bool> CloseOnOverlayClickProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(CloseOnOverlayClick), true);

    /// <inheritdoc cref="ShowCloseButton" />
    public static readonly StyledProperty<bool> ShowCloseButtonProperty =
        AvaloniaProperty.Register<DrawerHost, bool>(nameof(ShowCloseButton), true);

    /// <inheritdoc cref="IsOverlayVisible" />
    public static readonly DirectProperty<DrawerHost, bool> IsOverlayVisibleProperty =
        AvaloniaProperty.RegisterDirect<DrawerHost, bool>(
            nameof(IsOverlayVisible),
            o => o.IsOverlayVisible);

    /// <inheritdoc cref="HasTitle" />
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

    /// <summary>
    /// 初始化 <see cref="DrawerHost" /> 的新实例。
    /// </summary>
    public DrawerHost()
    {
        HasTitle = !string.IsNullOrWhiteSpace(Title);
        PseudoClasses.Set(":left", Placement == DrawerPlacement.Left);
        PseudoClasses.Set(":right", Placement == DrawerPlacement.Right);
        UpdateOverlayVisible();
    }

    /// <summary>
    /// 获取或设置抽屉当前是否打开。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <summary>
    /// 获取或设置标题文本。
    /// </summary>
    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    /// <summary>
    /// 获取或设置抽屉内容。
    /// </summary>
    public object? DrawerContent
    {
        get => GetValue(DrawerContentProperty);
        set => SetValue(DrawerContentProperty, value);
    }

    /// <summary>
    /// 获取或设置抽屉宽度。
    /// </summary>
    public double DrawerWidth
    {
        get => GetValue(DrawerWidthProperty);
        set => SetValue(DrawerWidthProperty, value);
    }

    /// <summary>
    /// 获取或设置抽屉出现方向。
    /// </summary>
    public DrawerPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示遮罩层。
    /// </summary>
    public bool ShowOverlay
    {
        get => GetValue(ShowOverlayProperty);
        set => SetValue(ShowOverlayProperty, value);
    }

    /// <summary>
    /// 获取或设置是否允许点击遮罩关闭抽屉。
    /// </summary>
    public bool CloseOnOverlayClick
    {
        get => GetValue(CloseOnOverlayClickProperty);
        set => SetValue(CloseOnOverlayClickProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示关闭按钮。
    /// </summary>
    public bool ShowCloseButton
    {
        get => GetValue(ShowCloseButtonProperty);
        set => SetValue(ShowCloseButtonProperty, value);
    }

    /// <summary>
    /// 获取当前遮罩层是否可见。
    /// </summary>
    public bool IsOverlayVisible
    {
        get => _isOverlayVisible;
        private set => SetAndRaise(IsOverlayVisibleProperty, ref _isOverlayVisible, value);
    }

    /// <summary>
    /// 获取当前是否存在标题。
    /// </summary>
    public bool HasTitle
    {
        get => _hasTitle;
        private set => SetAndRaise(HasTitleProperty, ref _hasTitle, value);
    }

    /// <summary>
    /// 抽屉关闭后触发。
    /// </summary>
    public event EventHandler? Closed;

    /// <summary>
    /// 使用指定参数打开抽屉。
    /// </summary>
    /// <param name="options">抽屉配置参数。</param>
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

    /// <summary>
    /// 关闭当前抽屉。
    /// </summary>
    public void Close()
    {
        IsOpen = false;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        _current = this;
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        if (ReferenceEquals(_current, this))
        {
            _current = null;
        }
    }

    /// <inheritdoc />
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
