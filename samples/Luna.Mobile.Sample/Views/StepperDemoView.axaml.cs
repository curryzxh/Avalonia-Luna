using Avalonia.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class StepperDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public StepperDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
