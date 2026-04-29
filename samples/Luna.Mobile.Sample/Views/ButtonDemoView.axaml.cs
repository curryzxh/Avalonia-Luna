using Avalonia.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class ButtonDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public ButtonDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
