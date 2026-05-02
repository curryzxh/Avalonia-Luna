using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Threading.Tasks;

namespace Luna.Mobile.Sample.Views;

public partial class PullDownRefreshDemoView : UserControl
{
    public PullDownRefreshDemoView()
    {
        InitializeComponent();
    }

    private async void OnRefreshRequested(object? sender, RoutedEventArgs e)
    {
        RefreshResultText.Text = "最近刷新：刷新中...";
        await Task.Delay(2000);

        DemoPullDownRefresh.IsRefreshing = false;
        RefreshResultText.Text = $"最近刷新：{DateTime.Now:HH:mm:ss}";
    }
}
