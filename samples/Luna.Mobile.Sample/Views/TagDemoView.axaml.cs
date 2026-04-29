using Avalonia.Controls;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class TagDemoView : UserControl
{
    public event EventHandler? BackRequested;

    public TagDemoView()
    {
        InitializeComponent();
    }

    private void OnBackClick(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        BackRequested?.Invoke(this, EventArgs.Empty);
    }
}
