using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Presenters;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Media;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// 气泡弹出层控件，支持多方向定位、主题切换与箭头显示。
/// </summary>
[TemplatePart("PART_Trigger", typeof(ContentPresenter))]
public sealed class Popover : ContentControl
{
    private Popup? _popup;
    private Border? _bubble;
    private Border? _arrow;
    private ContentPresenter? _popupContentPresenter;
    private ContentPresenter? _triggerPresenter;
    private bool _isBuildingPopup;

    /// <inheritdoc cref="PopupContent" />
    public static readonly StyledProperty<object?> PopupContentProperty =
        AvaloniaProperty.Register<Popover, object?>(nameof(PopupContent));

    /// <inheritdoc cref="Placement" />
    public static readonly StyledProperty<PopoverPlacement> PlacementProperty =
        AvaloniaProperty.Register<Popover, PopoverPlacement>(nameof(Placement), PopoverPlacement.Top);

    /// <inheritdoc cref="Theme" />
    public new static readonly StyledProperty<PopoverTheme> ThemeProperty =
        AvaloniaProperty.Register<Popover, PopoverTheme>(nameof(Theme), PopoverTheme.Dark);

    /// <inheritdoc cref="ShowArrow" />
    public static readonly StyledProperty<bool> ShowArrowProperty =
        AvaloniaProperty.Register<Popover, bool>(nameof(ShowArrow), true);

    /// <inheritdoc cref="IsOpen" />
    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Popover, bool>(nameof(IsOpen));

    static Popover()
    {
        PlacementProperty.Changed.AddClassHandler<Popover>((control, _) => control.UpdatePlacement());
        ThemeProperty.Changed.AddClassHandler<Popover>((control, _) => control.UpdateTheme());
        ShowArrowProperty.Changed.AddClassHandler<Popover>((control, _) =>
        {
            control.PseudoClasses.Set(":arrow", control.ShowArrow);
            control.UpdateArrowVisibility();
        });
        IsOpenProperty.Changed.AddClassHandler<Popover>((control, args) =>
        {
            control.PseudoClasses.Set(":open", args.GetNewValue<bool>());
            control.OnIsOpenChanged(args.GetNewValue<bool>());
        });
        PopupContentProperty.Changed.AddClassHandler<Popover>((control, _) => control.UpdatePopupContent());
    }

    /// <summary>
    /// 初始化 <see cref="Popover" /> 的新实例。
    /// </summary>
    public Popover()
    {
        PseudoClasses.Set(":arrow", ShowArrow);
        UpdatePlacement();
    }

    /// <summary>
    /// 获取或设置弹层内容。
    /// </summary>
    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    /// <summary>
    /// 获取或设置弹层相对于触发器的位置。
    /// </summary>
    public PopoverPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    /// <summary>
    /// 获取或设置弹层主题。
    /// </summary>
    public new PopoverTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示箭头。
    /// </summary>
    public bool ShowArrow
    {
        get => GetValue(ShowArrowProperty);
        set => SetValue(ShowArrowProperty, value);
    }

