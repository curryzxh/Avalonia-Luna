using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Luna.Mobile.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class RateDemoView : UserControl
{
    private static readonly string[] Texts = ["很差", "差", "一般", "好评", "优秀"];
    private static readonly string[] SpecialTexts = ["非常糟糕", "有些糟糕", "可以尝试", "可以前往", "推荐前往"];

    public event EventHandler? BackRequested;

    public RateDemoView()
    {
        InitializeComponent();

        TextRate1.Texts = Texts;
        TextRate2.Texts = Texts;
        TextRate3.Texts = Texts;

        PlaceTop.Texts = Texts;
        PlaceBottom.Texts = Texts;
        PlaceNone.Texts = Texts;

        SpecialRate.Texts = SpecialTexts;
        UpdateSpecialDesc(SpecialRate.Value);
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnSpecialValueChanged(object? sender, RateValueChangedEventArgs e)
    {
        UpdateSpecialDesc(e.NewValue);
    }

    private void UpdateSpecialDesc(double value)
    {
        var v = (int)Math.Round(Math.Clamp(value, 0, 5));
        if (v <= 0)
        {
            SpecialDesc.Text = string.Empty;
            SpecialDesc.Foreground = Brushes.Gray;
            return;
        }

        SpecialDesc.Text = SpecialTexts[Math.Clamp(v - 1, 0, SpecialTexts.Length - 1)];
        SpecialDesc.Foreground = v > 3 ? Brushes.Orange : Brushes.Black;
        SpecialDesc.FontWeight = v > 3 ? FontWeight.SemiBold : FontWeight.Normal;
    }
}

