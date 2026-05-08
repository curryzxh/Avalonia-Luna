using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class QRCodeSampleViewModel : SampleDetailViewModelBase
{
    public QRCodeSampleViewModel() : base("数据展示", "QRCode", "二维码生成和展示。需要集成 ZXing 或 QRCoder 等第三方库。")
    {
    }

    [ObservableProperty]
    private string _content = "https://github.com/luna-desktop";

    [ObservableProperty]
    private int _size = 160;

    [ObservableProperty]
    private string _statusText = "二维码功能需要集成第三方库（如 ZXing.Net 或 QRCoder）。当前展示 API 设计预览。";
}
