using Avalonia.Controls;
using Luna.Mobile.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class GuideDemoView : UserControl
{
    private GuideDemoViewModel ViewModel { get; } = new();

    public GuideDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        GuideHost.Finished += (_, _) => GuideHost.Close();
        GuideHost.SkipRequested += (_, _) => GuideHost.Close();
    }

    private void StartBaseGuide(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ConfigureGuide(
            showOverlay: true,
            new GuideStep
            {
                Target = BaseTitleTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.Center,
            },
            new GuideStep
            {
                Target = BaseLabelFieldTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.Bottom,
                HighlightPadding = 0,
            },
            new GuideStep
            {
                Target = BaseActionTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.BottomRight,
            });
    }

    private void StartNoMaskGuide(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ConfigureGuide(
            showOverlay: false,
            new GuideStep
            {
                Target = NoMaskTitleTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.Center,
            },
            new GuideStep
            {
                Target = NoMaskLabelFieldTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.Bottom,
                HighlightPadding = 0,
            },
            new GuideStep
            {
                Target = NoMaskActionTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.BottomRight,
            });
    }

    private void StartDialogGuide(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var dialogBody = BuildDialogBody();
        ConfigureGuide(
            showOverlay: true,
            new GuideStep
            {
                Target = DialogTitleTarget,
                Title = "用户引导标题",
                Content = dialogBody,
                Mode = GuideMode.Dialog,
            },
            new GuideStep
            {
                Target = DialogLabelFieldTarget,
                Title = "用户引导标题",
                Content = BuildDialogBody(),
                Mode = GuideMode.Dialog,
            },
            new GuideStep
            {
                Target = DialogActionTarget,
                Title = "用户引导标题",
                Content = BuildDialogBody(),
                Mode = GuideMode.Dialog,
            });
    }

    private void StartMixedGuide(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ConfigureGuide(
            showOverlay: true,
            new GuideStep
            {
                Target = MixedTitleTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.Center,
            },
            new GuideStep
            {
                Target = MixedLabelFieldTarget,
                Title = "用户引导标题",
                Content = BuildDialogBody(),
                Mode = GuideMode.Dialog,
            },
            new GuideStep
            {
                Target = MixedActionTarget,
                Title = "用户引导标题",
                Body = "用户引导的说明文案",
                Placement = GuidePlacement.BottomRight,
            });
    }

    private void StartCustomPopoverGuide(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ConfigureGuide(
            showOverlay: true,
            new GuideStep
            {
                Target = CustomTitleTarget,
                Content = BuildCustomPopoverContent(),
                Placement = GuidePlacement.Center,
            },
            new GuideStep
            {
                Target = CustomLabelFieldTarget,
                Content = BuildCustomPopoverContent(),
                Placement = GuidePlacement.Bottom,
                HighlightPadding = 0,
            },
            new GuideStep
            {
                Target = CustomActionTarget,
                Content = BuildCustomPopoverContent(),
                Placement = GuidePlacement.BottomRight,
            });
    }

    private void ConfigureGuide(bool showOverlay, params GuideStep[] steps)
    {
        GuideHost.Close();
        GuideHost.ShowOverlay = showOverlay;
        GuideHost.Mode = GuideMode.Popover;
        GuideHost.HideSkip = false;
        GuideHost.HideCounter = false;
        GuideHost.Steps.Clear();

        foreach (var step in steps)
        {
            GuideHost.Steps.Add(step);
        }

        GuideHost.Start();
    }

    private Control BuildDialogBody()
    {
        return new StackPanel
        {
            Spacing = 12,
            Children =
            {
                new TextBlock
                {
                    Text = "用户引导的说明文案",
                    TextWrapping = Avalonia.Media.TextWrapping.Wrap,
                    TextAlignment = Avalonia.Media.TextAlignment.Center
                },
                new Border
                {
                    CornerRadius = new CornerRadius(12),
                    Background = Avalonia.Media.Brush.Parse("#E8F3FF"),
                    Height = 120,
                    Child = new TextBlock
                    {
                        Text = "插图占位",
                        HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                        VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
                        Foreground = Avalonia.Media.Brush.Parse("#0052D9")
                    }
                }
            }
        };
    }

    private Control BuildCustomPopoverContent()
    {
        var panel = new StackPanel
        {
            Spacing = 12
        };

        panel.Children.Add(new TextBlock
        {
            Text = "自定义的图形或说明文案，用来解释或指导该功能使用。",
            TextWrapping = Avalonia.Media.TextWrapping.Wrap,
            Foreground = Avalonia.Media.Brush.Parse("#4B5563")
        });

        var actions = new StackPanel
        {
            Orientation = Avalonia.Layout.Orientation.Horizontal,
            Spacing = 8,
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Right
        };

        var skipButton = new Button
        {
            Content = "跳过",
            Classes = { "text", "small" }
        };
        skipButton.Click += (_, _) => GuideHost.Skip();

        var nextButton = new Button
        {
            Content = "下一步",
            Classes = { "primary", "small" }
        };
        nextButton.Click += (_, _) =>
        {
            if (GuideHost.IsLastStep)
            {
                GuideHost.Finish();
            }
            else
            {
                GuideHost.Next();
            }
        };

        actions.Children.Add(skipButton);
        actions.Children.Add(nextButton);
        panel.Children.Add(actions);

        return panel;
    }
}
