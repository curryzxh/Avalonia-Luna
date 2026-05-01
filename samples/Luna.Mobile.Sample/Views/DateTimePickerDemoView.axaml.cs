using Avalonia.Controls;
using Luna.Mobile.Controls;
using System;
using Luna.Mobile.Sample.ViewModels;

namespace Luna.Mobile.Sample.Views;

public partial class DateTimePickerDemoView : UserControl
{
    private DateTimePickerDemoViewModel ViewModel { get; } = new();

    public DateTimePickerDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        ViewModel.DateTimePickerRequested += OnDateTimePickerRequested;
        DateTimePickerHost.Confirmed += OnDateTimePickerConfirmed;
    }

    private void OnDateTimePickerRequested(DateTimePickerOptions options)
    {
        DateTimePickerHost.Show(options);
    }

    private void OnDateTimePickerConfirmed(object? sender, DateTimePickerConfirmedEventArgs e)
    {
        ViewModel.OnDateTimePickerConfirmed(e);
    }
}
