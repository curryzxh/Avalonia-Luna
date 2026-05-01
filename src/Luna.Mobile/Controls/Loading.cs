using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using System;

namespace Luna.Mobile.Controls;

/// <summary>
/// Loading 指示器主题类型。
/// </summary>
public enum LoadingTheme
{
    /// <summary>
    /// Circular。
    /// </summary>
    Circular,
    /// <summary>
    /// Spinner。
    /// </summary>
    Spinner,
    /// <summary>
    /// Dots。
    /// </summary>
    Dots,
}

/// <summary>
/// Loading 控件，可作为独立加载状态，也可包裹内容并在其上显示遮罩加载。
/// </summary>
public sealed class Loading : ContentControl
{
    private bool _showStandalone;
    private bool _showOverlay;
    private bool _showText;

    /// <summary>
    /// 初始化 <see cref="Loading" /> 的新实例。
    /// </summary>
    public Loading()
    {
        UpdateVisibilityFlags();
    }

    /// <inheritdoc cref="IsLoading" />
    public static readonly StyledProperty<bool> IsLoadingProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(IsLoading), true);

    /// <inheritdoc cref="Theme" />
    public static readonly new StyledProperty<LoadingTheme> ThemeProperty =
        AvaloniaProperty.Register<Loading, LoadingTheme>(nameof(Theme), LoadingTheme.Circular);

    /// <inheritdoc cref="Size" />
    public static readonly StyledProperty<double> SizeProperty =
        AvaloniaProperty.Register<Loading, double>(nameof(Size), 22);

    /// <inheritdoc cref="Duration" />
    public static readonly StyledProperty<TimeSpan> DurationProperty =
        AvaloniaProperty.Register<Loading, TimeSpan>(nameof(Duration), TimeSpan.FromMilliseconds(3000));

    /// <inheritdoc cref="Text" />
    public static readonly StyledProperty<string?> TextProperty =
        AvaloniaProperty.Register<Loading, string?>(nameof(Text));

    /// <inheritdoc cref="Layout" />
    public static readonly StyledProperty<Orientation> LayoutProperty =
        AvaloniaProperty.Register<Loading, Orientation>(nameof(Layout), Orientation.Horizontal);

    /// <inheritdoc cref="Indicator" />
    public static readonly StyledProperty<bool> IndicatorProperty =
        AvaloniaProperty.Register<Loading, bool>(nameof(Indicator), true);

    /// <inheritdoc cref="ShowStandalone" />
    public static readonly DirectProperty<Loading, bool> ShowStandaloneProperty =
        AvaloniaProperty.RegisterDirect<Loading, bool>(
            nameof(ShowStandalone),
            o => o.ShowStandalone);

    /// <inheritdoc cref="ShowOverlay" />
    public static readonly DirectProperty<Loading, bool> ShowOverlayProperty =
        AvaloniaProperty.RegisterDirect<Loading, bool>(
            nameof(ShowOverlay),
            o => o.ShowOverlay);

    /// <inheritdoc cref="ShowText" />
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

    /// <summary>
    /// 获取或设置是否处于加载状态。
    /// </summary>
    public bool IsLoading
    {
        get => GetValue(IsLoadingProperty);
        set => SetValue(IsLoadingProperty, value);
    }

    /// <summary>
    /// 获取或设置加载指示器主题。
    /// </summary>
    public new LoadingTheme Theme
    {
        get => GetValue(ThemeProperty);
        set => SetValue(ThemeProperty, value);
    }

    /// <summary>
    /// 获取或设置指示器尺寸。
    /// </summary>
    public double Size
    {
        get => GetValue(SizeProperty);
        set => SetValue(SizeProperty, value);
    }

    /// <summary>
    /// 获取或设置默认加载动画周期。
    /// </summary>
    public TimeSpan Duration
    {
        get => GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    /// <summary>
    /// 获取或设置提示文本。
    /// </summary>
    public string? Text
    {
        get => GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    /// <summary>
    /// 获取或设置文本与指示器的布局方向。
    /// </summary>
    public Orientation Layout
    {
        get => GetValue(LayoutProperty);
        set => SetValue(LayoutProperty, value);
    }

    /// <summary>
    /// 获取或设置是否显示指示器。
    /// </summary>
    public bool Indicator
    {
        get => GetValue(IndicatorProperty);
        set => SetValue(IndicatorProperty, value);
    }

    /// <summary>
    /// 获取当前是否以独立加载态显示。
    /// </summary>
    public bool ShowStandalone
    {
        get => _showStandalone;
        private set => SetAndRaise(ShowStandaloneProperty, ref _showStandalone, value);
    }

    /// <summary>
    /// 获取当前是否以内容遮罩形式显示。
    /// </summary>
    public bool ShowOverlay
    {
        get => _showOverlay;
        private set => SetAndRaise(ShowOverlayProperty, ref _showOverlay, value);
    }

    /// <summary>
    /// 获取当前是否显示提示文本。
    /// </summary>
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
