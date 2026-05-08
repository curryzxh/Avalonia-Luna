using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

[PseudoClasses(PC_Open)]
public class Popconfirm : ContentControl
{
    private const string PC_Open = ":open";

    public static readonly StyledProperty<string?> PopupContentProperty =
        AvaloniaProperty.Register<Popconfirm, string?>(nameof(PopupContent));

    public static readonly StyledProperty<string?> ConfirmTextProperty =
        AvaloniaProperty.Register<Popconfirm, string?>(nameof(ConfirmText), "确认");

    public static readonly StyledProperty<string?> CancelTextProperty =
        AvaloniaProperty.Register<Popconfirm, string?>(nameof(CancelText), "取消");

    public static readonly StyledProperty<bool> IsOpenProperty =
        AvaloniaProperty.Register<Popconfirm, bool>(nameof(IsOpen));

    public static readonly StyledProperty<Avalonia.Controls.PlacementMode> PlacementProperty =
        AvaloniaProperty.Register<Popconfirm, Avalonia.Controls.PlacementMode>(nameof(Placement),
            Avalonia.Controls.PlacementMode.Bottom);

    static Popconfirm()
    {
        IsOpenProperty.Changed.AddClassHandler<Popconfirm>((control, args) =>
            control.PseudoClasses.Set(PC_Open, args.GetNewValue<bool>()));
    }

    public string? PopupContent
    {
        get => GetValue(PopupContentProperty);
        set => SetValue(PopupContentProperty, value);
    }

    public string? ConfirmText
    {
        get => GetValue(ConfirmTextProperty);
        set => SetValue(ConfirmTextProperty, value);
    }

    public string? CancelText
    {
        get => GetValue(CancelTextProperty);
        set => SetValue(CancelTextProperty, value);
    }

    public bool IsOpen
    {
        get => GetValue(IsOpenProperty);
        set => SetValue(IsOpenProperty, value);
    }

    public Avalonia.Controls.PlacementMode Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }

    public event EventHandler? Confirmed;
    public event EventHandler? Canceled;

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);

        var confirmButton = e.NameScope.Find<Button>("PART_ConfirmButton");
        var cancelButton = e.NameScope.Find<Button>("PART_CancelButton");

        if (confirmButton is not null)
        {
            confirmButton.Click += (_, _) =>
            {
                IsOpen = false;
                Confirmed?.Invoke(this, EventArgs.Empty);
            };
        }

        if (cancelButton is not null)
        {
            cancelButton.Click += (_, _) =>
            {
                IsOpen = false;
                Canceled?.Invoke(this, EventArgs.Empty);
            };
        }
    }
}
