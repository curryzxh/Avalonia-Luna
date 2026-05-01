using Avalonia.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Sample.ViewModels;

public partial class RateDemoViewModel : DemoViewModelBase
{
    private static readonly IReadOnlyList<string> DefaultTexts = ["很差", "差", "一般", "好评", "优秀"];
    private static readonly IReadOnlyList<string> SpecialTextsSource = ["非常糟糕", "有些糟糕", "可以尝试", "可以前往", "推荐前往"];

    [ObservableProperty]
    private double _specialValue = 3;

    [ObservableProperty]
    private string _specialDescription = string.Empty;

    [ObservableProperty]
    private IBrush _specialDescriptionBrush = Brushes.Gray;

    [ObservableProperty]
    private FontWeight _specialDescriptionFontWeight = FontWeight.Normal;

    public RateDemoViewModel()
    {
        UpdateSpecialDescription(SpecialValue);
    }

    public IReadOnlyList<string> Texts { get; } = DefaultTexts;

    public IReadOnlyList<string> SpecialTexts { get; } = SpecialTextsSource;

    partial void OnSpecialValueChanged(double value)
    {
        UpdateSpecialDescription(value);
    }

    private void UpdateSpecialDescription(double value)
    {
        var roundedValue = (int)Math.Round(Math.Clamp(value, 0, 5));
        if (roundedValue <= 0)
        {
            SpecialDescription = string.Empty;
            SpecialDescriptionBrush = Brushes.Gray;
            SpecialDescriptionFontWeight = FontWeight.Normal;
            return;
        }

        SpecialDescription = SpecialTexts[Math.Clamp(roundedValue - 1, 0, SpecialTexts.Count - 1)];
        SpecialDescriptionBrush = roundedValue > 3 ? Brushes.Orange : Brushes.Black;
        SpecialDescriptionFontWeight = roundedValue > 3 ? FontWeight.SemiBold : FontWeight.Normal;
    }
}
