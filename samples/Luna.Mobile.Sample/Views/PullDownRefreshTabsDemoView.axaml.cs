using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class PullDownRefreshTabsDemoView : UserControl
{
    private PullDownRefreshTabsDemoViewModel ViewModel { get; } = new();

    public PullDownRefreshTabsDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
