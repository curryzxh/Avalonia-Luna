using Avalonia.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class RadioDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public RadioDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
