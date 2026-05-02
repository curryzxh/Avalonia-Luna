using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class StepsDemoView : UserControl
{
    private StepsDemoViewModel ViewModel { get; } = new();

    public StepsDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
