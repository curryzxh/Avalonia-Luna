using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

[PseudoClasses(PC_Info, PC_Success, PC_Warning, PC_Error)]
public class Alert : TemplatedControl
{
    private const string PC_Info = ":info";
    private const string PC_Success = ":success";
    private const string PC_Warning = ":warning";
    private const string PC_Error = ":error";

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Alert, string?>(nameof(Title));

    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<Alert, string?>(nameof(Message));

    public static readonly StyledProperty<AlertTheme> ThemeProperty =
        AvaloniaProperty.Register<Alert, AlertTheme>(nameof(Theme), AlertTheme.Info);

    public static readonly StyledProperty<bool> IsClosableProperty =
        AvaloniaProperty.Register<Alert, bool>(nameof(IsClosable));

    public static readonly StyledProperty<bool> IsClosedProperty =
        AvaloniaProperty.Register<Alert, bool>(nameof(IsClosed));

    static Alert()
    {
        ThemeProperty.Changed.AddClassHandler<Alert>((control, _) => control.UpdatePseudoClasses());
        IsClosedProperty.Changed.AddClassHandler<Alert>((control, _) =>
            control.PseudoClasses.Set(":closed", control.IsClosed));
    }

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }

    public AlertTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public bool IsClosable
    {
        get => GetValue(IsClosableProperty);
        set => SetValue(IsClosableProperty, value);
    }

    public bool IsClosed
    {
        get => GetValue(IsClosedProperty);
        set => SetValue(IsClosedProperty, value);
    }

    public event EventHandler? Closed;

    public void Close()
    {
        IsClosed = true;
        Closed?.Invoke(this, EventArgs.Empty);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        var closeButton = e.NameScope.Find<Button>("PART_CloseButton");
        if (closeButton is not null)
        {
            closeButton.Click += (_, _) => Close();
        }
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(PC_Info, Theme == AlertTheme.Info);
        PseudoClasses.Set(PC_Success, Theme == AlertTheme.Success);
        PseudoClasses.Set(PC_Warning, Theme == AlertTheme.Warning);
        PseudoClasses.Set(PC_Error, Theme == AlertTheme.Error);
    }
}

public enum AlertTheme
{
    Info,
    Success,
    Warning,
    Error,
}
