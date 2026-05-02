using Avalonia.Controls;
using Luna.Mobile.Controls;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class CascaderDemoView : UserControl
{
    private CascaderDemoViewModel ViewModel { get; } = new();

    public CascaderDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.CascaderRequested += OnCascaderRequested;
        CascaderHost.Changed += OnCascaderChanged;
    }

    private void OnCascaderRequested(CascaderOptions options)
    {
        CascaderHost.Show(options);
    }

    private void OnCascaderChanged(object? sender, CascaderChangedEventArgs e)
    {
        ViewModel.OnCascaderChanged(e);
    }
}
