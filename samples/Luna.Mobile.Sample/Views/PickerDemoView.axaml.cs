using Avalonia.Controls;
using Luna.Mobile.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class PickerDemoView : UserControl
{
    private PickerDemoViewModel ViewModel { get; } = new();

    public PickerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.PickerRequested += OnPickerRequested;
        PickerHost.Confirmed += OnPickerConfirmed;
    }

    private void OnPickerRequested(PickerOptions options)
    {
        PickerHost.Show(options);
    }

    private void OnPickerConfirmed(object? sender, PickerConfirmedEventArgs e)
    {
        ViewModel.OnPickerConfirmed(e);
    }
}
