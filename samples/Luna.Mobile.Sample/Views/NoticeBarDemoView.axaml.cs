using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using Toast = Luna.Mobile.Controls.Toast;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class NoticeBarDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public NoticeBarDemoView()
    {
        InitializeComponent();
        VerticalNotice.ContentList =
        [
            "君不见",
            "高堂明镜悲白发",
            "朝如青丝暮成雪",
            "人生得意须尽欢",
            "莫使金樽空对月",
        ];
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }

    private void OnCloseNoticeCloseRequested(object? sender, EventArgs e)
    {
        CloseNotice.Visible = false;
    }

    private void OnEntrancePressed(object? sender, PointerPressedEventArgs e)
    {
        Toast.Show("click:NoticeBar");
    }

    private void OnCustomSuffixRequested(object? sender, EventArgs e)
    {
        CustomNotice.Visible = false;
    }
}
