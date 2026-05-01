using Avalonia.Threading;
using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using Message = Luna.Mobile.Controls.Message;
using System;

namespace Luna.Mobile.Sample.ViewModels;

public partial class MessageDemoViewModel : DemoViewModelBase
{
    [RelayCommand]
    private void TextOnly()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条纯文字的消息通知 5s消失",
            Duration = TimeSpan.FromSeconds(5),
            ShowIcon = false,
        });
    }

    [RelayCommand]
    private void Icon()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条带图标的消息通知 5s消失",
            Duration = TimeSpan.FromSeconds(5),
            ShowIcon = true,
        });
    }

    [RelayCommand]
    private void Close()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条带关闭的消息通知 常驻可关闭",
            Duration = TimeSpan.Zero,
            ShowIcon = true,
            CloseBtn = true,
        });
    }

    [RelayCommand]
    private void Marquee()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条普通的通知信息，这是一条普通的通知信息，这是一条普通的通知信息",
            Duration = TimeSpan.FromSeconds(5),
            ShowIcon = false,
            Marquee = true,
        });
    }

    [RelayCommand]
    private void Link()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条带操作的消息通知",
            Duration = TimeSpan.Zero,
            ShowIcon = true,
            Link = "链接",
        });
    }

    [RelayCommand]
    private void Info()
    {
        Message.Info(new MessageOptions
        {
            Content = "这是一条普通消息通知",
            Duration = TimeSpan.FromSeconds(3),
            ShowIcon = true,
        });
    }

    [RelayCommand]
    private void Success()
    {
        Message.Success(new MessageOptions
        {
            Content = "这是一条需要成功的提示消息",
            Duration = TimeSpan.FromSeconds(3),
            ShowIcon = true,
        });
    }

    [RelayCommand]
    private void Warning()
    {
        Message.Warning(new MessageOptions
        {
            Content = "这是一条需要用户关注到的警示通知",
            Duration = TimeSpan.FromSeconds(3),
            ShowIcon = true,
        });
    }

    [RelayCommand]
    private void Error()
    {
        Message.Error(new MessageOptions
        {
            Content = "这是一条错误提示通知",
            Duration = TimeSpan.FromSeconds(3),
            ShowIcon = true,
        });
    }

    [RelayCommand]
    private void OpenMultiple()
    {
        var themes = new[] { MessageTheme.Info, MessageTheme.Warning, MessageTheme.Success, MessageTheme.Error };
        for (var i = 0; i < themes.Length; i++)
        {
            var index = i;
            var timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(300 * index),
            };
            timer.Tick += (_, _) =>
            {
                timer.Stop();
                var options = new MessageOptions
                {
                    Content = "这是一条通知信息",
                    Duration = TimeSpan.FromSeconds(5),
                    ShowIcon = true,
                };

                switch (themes[index])
                {
                    case MessageTheme.Warning:
                        Message.Warning(options);
                        break;
                    case MessageTheme.Success:
                        Message.Success(options);
                        break;
                    case MessageTheme.Error:
                        Message.Error(options);
                        break;
                    default:
                        Message.Info(options);
                        break;
                }
            };
            timer.Start();
        }
    }

    [RelayCommand]
    private void CloseAll()
    {
        Message.CloseAll();
    }
}
