using Avalonia.Controls;
using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using Luna.Mobile.Controls;
using System;
using System.Collections.Generic;

namespace Luna.Mobile.Sample.Views;

public partial class ActionSheetDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public ActionSheetDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnNormalListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            CancelText = "cancel",
            Items = BasicListItems(),
        });
    }

    private void OnMethodListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheet.Show(new ActionSheetOptions
        {
            CancelText = "cancel",
            Items = BasicListItems(),
        });
    }

    private void OnDescriptionListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Description = "Email Settings",
            CancelText = "cancel",
            Items = BasicListItems(),
        });
    }

    private void OnIconListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            CancelText = "cancel",
            Items = new List<ActionSheetItem>
            {
                new() { Label = "Move", Icon = CreateIcon() },
                new() { Label = "Mark as important", Icon = CreateIcon() },
                new() { Label = "Unsubscribe", Icon = CreateIcon() },
                new() { Label = "Add to Tasks", Icon = CreateIcon() },
            },
        });
    }

    private void OnBadgeListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Description = "Email Settings",
            CancelText = "cancel",
            Items = new List<ActionSheetItem>
            {
                new() { Label = "Move", BadgeDot = true },
                new() { Label = "Mark as important", BadgeCount = "8" },
                new() { Label = "Unsubscribe", BadgeCount = "99" },
                new() { Label = "Add to Tasks", BadgeCount = "1000" },
            },
        });
    }

    private void OnNormalGridClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Items = GridItems(),
        });
    }

    private void OnDescriptionGridClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Description = "动作面板描述文字",
            Items = GridItems(),
        });
    }

    private void OnBadgeGridClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Description = "带徽标宫格型",
            Items = new[]
            {
                new ActionSheetItem { Label = "微信", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "朋友圈", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "QQ", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "企业微信", Icon = CreateIcon(), BadgeCount = "99" },
                new ActionSheetItem { Label = "收藏", Icon = CreateIcon() },
                new ActionSheetItem { Label = "刷新", Icon = CreateIcon() },
                new ActionSheetItem { Label = "下载", Icon = CreateIcon() },
                new ActionSheetItem { Label = "复制", Icon = CreateIcon() },
            },
        });
    }

    private void OnStatusListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Description = "列表型选项状态",
            CancelText = "cancel",
            Items = new List<ActionSheetItem>
            {
                new() { Label = "Move", Icon = CreateIcon() },
                new() { Label = "Mark as important", Icon = CreateIcon(), Foreground = new ImmutableSolidColorBrush(Color.Parse("#0052D9")) },
                new() { Label = "Unsubscribe", Icon = CreateIcon(), Foreground = new ImmutableSolidColorBrush(Color.Parse("#E34D59")) },
                new() { Label = "Add to Tasks", Icon = CreateIcon(), IsDisabled = true },
            },
        });
    }

    private void OnAlignCenterListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Align = ActionSheetAlign.Center,
            Description = "Email Settings",
            CancelText = "cancel",
            Items = new List<ActionSheetItem>
            {
                new() { Label = "Move", Icon = CreateIcon() },
                new() { Label = "Mark as important", Icon = CreateIcon() },
                new() { Label = "Unsubscribe", Icon = CreateIcon() },
                new() { Label = "Add to Tasks", Icon = CreateIcon() },
            },
        });
    }

    private void OnAlignLeftListClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        ActionSheetHost.Show(new ActionSheetOptions
        {
            Align = ActionSheetAlign.Left,
            Description = "Email Settings",
            CancelText = "cancel",
            Items = new List<ActionSheetItem>
            {
                new() { Label = "Move", Icon = CreateIcon() },
                new() { Label = "Mark as important", Icon = CreateIcon() },
                new() { Label = "Unsubscribe", Icon = CreateIcon() },
                new() { Label = "Add to Tasks", Icon = CreateIcon() },
            },
        });
    }

    private static IReadOnlyList<ActionSheetItem> BasicListItems()
    {
        return new List<ActionSheetItem>
        {
            new() { Label = "Move" },
            new() { Label = "Mark as important" },
            new() { Label = "Unsubscribe" },
            new() { Label = "Add to Tasks" },
        };
    }

    private static ActionSheetItem[] GridItems()
    {
        return new[]
        {
            new ActionSheetItem { Label = "微信", Icon = CreateIcon() },
            new ActionSheetItem { Label = "朋友圈", Icon = CreateIcon() },
            new ActionSheetItem { Label = "QQ", Icon = CreateIcon() },
            new ActionSheetItem { Label = "企业微信", Icon = CreateIcon() },
            new ActionSheetItem { Label = "收藏", Icon = CreateIcon() },
            new ActionSheetItem { Label = "刷新", Icon = CreateIcon() },
            new ActionSheetItem { Label = "下载", Icon = CreateIcon() },
            new ActionSheetItem { Label = "复制", Icon = CreateIcon() },
        };
    }

    private static Avalonia.Controls.Shapes.Path CreateIcon()
    {
        return new Avalonia.Controls.Shapes.Path
        {
            Width = 22,
            Height = 22,
            Data = Geometry.Parse("M12 2C6.477 2 2 6.477 2 12C2 17.523 6.477 22 12 22C17.523 22 22 17.523 22 12C22 6.477 17.523 2 12 2Z M8 12L11 15L16 9"),
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Fill = Brushes.Transparent,
            Stretch = Stretch.Uniform
        };
    }
}
