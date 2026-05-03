using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class PullDownRefreshDemoView : UserControl
{
    private PullDownRefreshDemoViewModel ViewModel { get; } = new();

    public PullDownRefreshDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
