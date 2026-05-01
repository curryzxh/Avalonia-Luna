using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using CommunityToolkit.Mvvm.Input;
using Luna.Mobile.Controls;
using Dialog = Luna.Mobile.Controls.Dialog;
using Toast = Luna.Mobile.Controls.Toast;

namespace Luna.Mobile.Sample.ViewModels;

public partial class DialogDemoViewModel : DemoViewModelBase
{
    public void OnDialogConfirmed() => Toast.Show("confirm");

    public void OnDialogCanceled() => Toast.Show("cancel");

    public void OnDialogClosed() => Toast.Show("close");

    [RelayCommand]
    private void FeedbackWithTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
            CloseOnOverlayClick = false,
        });
    }

    [RelayCommand]
    private void FeedbackNoTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
            CloseOnOverlayClick = false,
        });
    }

    [RelayCommand]
    private void FeedbackOnlyTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            ConfirmButton = new DialogButtonOptions { Content = "知道了", Theme = DialogButtonTheme.Primary },
        });
    }

    [RelayCommand]
    private void FeedbackLongContent()
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

    [RelayCommand]
    private void ConfirmWithTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    [RelayCommand]
    private void ConfirmNoTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Content = "告知当前状态、信息和解决方法，等内容。描述文案尽可能控制在三行内",
            ConfirmButton = new DialogButtonOptions { Content = "警示操作", Theme = DialogButtonTheme.Danger },
            CancelButton = new DialogButtonOptions { Content = "取消" },
            CloseOnOverlayClick = true,
        });
    }

    [RelayCommand]
    private void ConfirmOnlyTitle()
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    [RelayCommand]
    private void InputNoDesc()
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

    [RelayCommand]
    private void InputWithDesc()
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

    [RelayCommand]
    private void ImageTopWithAll() => ShowImageDialog("top", true, true);

    [RelayCommand]
    private void ImageTopNoTitle() => ShowImageDialog("top", false, true);

    [RelayCommand]
    private void ImageTopOnlyTitle() => ShowImageDialog("top", true, false);

    [RelayCommand]
    private void ImageTopOnlyImage() => ShowImageDialog("top", false, false);

    [RelayCommand]
    private void ImageMiddleWithAll() => ShowImageDialog("middle", true, true);

    [RelayCommand]
    private void ImageMiddleOnlyTitle() => ShowImageDialog("middle", true, false);

    [RelayCommand]
    private void TextButtons()
    {
        Dialog.Show(new DialogOptions
        {
            Title = "对话框标题",
            Content = "告知当前状态、信息和解决方法",
            ConfirmButton = new DialogButtonOptions { Content = "确认", Theme = DialogButtonTheme.Primary },
            CancelButton = new DialogButtonOptions { Content = "取消" },
        });
    }

    [RelayCommand]
    private void HorizontalButtons()
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

    [RelayCommand]
    private void VerticalButtons()
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

    [RelayCommand]
    private void CloseButtonDialog()
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

    private static void ShowImageDialog(string placement, bool hasTitle, bool hasContent)
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
}
