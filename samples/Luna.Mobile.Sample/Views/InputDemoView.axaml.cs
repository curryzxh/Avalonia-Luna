using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class InputDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public InputDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}

