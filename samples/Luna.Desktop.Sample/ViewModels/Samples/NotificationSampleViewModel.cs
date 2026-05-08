using System;
using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Avalonia.Threading;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public enum NotificationTheme { Info, Success, Warning, Error }

public enum NotificationPosition { TopRight, TopLeft, BottomRight, BottomLeft }

public sealed record NotificationItem(
    string Title,
    string Content,
    NotificationTheme Theme,
    DateTimeOffset Timestamp,
    TimeSpan Duration,
    bool ShowClose = true);

public partial class NotificationSampleViewModel()
    : SampleDetailViewModelBase("反馈", "Notification", "通知提醒，支持 info/success/warning/error 四种类型、位置、duration 和关闭操作。")
{
    public ObservableCollection<NotificationItem> ActiveNotifications { get; } = new();

    [ObservableProperty]
    private NotificationPosition selectedPosition = NotificationPosition.TopRight;

    [ObservableProperty]
    private double durationSeconds = 5;

    [ObservableProperty]
    private int notificationCount;

    [RelayCommand]
    private void ShowInfoNotification()
    {
        AddNotification("信息通知", "这是一条信息通知消息。", NotificationTheme.Info);
    }

    [RelayCommand]
    private void ShowSuccessNotification()
    {
        AddNotification("提交成功", "数据已保存，可以继续下一步。", NotificationTheme.Success);
    }

    [RelayCommand]
    private void ShowWarningNotification()
    {
        AddNotification("需要注意", "部分配置还没有完成。", NotificationTheme.Warning);
    }

    [RelayCommand]
    private void ShowErrorNotification()
    {
        AddNotification("操作失败", "网络连接异常，请稍后重试。", NotificationTheme.Error);
    }

    [RelayCommand]
    private void ClearAllNotifications()
    {
        ActiveNotifications.Clear();
        NotificationCount = 0;
    }

    private void AddNotification(string title, string content, NotificationTheme theme)
    {
        var item = new NotificationItem(
            title,
            content,
            theme,
            DateTimeOffset.Now,
            TimeSpan.FromSeconds(DurationSeconds));

        ActiveNotifications.Add(item);
        NotificationCount = ActiveNotifications.Count;

        if (DurationSeconds > 0)
        {
            DispatcherTimer.RunOnce(() =>
            {
                if (ActiveNotifications.Contains(item))
                {
                    ActiveNotifications.Remove(item);
                    NotificationCount = ActiveNotifications.Count;
                }
            }, TimeSpan.FromSeconds(DurationSeconds));
        }
    }
}
