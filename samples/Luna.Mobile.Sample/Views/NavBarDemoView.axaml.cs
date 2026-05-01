using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class NavBarDemoView : UserControl
{
    private NavBarDemoViewModel ViewModel { get; } = new();

    public NavBarDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
