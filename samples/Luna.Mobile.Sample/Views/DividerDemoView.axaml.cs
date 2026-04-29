using Avalonia.Controls;
using Avalonia.Interactivity;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class DividerDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public DividerDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}

