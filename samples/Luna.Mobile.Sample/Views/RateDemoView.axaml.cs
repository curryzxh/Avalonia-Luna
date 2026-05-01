using Avalonia.Controls;
using Luna.Mobile.Sample.ViewModels;
using System;

namespace Luna.Mobile.Sample.Views;

public partial class RateDemoView : UserControl
{
    private RateDemoViewModel ViewModel { get; } = new();

    public event EventHandler? BackRequested
    {
        add => ViewModel.BackRequested += value;
        remove => ViewModel.BackRequested -= value;
    }

    public RateDemoView()
    {
        InitializeComponent();
        DataContext = ViewModel;
    }
}
