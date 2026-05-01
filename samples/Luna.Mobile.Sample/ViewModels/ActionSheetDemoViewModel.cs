using Avalonia.Controls.Shapes;
using Avalonia.Media;
using Avalonia.Media.Immutable;
using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using System;
using System.Collections.Generic;
using ShapePath = Avalonia.Controls.Shapes.Path;

namespace Luna.Mobile.Sample.ViewModels;

public sealed class ActionSheetRequest
{
    public ActionSheetRequest(ActionSheetOptions options, bool useStaticApi = false)
    {
        Options = options;
        UseStaticApi = useStaticApi;
    }

    public ActionSheetOptions Options { get; }

    public bool UseStaticApi { get; }
}

public partial class ActionSheetDemoViewModel : DemoViewModelBase
{
    public event Action<ActionSheetRequest>? ActionSheetRequested;

    [RelayCommand]
    private void NormalList()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            CancelText = "cancel",
            Items = BasicListItems(),
        });
    }

    [RelayCommand]
    private void MethodList()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            CancelText = "cancel",
            Items = BasicListItems(),
        }, useStaticApi: true);
    }

    [RelayCommand]
    private void DescriptionList()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            Description = "Email Settings",
            CancelText = "cancel",
            Items = BasicListItems(),
        });
    }

    [RelayCommand]
    private void IconList()
    {
        RequestActionSheet(new ActionSheetOptions
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

    [RelayCommand]
    private void BadgeList()
    {
        RequestActionSheet(new ActionSheetOptions
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

    [RelayCommand]
    private void NormalGrid()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Items = GridItems(),
        });
    }

    [RelayCommand]
    private void DescriptionGrid()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Description = "动作面板描述文字",
            Items = GridItems(),
        });
    }

    [RelayCommand]
    private void BadgeGrid()
    {
        RequestActionSheet(new ActionSheetOptions
        {
            Theme = ActionSheetTheme.Grid,
            Description = "带徽标宫格型",
            Items =
            [
                new ActionSheetItem { Label = "微信", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "朋友圈", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "QQ", Icon = CreateIcon(), BadgeDot = true },
                new ActionSheetItem { Label = "企业微信", Icon = CreateIcon(), BadgeCount = "99" },
                new ActionSheetItem { Label = "收藏", Icon = CreateIcon() },
                new ActionSheetItem { Label = "刷新", Icon = CreateIcon() },
                new ActionSheetItem { Label = "下载", Icon = CreateIcon() },
                new ActionSheetItem { Label = "复制", Icon = CreateIcon() },
            ],
        });
    }

    [RelayCommand]
    private void StatusList()
    {
        RequestActionSheet(new ActionSheetOptions
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

    [RelayCommand]
    private void AlignCenterList()
    {
        RequestActionSheet(new ActionSheetOptions
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

    [RelayCommand]
    private void AlignLeftList()
    {
        RequestActionSheet(new ActionSheetOptions
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

    private void RequestActionSheet(ActionSheetOptions options, bool useStaticApi = false)
    {
        ActionSheetRequested?.Invoke(new ActionSheetRequest(options, useStaticApi));
    }

    private static IReadOnlyList<ActionSheetItem> BasicListItems()
    {
        return
        [
            new ActionSheetItem { Label = "Move" },
            new ActionSheetItem { Label = "Mark as important" },
            new ActionSheetItem { Label = "Unsubscribe" },
            new ActionSheetItem { Label = "Add to Tasks" },
        ];
    }

    private static ActionSheetItem[] GridItems()
    {
        return
        [
            new ActionSheetItem { Label = "微信", Icon = CreateIcon() },
            new ActionSheetItem { Label = "朋友圈", Icon = CreateIcon() },
            new ActionSheetItem { Label = "QQ", Icon = CreateIcon() },
            new ActionSheetItem { Label = "企业微信", Icon = CreateIcon() },
            new ActionSheetItem { Label = "收藏", Icon = CreateIcon() },
            new ActionSheetItem { Label = "刷新", Icon = CreateIcon() },
            new ActionSheetItem { Label = "下载", Icon = CreateIcon() },
            new ActionSheetItem { Label = "复制", Icon = CreateIcon() },
        ];
    }

    private static ShapePath CreateIcon()
    {
        return new ShapePath
        {
            Width = 22,
            Height = 22,
            Data = Geometry.Parse("M12 2C6.477 2 2 6.477 2 12C2 17.523 6.477 22 12 22C17.523 22 22 17.523 22 12C22 6.477 17.523 2 12 2Z M8 12L11 15L16 9"),
            Stroke = Brushes.Black,
            StrokeThickness = 2,
            Fill = Brushes.Transparent,
            Stretch = Stretch.Uniform,
        };
    }
}
