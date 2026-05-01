using Avalonia.Controls;
using Luna.Mobile.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class DrawerDemoView : UserControl
{
        private DrawerDemoViewModel ViewModel { get; } = new();

    public DrawerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnOpenLeftClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Drawer.Show(new DrawerOptions
        {
            Title = "左侧抽屉",
            Placement = Luna.Mobile.Controls.DrawerPlacement.Left,
            Content = DrawerContent(),
        });
    }

    private void OnOpenRightClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Drawer.Show(new DrawerOptions
        {
            Title = "右侧抽屉",
            Placement = Luna.Mobile.Controls.DrawerPlacement.Right,
            Content = DrawerContent(),
        });
    }

    private void OnOpenNoOverlayClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Drawer.Show(new DrawerOptions
        {
            Title = "无遮罩",
            Placement = Luna.Mobile.Controls.DrawerPlacement.Right,
            ShowOverlay = false,
            Content = DrawerContent(),
        });
    }

    private void OnOpenNoOverlayCloseClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        Drawer.Show(new DrawerOptions
        {
            Title = "遮罩不可点击关闭",
            Placement = Luna.Mobile.Controls.DrawerPlacement.Right,
            CloseOnOverlayClick = false,
            Content = DrawerContent(),
        });
    }

    private static Control DrawerContent()
    {
        var closeButton = new Avalonia.Controls.Button
        {
            Content = "关闭",
            HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Stretch,
        };
        closeButton.Classes.Add("primary");
        closeButton.Classes.Add("large");
        closeButton.Click += (_, _) => Drawer.Close();

        return new StackPanel
        {
            Spacing = 12,
            Children =
            {
                new TextBlock { Text = "抽屉内容", FontSize = 16 },
                closeButton
            }
        };
    }
}
