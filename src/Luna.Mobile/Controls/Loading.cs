using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using System;

namespace Luna.Mobile.Controls;

public enum LoadingTheme
{
    Circular,
    Spinner,
    Dots,
}

public sealed class Loading : ContentControl
{
    private bool _showStandalone;
    private bool _showOverlay;
    private bool _showText;

    public Loading()
    {
        UpdateVisibilityFlags();
    }

    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(IsLoading), true);

    public static readonly new StyledProperty<LoadingTheme> ThemeProperty =
        AvaloniaProperty.Register<Loading, LoadingTheme>(nameof(Theme), LoadingTheme.Circular);

    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Loading, double>(nameof(Size), 22);

    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<Loading, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(3000));

    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Loading, string?>(nameof(Text));

    public static readonly StyledProperty<Orientation> LayoutProperty =
        AvaloniaProperty.Register<Loading, Orientation>(nameof(Layout), Orientation.Horizontal);

    public static readonly StyledProperty<bool> IndicatorProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(Indicator), true);

    public static readonly DirectProperty<Loading, bool> ShowStandaloneProperty =
        AvaloniaProperty.RegisterDirect<Loading, bool>(
            nameof(ShowStandalone),
            o => o.ShowStandalone);

    public static readonly DirectProperty<Loading, bool> ShowOverlayProperty =
        AvaloniaProperty.RegisterDirect<Loading, bool>(
            nameof(ShowOverlay),
            o => o.ShowOverlay);

    public static readonly DirectProperty<Loading, bool> ShowTextProperty =
        AvaloniaProperty.RegisterDirect<Loading, bool>(
            nameof(ShowText),
            o => o.ShowText);

    static Loading()
    {
        IsLoadingProperty.Changed.AddClassHandler<Loading>((control, _) => control.UpdateVisibilityFlags());
        ContentProperty.Changed.AddClassHandler<Loading>((control, _) => control.UpdateVisibilityFlags());
        TextProperty.Changed.AddClassHandler<Loading>((control, _) => control.UpdateVisibilityFlags());
    }

    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    public new LoadingTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public Orientation Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    public bool Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    public bool ShowStandalone
    {
        get => _showStandalone;
        private set => SetAndRaise(ShowStandaloneProperty, ref _showStandalone, value);
    }

    public bool ShowOverlay
    {
        get => _showOverlay;
        private set => SetAndRaise(ShowOverlayProperty, ref _showOverlay, value);
    }

    public bool ShowText
    {
        get => _showText;
        private set => SetAndRaise(ShowTextProperty, ref _showText, value);
    }

    private void UpdateVisibilityFlags()
    {
        var hasContent = Content is not null;
        ShowStandalone = IsLoading && !hasContent;
        ShowOverlay = IsLoading && hasContent;
        ShowText = !string.IsNullOrWhiteSpace(Text);
    }
}