    /// <summary>
    /// 获取或设置弹层是否打开。
    /// </summary>
    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    /// <inheritdoc />
    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _triggerPresenter = e.NameScope.Find<ContentPresenter>("PART_Trigger");
    }

    /// <inheritdoc />
    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        EnsurePopup();
    }

    /// <inheritdoc />
    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        ClosePopup();
        if (_popup is not null)
        {
            _popup.Closed -= OnPopupClosed;
            _popup = null;
        }
    }

    /// <inheritdoc />
    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        IsOpen = !IsOpen;
    }

    private void EnsurePopup()
    {
        if (_popup is not null || _isBuildingPopup)
            return;

        _isBuildingPopup = true;
        try
        {
            _arrow = new Border
            {
                Width = 10,
                Height = 10,
                RenderTransformOrigin = new RelativePoint(0.5, 0.5, RelativeUnit.Relative),
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom,
                Margin = new Thickness(0, -5, 0, 0),
                RenderTransform = new RotateTransform(45),
            };

            _popupContentPresenter = new ContentPresenter
            {
                Name = "PART_PopupContent",
            };

            _bubble = new Border
            {
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(12, 8),
                Margin = new Thickness(0, 5, 0, 0),
                MaxWidth = 240,
                Child = _popupContentPresenter,
            };

            var grid = new Grid();
            grid.Children.Add(_arrow);
            grid.Children.Add(_bubble);

            _popup = new Popup
            {
                Child = grid,
                PlacementTarget = this,
                IsLightDismissEnabled = true,
            };

            _popup.Closed += OnPopupClosed;

            UpdateTheme();
            UpdatePopupContent();
            UpdatePlacement();
            UpdateArrowVisibility();

            if (IsOpen)
            {
                _popup.IsOpen = true;
            }
        }
        finally
        {
            _isBuildingPopup = false;
        }
    }

    private void OnIsOpenChanged(bool isOpen)
    {
        if (_popup is null)
            return;

        _popup.IsOpen = isOpen;
    }

    private void OnPopupClosed(object? sender, EventArgs e)
    {
        IsOpen = false;
    }

    private void ClosePopup()
    {
        if (_popup is not null)
        {
            _popup.IsOpen = false;
        }
    }

    private void UpdateTheme()
    {
        if (_bubble is null || _arrow is null)
            return;

        var (bg, fg) = Theme switch
        {
            PopoverTheme.Light => (GetResourceBrush("Luna.Brush.Background.Container"), GetResourceBrush("Luna.Brush.Text.Primary")),
            PopoverTheme.Brand => (GetResourceBrush("Luna.Brush.Brand"), GetWhiteBrush()),
            PopoverTheme.Success => (GetResourceBrush("Luna.Brush.Success"), GetWhiteBrush()),
            PopoverTheme.Warning => (GetResourceBrush("Luna.Brush.Warning"), GetWhiteBrush()),
            PopoverTheme.Error => (GetResourceBrush("Luna.Brush.Error"), GetWhiteBrush()),
            _ => (GetResourceBrush("Luna.Brush.Text.Primary"), GetWhiteBrush()),
        };

        _bubble.Background = bg;
        _arrow.Background = bg;

        if (_popupContentPresenter is not null)
        {
            _popupContentPresenter.Foreground = fg;
        }

        PseudoClasses.Set(":dark", Theme == PopoverTheme.Dark);
        PseudoClasses.Set(":light", Theme == PopoverTheme.Light);
        PseudoClasses.Set(":brand", Theme == PopoverTheme.Brand);
        PseudoClasses.Set(":success", Theme == PopoverTheme.Success);
        PseudoClasses.Set(":warning", Theme == PopoverTheme.Warning);
        PseudoClasses.Set(":error", Theme == PopoverTheme.Error);
    }

    private void UpdatePopupContent()
    {
        if (_popupContentPresenter is null)
            return;

        _popupContentPresenter.Content = PopupContent;

        if (this.TryFindResource("Luna.FontSize.Body.Medium", out var fs) && fs is double d)
        {
            _popupContentPresenter.FontSize = d;
        }
    }

    private void UpdatePlacement()
    {
        var placement = Placement;
        PseudoClasses.Set(":top", placement == PopoverPlacement.Top);
        PseudoClasses.Set(":top-left", placement == PopoverPlacement.TopLeft);
        PseudoClasses.Set(":top-right", placement == PopoverPlacement.TopRight);
        PseudoClasses.Set(":bottom", placement == PopoverPlacement.Bottom);
        PseudoClasses.Set(":bottom-left", placement == PopoverPlacement.BottomLeft);
        PseudoClasses.Set(":bottom-right", placement == PopoverPlacement.BottomRight);
        PseudoClasses.Set(":left", placement == PopoverPlacement.Left);
        PseudoClasses.Set(":left-top", placement == PopoverPlacement.LeftTop);
        PseudoClasses.Set(":left-bottom", placement == PopoverPlacement.LeftBottom);
        PseudoClasses.Set(":right", placement == PopoverPlacement.Right);
        PseudoClasses.Set(":right-top", placement == PopoverPlacement.RightTop);
        PseudoClasses.Set(":right-bottom", placement == PopoverPlacement.RightBottom);

        if (_popup is null || _arrow is null || _bubble is null)
            return;

        var mode = placement switch
        {
            PopoverPlacement.Top or PopoverPlacement.TopLeft or PopoverPlacement.TopRight => PlacementMode.Top,
            PopoverPlacement.Bottom or PopoverPlacement.BottomLeft or PopoverPlacement.BottomRight => PlacementMode.Bottom,
            PopoverPlacement.Left or PopoverPlacement.LeftTop or PopoverPlacement.LeftBottom => PlacementMode.Left,
            PopoverPlacement.Right or PopoverPlacement.RightTop or PopoverPlacement.RightBottom => PlacementMode.Right,
            _ => PlacementMode.Top,
        };

        _popup.Placement = mode;

        if (mode == PlacementMode.Bottom)
        {
            _arrow.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Top;
            _arrow.Margin = new Thickness(0, 0, 0, -5);
            _bubble.Margin = new Thickness(0, 0, 0, 5);
        }
        else if (mode == PlacementMode.Top)
        {
            _arrow.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Bottom;
            _arrow.Margin = new Thickness(0, -5, 0, 0);
            _bubble.Margin = new Thickness(0, 5, 0, 0);
        }
        else if (mode == PlacementMode.Left)
        {
            _arrow.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right;
            _arrow.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            _arrow.Margin = new Thickness(-5, 0, 0, 0);
            _bubble.Margin = new Thickness(5, 0, 0, 0);
        }
        else if (mode == PlacementMode.Right)
        {
            _arrow.HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Left;
            _arrow.VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center;
            _arrow.Margin = new Thickness(0, 0, -5, 0);
            _bubble.Margin = new Thickness(0, 0, 5, 0);
        }
    }

    private void UpdateArrowVisibility()
    {
        if (_arrow is null)
            return;
        _arrow.IsVisible = ShowArrow;
        if (!ShowArrow && _bubble is not null)
        {
            _bubble.Margin = new Thickness(0);
        }
    }

    private IBrush GetResourceBrush(string key)
    {
        return this.TryFindResource(key, out var value) && value is IBrush b ? b : Brushes.Gray;
    }

    private static IBrush GetWhiteBrush() => Brushes.White;
}
