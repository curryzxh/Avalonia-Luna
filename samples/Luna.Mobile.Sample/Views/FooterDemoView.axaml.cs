using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class FooterDemoView : UserControl
{
    private FooterDemoViewModel ViewModel { get; } = new();

    public FooterDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
