using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class ButtonDemoView : UserControl
{
    private ButtonDemoViewModel ViewModel { get; } = new();

    public ButtonDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
