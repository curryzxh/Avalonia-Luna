using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Layout;
using Avalonia.Media;
using Luna.Mobile.Controls;
using Dialog = Luna.Mobile.Controls.Dialog;
using Toast = Luna.Mobile.Controls.Toast;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class DialogDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public DialogDemoView()
    {
        InitializeComponent();
        DialogHost.Confirmed += (_, _) => Toast.Show("confirm");
        DialogHost.Canceled += (_, _) => Toast.Show("cancel");
        DialogHost.Closed += (_, _) => Toast.Show("close");
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnFeedbackWithTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
            CloseOnOverlayClick = false,
        });
    }

    private void OnFeedbackNoTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
            CloseOnOverlayClick = false,
        });
    }

    private void OnFeedbackOnlyTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
        });
    }

    private void OnFeedbackLongContentClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案。这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案。这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案，这里是辅助内容文案。",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
            MaxContentHeight = 100,
            CloseOnOverlayClick = true,
        });
    }

    private void OnConfirmWithTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    private void OnConfirmNoTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "警示操作", Theme = DialogButtonTheme.Danger },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            CloseOnOverlayClick = true,
        });
    }

    private void OnConfirmOnlyTitleClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    private void OnInputNoDescClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Middle = new TextBox { PlaceholderText = "请输入文字" },
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            CloseOnOverlayClick = true,
        });
    }

    private void OnInputWithDescClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法等，描述文案尽可能控制在三行内",
            Middle = new TextBox { PlaceholderText = "请输入文字" },
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            CloseOnOverlayClick = true,
        });
    }

    private void OnImageTopWithAllClick(object? sender, RoutedEventArgs e) => ShowImageDialog("top", true, true);
    private void OnImageTopNoTitleClick(object? sender, RoutedEventArgs e) => ShowImageDialog("top", false, true);
    private void OnImageTopOnlyTitleClick(object? sender, RoutedEventArgs e) => ShowImageDialog("top", true, false);
    private void OnImageTopOnlyImageClick(object? sender, RoutedEventArgs e) => ShowImageDialog("top", false, false);
    private void OnImageMiddleWithAllClick(object? sender, RoutedEventArgs e) => ShowImageDialog("middle", true, true);
    private void OnImageMiddleOnlyTitleClick(object? sender, RoutedEventArgs e) => ShowImageDialog("middle", true, false);

    private void ShowImageDialog(string placement, bool hasTitle, bool hasContent)
    {
        var image = new Border
        {
            Width = 260,
            Height = 120,
            Background = Brushes.WhiteSmoke,
            CornerRadius = new Avalonia.CornerRadius(6),
            Child = new TextBlock
            {
                Text = "Image",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = Brushes.Gray,
            },
        };

        Dialog.Show(new DialogOptions
        {
            Title = hasTitle ? "对话框标题" : null,
            Content = hasContent ? "告知当前状态、信息和解决方法" : null,
            Top = placement == "top" ? image : null,
            Middle = placement == "middle" ? image : null,
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            CloseOnOverlayClick = true,
        });
    }

    private void OnTextButtonsClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    private void OnHorizontalButtonsClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            ButtonLayout = DialogButtonLayout.Horizontal,
        });
    }

    private void OnVerticalButtonsClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            ButtonLayout = DialogButtonLayout.Vertical,
        });
    }

    private void OnCloseButtonDialogClick(object? sender, RoutedEventArgs e)
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            ButtonLayout = DialogButtonLayout.Vertical,
            ShowCloseButton = true,
        });
    }
}
