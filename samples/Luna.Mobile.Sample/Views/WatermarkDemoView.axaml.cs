using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class WatermarkDemoView : UserControl
{
    private WatermarkDemoViewModel ViewModel { get; } = new();

    public WatermarkDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
