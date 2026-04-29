using Avalonia.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class BadgeDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public BadgeDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
