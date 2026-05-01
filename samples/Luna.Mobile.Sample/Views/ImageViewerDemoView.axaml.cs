using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Luna.Mobile.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class ImageViewerDemoView : UserControl
{
    private ImageViewerDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public ImageViewerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }

    private void OnBasePreviewClick(object? sender, RoutedEventArgs e)
    {
        var viewer = this.FindControl<ImageViewer>("PART_Viewer");
        if (viewer is null) return;

        viewer.Images =
        [
            CreatePlaceholderImage("图片 1", Color.FromRgb(100, 149, 237)),
            CreatePlaceholderImage("图片 2", Color.FromRgb(255, 127, 80)),
        ];
        viewer.ShowIndex = false;
        viewer.ShowDelete = false;
        viewer.CurrentIndex = 0;
        viewer.Show();
    }

    private void OnAlignPreviewClick(object? sender, RoutedEventArgs e)
    {
        var viewer = this.FindControl<ImageViewer>("PART_Viewer");
        if (viewer is null) return;

        viewer.Images =
        [
            CreatePlaceholderImage("图片 1 (start)", Color.FromRgb(100, 149, 237)),
            CreatePlaceholderImage("图片 2 (end)", Color.FromRgb(255, 127, 80)),
            CreatePlaceholderImage("图片 3 (center)", Color.FromRgb(60, 179, 113)),
        ];
        viewer.ShowIndex = false;
        viewer.ShowDelete = false;
        viewer.CurrentIndex = 0;
        viewer.Show();
    }

    private void OnOperationPreviewClick(object? sender, RoutedEventArgs e)
    {
        var viewer = this.FindControl<ImageViewer>("PART_Viewer");
        if (viewer is null) return;

        viewer.Images =
        [
            CreatePlaceholderImage("图片 1", Color.FromRgb(100, 149, 237)),
            CreatePlaceholderImage("图片 2", Color.FromRgb(255, 127, 80)),
        ];
        viewer.ShowIndex = true;
        viewer.ShowDelete = true;
        viewer.CurrentIndex = 0;
        viewer.Show();
    }

    private static Border CreatePlaceholderImage(string text, Color color)
    {
        return new Border
        {
            Background = new SolidColorBrush(color),
            Width = 300,
            Height = 400,
            CornerRadius = new CornerRadius(8),
            Child = new TextBlock
            {
                Text = text,
                Foreground = Brushes.White,
                FontSize = 24,
                HorizontalAlignment = Avalonia.Layout.HorizontalAlignment.Center,
                VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            },
        };
    }
}
