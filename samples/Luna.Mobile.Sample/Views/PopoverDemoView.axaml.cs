using Avalonia.Controls;
using Avalonia.Interactivity;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class PopoverDemoView : UserControl
{
        private PopoverDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public PopoverDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
