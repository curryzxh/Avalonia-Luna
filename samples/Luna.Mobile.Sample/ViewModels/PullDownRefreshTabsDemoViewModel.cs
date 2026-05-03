using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using System.Threading.Tasks;

namespace Luna.Mobile.Sample.ViewModels;

public partial class PullDownRefreshTabsDemoViewModel : DemoViewModelBase
{
    [ObservableProperty]
    private string firstTabRefreshResultText = "页签一：尚未触发";

    [ObservableProperty]
    private string secondTabRefreshResultText = "页签二：尚未触发";

    [RelayCommand]
    private async Task RefreshFirstTabAsync(PullDownRefresh? refreshControl)
    {
        FirstTabRefreshResultText = "页签一：刷新中...";
        await Task.Delay(1500);

        if (refreshControl is not null)
        {
            refreshControl.IsRefreshing = false;
        }

        FirstTabRefreshResultText = $"页签一：{DateTime.Now:HH:mm:ss}";
    }

    [RelayCommand]
    private async Task RefreshSecondTabAsync(PullDownRefresh? refreshControl)
    {
        SecondTabRefreshResultText = "页签二：刷新中...";
        await Task.Delay(1500);

        if (refreshControl is not null)
        {
            refreshControl.IsRefreshing = false;
        }

        SecondTabRefreshResultText = $"页签二：{DateTime.Now:HH:mm:ss}";
    }
}
