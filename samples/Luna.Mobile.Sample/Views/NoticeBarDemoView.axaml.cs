using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Luna.Mobile.Controls;
using Toast = Luna.Mobile.Controls.Toast;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class NoticeBarDemoView : UserControl
{
        private NoticeBarDemoViewModel ViewModel { get; } = new();

    public NoticeBarDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        VerticalNotice.ContentList =
        [
            "君不见",
            "高堂明镜悲白发",
            "朝如青丝暮成雪",
            "人生得意须尽欢",
            "莫使金樽空对月",
        ];
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
