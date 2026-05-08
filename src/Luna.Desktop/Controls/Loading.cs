using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Media;
using Avalonia.Styling;

namespace Luna.Desktop.Controls;

[PseudoClasses(":spinner", ":circular", ":loading")]
public class Loading : ContentControl
{
    public static readonly StyledProperty<string> TextProperty =
        AvaloniaProperty.Register<Loading, string>(nameof(Text), "加载中...");

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(IsLoading), true);

    public static readonly StyledProperty<LoadingIndicator> IndicatorProperty =
        AvaloniaProperty.Register<Loading, LoadingIndicator>(nameof(Indicator), LoadingIndicator.Spinner);

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Loading, double>(nameof(Size), 24);

    private static readonly StyledProperty<double> RotationProperty =
        AvaloniaProperty.Register<Loading, double>(nameof(Rotation), 0);

    private IDisposable? _animationDisposable;

    public string Text
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

    private double Rotation
    {
        get => GetValue(RotationProperty);
        set => SetValue(RotationProperty, value);
    }

    static Loading()
    {
        IsLoadingProperty.Changed.AddClassHandler<Loading>(OnIsLoadingChanged);
        IndicatorProperty.Changed.AddClassHandler<Loading>(OnIndicatorChanged);
    }

    public override void Render(DrawingContext context)
    {
        base.Render(context);
    }

    protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
    {
        base.OnApplyTemplate(e);
        UpdateState();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        UpdateState();
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnDetachedFromVisualTree(e);
        StopAnimation();
    }

    private static void OnIsLoadingChanged(Loading sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdateState();
    }

    private static void OnIndicatorChanged(Loading sender, AvaloniaPropertyChangedEventArgs e)
    {
        sender.UpdatePseudoClasses();
    }

    private void UpdateState()
    {
        PseudoClasses.Set(":loading", IsLoading);
        UpdatePseudoClasses();

        if (IsLoading)
        {
            StartAnimation();
        }
        else
        {
            StopAnimation();
        }
    }

    private void UpdatePseudoClasses()
    {
        PseudoClasses.Set(":spinner", Indicator == LoadingIndicator.Spinner);
        PseudoClasses.Set(":circular", Indicator == LoadingIndicator.Circular);
    }

    private void StartAnimation()
    {
        StopAnimation();

        var animation = new Avalonia.Animation.Animation
        {
            Duration = TimeSpan.FromSeconds(1),
            IterationCount = IterationCount.Infinite,
            Easing = new LinearEasing(),
            Children =
            {
                new KeyFrame
                {
                    Cue = new Cue(0),
                    Setters = { new Setter(RotationProperty, 0.0) }
                },
                new KeyFrame
                {
                    Cue = new Cue(1),
                    Setters = { new Setter(RotationProperty, 360.0) }
                }
            }
        };

        _animationDisposable = animation.RunAsync(this);
    }

    private void StopAnimation()
    {
        _animationDisposable?.Dispose();
        _animationDisposable = null;
    }
}

public enum LoadingIndicator
{
    Spinner,
    Circular,
}
