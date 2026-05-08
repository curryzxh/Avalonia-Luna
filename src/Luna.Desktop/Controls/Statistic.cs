using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;

namespace Luna.Desktop.Controls;

public enum StatisticTrend
{
    None,
    Up,
    Down,
}

public class Statistic : TemplatedControl
{
    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<Statistic, string?>(nameof(Title));

    public static readonly StyledProperty<string?> ValueProperty =
        AvaloniaProperty.Register<Statistic, string?>(nameof(Value), "0");

    public static readonly StyledProperty<string?> PrefixProperty =
        AvaloniaProperty.Register<Statistic, string?>(nameof(Prefix));

    public static readonly StyledProperty<string?> SuffixProperty =
        AvaloniaProperty.Register<Statistic, string?>(nameof(Suffix));

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<Statistic, string?>(nameof(Description));

    public static readonly StyledProperty<StatisticTrend> TrendProperty =
        AvaloniaProperty.Register<Statistic, StatisticTrend>(nameof(Trend), StatisticTrend.None);

    public static readonly StyledProperty<bool> LoadingProperty =
        AvaloniaProperty.Register<Statistic, bool>(nameof(Loading));

    public string? Title
    {
        get => GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public string? Value
    {
        get => GetValue(ValueProperty);
        set => SetValue(ValueProperty, value);
    }

    public string? Prefix
    {
        get => GetValue(PrefixProperty);
        set => SetValue(PrefixProperty, value);
    }

    public string? Suffix
    {
        get => GetValue(SuffixProperty);
        set => SetValue(SuffixProperty, value);
    }

    public string? Description
    {
        get => GetValue(DescriptionProperty);
        set => SetValue(DescriptionProperty, value);
    }

    public StatisticTrend Trend
    {
        get => GetValue(TrendProperty);
        set => SetValue(TrendProperty, value);
    }

    public bool Loading
    {
        get => GetValue(LoadingProperty);
        set => SetValue(LoadingProperty, value);
    }

    static Statistic()
    {
        TrendProperty.Changed.AddClassHandler<Statistic>((s, _) => s.UpdateTrend());
        LoadingProperty.Changed.AddClassHandler<Statistic>((s, _) => s.UpdateLoading());
    }

    public Statistic()
    {
        UpdateTrend();
        UpdateLoading();
    }

    private void UpdateTrend()
    {
        PseudoClasses.Set(":trend-up", Trend == StatisticTrend.Up);
        PseudoClasses.Set(":trend-down", Trend == StatisticTrend.Down);
    }

    private void UpdateLoading()
    {
        PseudoClasses.Set(":loading", Loading);
    }
}
