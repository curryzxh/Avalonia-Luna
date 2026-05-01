using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class DialogDemoView : UserControl
{
    private DialogDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public DialogDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
        DialogHost.Confirmed += (_, _) => ViewModel.OnDialogConfirmed();
        DialogHost.Canceled += (_, _) => ViewModel.OnDialogCanceled();
        DialogHost.Closed += (_, _) => ViewModel.OnDialogClosed();
    }
}
