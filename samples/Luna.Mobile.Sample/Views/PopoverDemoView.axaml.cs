using Avalonia.Controls;
using Avalonia.Interactivity;

namespace Luna.Mobile.Sample.Views;

public partial class PopoverDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public PopoverDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
