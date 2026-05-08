using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class ImageViewerSampleViewModel : SampleDetailViewModelBase
{
    public ImageViewerSampleViewModel() : base("基础", "ImageViewer", "图片预览控件，支持放大、缩小和旋转。")
    {
    }

    [ObservableProperty]
    private double _zoom = 1.0;

    [ObservableProperty]
    private double _rotate;

    [ObservableProperty]
    private string _imageUrl = "https://coresg-normal.trae.ai/api/ide/v1/text_to_image?prompt=a%20beautiful%20mountain%20landscape%20with%20lake%20reflection%20at%20sunset%2C%20photorealistic&image_size=landscape_16_9";
}
