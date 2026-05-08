using Avalonia;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Luna.Desktop.Sample.ViewModels.Samples;

public partial class ImageViewerSampleViewModel : SampleDetailViewModelBase
{
    public ImageViewerSampleViewModel() : base("基础", "ImageViewer", "图片预览控件，支持放大、缩小、旋转和拖拽平移。")
    {
        Source = CreateSampleImage();
    }

    [ObservableProperty]
    private double _zoom = 1.0;

    [ObservableProperty]
    private double _rotate;

    [ObservableProperty]
    private IImage? _source;

    private static Bitmap CreateSampleImage()
    {
        var width = 640;
        var height = 400;
        var bitmap = new WriteableBitmap(
            new PixelSize(width, height),
            new Vector(96, 96),
            PixelFormat.Bgra8888,
            AlphaFormat.Premul);

        using var fb = bitmap.Lock();
        var rowBytes = fb.RowBytes;
        var buffer = new byte[fb.Size.Height * rowBytes];
        for (var y = 0; y < height; y++)
        {
            for (var x = 0; x < width; x++)
            {
                var r = (byte)(x * 255 / width);
                var g = (byte)(y * 255 / height);
                var b = (byte)128;
                var offset = y * rowBytes + x * 4;
                buffer[offset] = b;
                buffer[offset + 1] = g;
                buffer[offset + 2] = r;
                buffer[offset + 3] = 0xFF;
            }
        }

        System.Runtime.InteropServices.Marshal.Copy(buffer, 0, fb.Address, buffer.Length);
        return bitmap;
    }
}
