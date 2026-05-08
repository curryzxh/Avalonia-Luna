using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Luna.Desktop.Controls;

[PseudoClasses(":open")]
public class LunaPopup : ContentControl
{
    public static readonly StyledProperty<object?> PopupContentProperty =
        AvaloniaProperty.Register<LunaPopup, object?>(nameof(PopupContent));

    public static readonly StyledProperty<Control?> AnchorProperty =
        AvaloniaProperty.Register<LunaPopup, Control?>(nameof(Anchor));

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<LunaPopup, bool>(nameof(IsOpen), false);

    public static readonly StyledProperty<PlacementMode> PlacementProperty =
        AvaloniaProperty.Register<LunaPopup, PlacementMode>(nameof(Placement), PlacementMode.BottomEdgeAlignedLeft);

    public static readonly StyledProperty<PopupTrigger> TriggerProperty =
        AvaloniaProperty.Register<LunaPopup, PopupTrigger>(nameof(Trigger), PopupTrigger.Click);

    public static readonly StyledProperty<bool> ShowArrowProperty =
        AvaloniaProperty.Register<LunaPopup, bool>(nameof(ShowArrow), false);

    public static readonly StyledProperty<bool> IsLightDismissEnabledProperty =
        AvaloniaProperty.Register<LunaPopup, bool>(nameof(IsLightDismissEnabled), true);

    private Popup? _popup;

    public object? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public Control? Anchor
    {
        get => GetValue(AnchorProperty);
        set => SetValue(AnchorProperty, value);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public PopupTrigger Trigger
    {
        get => GetValue(TriggerProperty);
        set => SetValue(TriggerProperty, value);
    }

    public bool ShowArrow
    {
        get => GetValue(ShowArrowProperty);
        set => SetValue(ShowArrowProperty, value);
    }

    public bool IsLightDismissEnabled
    {
        get => GetValue(IsLightDismissEnabledProperty);
        set => SetValue(IsLightDismissEnabledProperty, value);
    }

    static LunaPopup()
    {
        IsOpenProperty.Changed.AddClassHandler<LunaPopup>(OnIsOpenChanged);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        _popup = e.NameScope.Find<Popup>("PART_Popup");
        if (_popup != null)
        {
            _popup.Opened += OnPopupOpened;
            _popup.Closed += OnPopupClosed;
        }
    }

    protected override void OnPointerPressed(PointerPressedEventArgs e)
    {
        base.OnPointerPressed(e);
        if (Trigger == PopupTrigger.Click)
        {
            IsOpen = !IsOpen;
        }
    }

    protected override void OnPointerEntered(PointerEventArgs e)
    {
        base.OnPointerEntered(e);
        if (Trigger == PopupTrigger.Hover)
        {
            IsOpen = true;
        }
    }

    protected override void OnPointerExited(PointerEventArgs e)
    {
        base.OnPointerExited(e);
        if (Trigger == PopupTrigger.Hover)
        {
            IsOpen = false;
        }
    }

    private static void OnIsOpenChanged(LunaPopup sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.PseudoClasses.Set(":open", sender.IsOpen);
    }

    private void OnPopupOpened(object? sender, EventArgs e)
    {
        IsOpen = true;
    }

    private void OnPopupClosed(object? sender, EventArgs e)
    {
        IsOpen = false;
    }
}

public enum PopupTrigger
{
    Click,
    Hover,
    Focus,
    Manual,
}
