using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using System.Threading.Tasks;

namespace Luna.Mobile.Sample.ViewModels;

public partial class PullDownRefreshDemoViewModel : DemoViewModelBase
{
    [ObservableProperty]
    private string refreshResultText = "最近刷新：尚未触发";

    [RelayCommand]
    private async Task RefreshAsync(PullDownRefresh? refreshControl)
    {
        RefreshResultText = "最近刷新：刷新中...";
        await Task.Delay(2000);

        if (refreshControl is not null)
        {
            refreshControl.IsRefreshing = false;
        }

        RefreshResultText = $"最近刷新：{DateTime.Now:HH:mm:ss}";
    }
}
