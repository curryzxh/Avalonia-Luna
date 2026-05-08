using Avalonia;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

[PseudoClasses(PC_Spinner)]
public class Loading : TemplatedControl
{
    private const string PC_Spinner = ":spinner";

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Loading, string?>(nameof(Text));

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(IsLoading), true);

    public static readonly StyledProperty<LoadingIndicator> IndicatorProperty =
        AvaloniaProperty.Register<Loading, LoadingIndicator>(nameof(Indicator), LoadingIndicator.Spinner);

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Loading, double>(nameof(Size), 24);

    static Loading()
    {
        IndicatorProperty.Changed.AddClassHandler<Loading>((control, _) => control.UpdatePseudoClasses());
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public LoadingIndicator Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdatePseudoClasses();
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(PC_Spinner, Indicator == LoadingIndicator.Spinner);
    }
}

public enum LoadingIndicator
{
    Spinner,
}
