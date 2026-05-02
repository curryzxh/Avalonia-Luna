using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class FabDemoView : UserControl
{
    private FabDemoViewModel ViewModel { get; } = new();

    public FabDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
